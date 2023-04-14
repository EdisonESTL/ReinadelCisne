using ReinadelCisne.Auxiliars;
using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class DetailOrdenProduccionVM : BaseVM, IQueryAttributable
    {
        int _totalElementos;
        public int TotalElementos
        {
            get => _totalElementos;
            set
            {
                if (value != _totalElementos)
                {
                    _totalElementos = value;
                    OnPropertyChanged();
                }
            }
        }

        double _totalValores;
        public double TotalValores
        {
            get => _totalValores;
            set
            {
                if (value != _totalValores)
                {
                    _totalValores = value;
                    OnPropertyChanged();
                }
            }
        }

        OrderProduccionModel orderResp;
        public OrderProduccionModel OrderResp
        {
            get => orderResp;
            set
            {
                if(value != orderResp)
                {
                    orderResp = value;
                    OnPropertyChanged();
                }
            }
        }

        string mensajeConfirmacion;
        public string MensajeConfirmacion
        {
            get => mensajeConfirmacion;
            set
            {
                if(value != mensajeConfirmacion)
                {
                    mensajeConfirmacion = value;
                    OnPropertyChanged();
                }
            }
        }

        bool habilitarButtonAcept;
        public bool HabilitarButtonAcept
        {
            get => habilitarButtonAcept;
            set
            {
                if(value != habilitarButtonAcept)
                {
                    habilitarButtonAcept = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<ItemsListRMModel> MaterialProducir { get; set; } = new ObservableCollection<ItemsListRMModel>();

        public ICommand PushCommand => new Command((obj) =>
        {
            string butt = obj as string;
            Direccionar(butt);
        });
        private void Direccionar(string butt)
        {
            switch (butt)
            {
                case "aprobar":
                    AprobarUsoRM();
                    break;
                case "cancelar":
                    
                    //CancelarUsoRM();
                    break;
                default:
                    break;
            }
        }

        private void AprobarUsoRM()
        {
            foreach(var mt in MaterialProducir)
            {
                var saldos = App.Database.GetSaldosxKardex(mt.KardexRMModel).Result;
                var ultsaldo = saldos.OrderByDescending(x => x.Date).FirstOrDefault();

                double newCant = ultsaldo.Cantidad - mt.Amount;
                double newSaldo = ultsaldo.SaldoTotal - (newCant * ultsaldo.ValorUnitario);

                SaldosRMModel saldosRM = new SaldosRMModel()
                {
                    Date = mt.Date,
                    Cantidad = newCant,
                    SaldoTotal = newSaldo,
                    ValorUnitario = ultsaldo.ValorUnitario,
                    IdReconcimiento = mt.Id,
                    NombreReconocimiento = "Consumo"
                };

                App.Database.SaveSaldoRM(saldosRM);

                mt.KardexRMModel.SaldosRMs.Add(saldosRM);

                App.Database.UpdateRelationsKardexRM(mt.KardexRMModel);

                Shell.Current.DisplayAlert("Orden Aprobada", "La orden de produccion ha sido aprobada", "ok");

            }

            orderResp.EstadoOrder = "Aprobada";
            App.Database.SaveOrderProduccion(OrderResp);

            Shell.Current.GoToAsync($"//Rini/OrdenProduccion?IdOrder=");
            
        }
       
        public DetailOrdenProduccionVM()
        {

        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string idOrder = HttpUtility.UrlDecode(query["IdOrder"]);

            ShowDetails(idOrder);
        }

        private void ShowDetails(string idOrder)
        {
            try
            {
                int cont = 0;
                //Obtengo orden
                OrderResp = App.Database.GetOrderProduccion(int.Parse(idOrder)).Result;
                var producto = orderResp.Product;
                
                //Obtengo Producto
                var productoResp = App.Database.Get1Product(producto.Id).Result;
                var listaRM = productoResp.ListRMModel;

                //Obtengo lista de receta de producto
                var listaRMResp = App.Database.GetListRM(listaRM.Id).Result;
                var ListMaterialProdcir = App.Database.GetItemsListRMxListRm(listaRM).Result;
                                
                //var ListaMateriales = App.Database.GetItemsListRMxListRm(ListMaterialProdcir);
                foreach (var obj in ListMaterialProdcir)
                {
                    //Obtengo Kardex de Material
                    var kr = App.Database.GetKardexRM(obj.KardexRMModel).Result;
                    //Obtengo Ultimo saldo
                    var respi = App.Database.GetSaldosxKardex(kr).Result;
                    var ord = respi.OrderByDescending(x => x.Date).FirstOrDefault();

                    //Obtengo Material
                    var mt = App.Database.GetOneRM(kr.RawMaterialModell.Id).Result;
                    kr.RawMaterialModell = mt;
                   
                    obj.KardexRMModel = kr;
                    MaterialProducir.Add(obj);

                    if(ord.Cantidad > obj.Amount)
                    {
                        cont = cont + 1;
                    }
                }
                if(cont == ListMaterialProdcir.Count)
                {
                    MensajeConfirmacion = "Hay material disponible, se puede fabricar";
                    HabilitarButtonAcept = true;
                }
                TotalElementos = ListMaterialProdcir.Count;
                TotalValores = ListMaterialProdcir.Sum(x => x.TotalCost);
            }
            catch
            {
                Console.WriteLine("Fallo en llenado de datos DetailOrdenProduccionVM, en Show Details");
            }
            
        }
    }
}

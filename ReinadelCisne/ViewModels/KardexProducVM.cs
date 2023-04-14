using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using Xamarin.Forms;
using System.Linq;
using ReinadelCisne.Auxiliars;

namespace ReinadelCisne.ViewModels
{
    public class KardexProductVM : BaseVM, IQueryAttributable
    {
        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PassString> Kard { get; private set; } = new ObservableCollection<PassString>();
        public ObservableCollection<AuxiliarLayoutKardexRM> KardexLayout { get; set; } = new ObservableCollection<AuxiliarLayoutKardexRM>();
        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                OnPropertyChanged();
            }
        }

        public KardexProductVM()
        {
            //ObtenerKardex(product);
        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string ObjPass = HttpUtility.UrlDecode(query["objId"]);
                ObtenerObjeto(ObjPass);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        public ProductModel product = new ProductModel();
        private void ObtenerObjeto(string objPass)
        {
            try
            {
                ProductModel objresp = App.Database.Get1Product(int.Parse(objPass)).Result;
                Nombre = objresp.NameProduct;
                Descripcion = objresp.DescriptionProduct;
                //product = objresp
                ObtenerKardex(objresp);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load animal.");
            }
            
        }

        private void ObtenerKardex(ProductModel objresp)
        {
            
            var kardxProduct = objresp.Kardices;
            LLenarList(kardxProduct);
        }

        private void LLenarList(KardexModel resp)
        {
            //var resp1 = App.Database.GetAllKardxProduct().Result;
            var krdx = App.Database.Get1KardexProduct(resp.Id).Result;
            var ventasOrder = App.Database.ListOrders().Result;
            List<SaldosKardexProductModel> sald = krdx.SaldosProducts;

            foreach(var ss in sald)
            {
                //Inicio llenado inicial de kardex
                AuxiliarLayoutKardexRM KardexAll = new AuxiliarLayoutKardexRM
                {
                    Date = ss.Date,
                    Detail = ss.NombreReconocimiento
                };

                //Llenado de salidas
                var vents = (from vt in ventasOrder
                             where ss.NombreReconocimiento == "Venta" && ss.IdReconcimiento == vt.Id
                             select vt).FirstOrDefault();

                if(vents != null)
                {
                    KardexAll.SalidaCantidad = vents.AmountProduct;
                    KardexAll.SalidaValorUnitario = vents.ValorUnitario;
                    KardexAll.SalidaValorTotal = vents.Valor;
                }
                //LLenado de saldos
                KardexAll.SaldoCantidad = ss.Cantidad;
                KardexAll.SaldoValorUnitario = ss.ValorUnitario;
                KardexAll.SaldoTotal = ss.Cantidad * ss.ValorUnitario;

                //Lleno Laoyout para mostrar
                KardexLayout.Add(KardexAll);
            }

            
        }
    }
}

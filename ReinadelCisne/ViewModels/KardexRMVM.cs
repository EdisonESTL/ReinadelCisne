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
    public class KardexRMVM: BaseVM, IQueryAttributable
    {
        private RawMaterialModel _rawMaterial;
        public RawMaterialModel RawMaterial
        {
            get => _rawMaterial;
            set
            {
                _rawMaterial = value;
                OnPropertyChanged();
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string IdRM = HttpUtility.UrlDecode(query["IdRM"]);

                CargarRM(IdRM);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private void CargarRM(string idRM)
        {
            RawMaterialModel rm = App.Database.GetOneRM(int.Parse(idRM)).Result;

            RawMaterial = rm;

            CargarKardexRM();
            
        }

        private void CargarKardexRM()
        {
            var krs = App.Database.GetKardexsRM().Result;

            KardexRMModel query = (from k in krs
                         where k.IdRawMaterial == RawMaterial.Id
                         select k).FirstOrDefault();

            LlenarKard(query);
        }
        public ObservableCollection<ShoppingListModel> ShoppingLists { get; set; } = new ObservableCollection<ShoppingListModel>();
        public ObservableCollection<PassString> FormatoK { get; set; } = new ObservableCollection<PassString>();
        public ObservableCollection<ShoppingListModel> shoppingLists { get; set; } = new ObservableCollection<ShoppingListModel>();
        public ObservableCollection<SaldosRMModel> saldosRMs { get; set; } = new ObservableCollection<SaldosRMModel>();
        public ObservableCollection<AuxiliarLayoutKardexRM> KardexLayout { get; set; } = new ObservableCollection<AuxiliarLayoutKardexRM>();

        private void LlenarKard(KardexRMModel query)
        {
            LLenarSaldos(query);

            List<ShoppingListModel> Compras = query.ShoppingModell;
            List<SaldosRMModel> Saldos = query.SaldosRMs;
            List<ItemsListRMModel> Producciones = query.itemsListRMs;

            
            foreach(var s in Saldos)
            {
                //Inicio llenado inicial de kardex
                AuxiliarLayoutKardexRM KardexAll = new AuxiliarLayoutKardexRM
                {
                    Date = s.Date,
                    Detail = s.NombreReconocimiento
                };

                //LLeno compras
                var compra = (from c in Compras
                             where s.NombreReconocimiento == "Compra" && s.IdReconcimiento == c.Id
                             select c).FirstOrDefault();

               if(compra != null)
                {
                    KardexAll.EntradaCantidad = compra.Amount;
                    KardexAll.EntradaValorUnitario = compra.ValorUnitario;
                    KardexAll.EntradaValorTotal = KardexAll.EntradaCantidad * KardexAll.EntradaValorUnitario;
                }

                //Lleno de salidas
                var salida = (from pr in Producciones
                              where s.NombreReconocimiento == "Consumo" && s.IdReconcimiento == pr.Id
                              select pr).FirstOrDefault();

                if(salida != null)
                {
                    KardexAll.SalidaCantidad = salida.Amount;
                    KardexAll.SalidaValorUnitario = salida.UnitCost;
                    KardexAll.SalidaValorTotal = salida.TotalCost;
                }
                //LLenado de saldos

                KardexAll.SaldoCantidad = s.Cantidad;
                KardexAll.SaldoValorUnitario = s.ValorUnitario;
                KardexAll.SaldoTotal = s.Cantidad * s.ValorUnitario;

                //Lleno Laoyout para mostrar
                KardexLayout.Add(KardexAll);
            }
        }

        private void LLenarSaldos(KardexRMModel query)
        {
            foreach (var q in query.SaldosRMs)
            {
                saldosRMs.Add(q);
            }
        }
    }
}

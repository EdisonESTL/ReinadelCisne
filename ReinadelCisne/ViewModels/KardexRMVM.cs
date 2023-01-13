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

                //LLenado de saldos

                KardexAll.SaldoCantidad = s.Cantidad;
                KardexAll.SaldoValorUnitario = s.ValorUnitario;
                KardexAll.SaldoTotal = s.SaldoTotal;

                //Lleno Laoyout para mostrar
                KardexLayout.Add(KardexAll);
            }
            //var ds = query.ShoppingModell;

            /*PassString itms = new PassString()
            {
                Data0 = query.Date.ToString("dddd d MMM yyyy"),
                Data1 = "inventario inicial",
                Data2 = query.Cantidad.ToString(),
                Data3 = query.ValorUnitario.ToString("C"),
                Data4 = (query.Cantidad * query.ValorUnitario).ToString("C")
            };

            FormatoK.Add(itms);
            foreach(var a in ds)
            {
                PassString it = new PassString()
                {
                    Data0 = a.Date.ToString("dddd d MMM yyyy"),
                    Data1 = "compra",
                    Data2 = a.Amount.ToString(),
                    Data3 = a.ValorUnitario.ToString("C"),
                    Data4 = (a.Amount * a.ValorUnitario).ToString("C")
                };

                FormatoK.Add(it);
            }*/
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

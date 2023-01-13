using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace ReinadelCisne.ViewModels
{
    public class SaleDetailVM : BaseVM, IQueryAttributable
    {
        private SaleModel _sale;
        public SaleModel Sale
        {
            get => _sale;
            set
            {
                _sale = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderModel> Orders { get; set; } = new ObservableCollection<OrderModel>();
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string IdPP = HttpUtility.UrlDecode(query["IdPP"]);

                CargarPP(IdPP);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private void CargarPP(string idPP)
        {
            var resp = App.Database.GetSale(int.Parse(idPP)).Result;

            Sale = resp;

            CargarObjPP();
        }

        private void CargarObjPP()
        {
            var resp = App.Database.ListOrders().Result;

            var filt = (from a in resp
                        where a.SaleModeld == Sale.Id
                        select a).ToList();
            foreach(var e in filt)
            {
                Orders.Add(e);
            }
        }

        public ICommand PushCommand => new Command((obj) =>
        {
            var resp = obj as string;
            Direccionar(resp);
        });

        private void Direccionar(string resp)
        {
            switch (resp)
            {
                case "regresar":
                    Shell.Current.GoToAsync("//Rini/RProductosEnProceso");
                    break;
                case "home":
                    Shell.Current.GoToAsync("//Rini");
                    break;
                case "entregar":
                    EntregarPP();
                    break;
                default:
                    break;
            }
        }

        private void EntregarPP()
        {
            Shell.Current.DisplayAlert("Entregar", "El pedido ha sido entregado", "ok");

            Sale.DateSale = DateTime.Now;
            Sale.SaleStatus = "Entregado";
            
            App.Database.SaveSale(Sale);
            Direccionar("home");
        }
    }
}

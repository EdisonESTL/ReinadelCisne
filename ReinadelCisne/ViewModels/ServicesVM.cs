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
    public class ServicesVM : BaseVM
    {
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private SaleModel _seleccionPP;
        public SaleModel SeleccionPP
        {
            get => _seleccionPP;
            set
            {
                if(_seleccionPP != value)
                {
                    _seleccionPP = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SaleModel> ListProdProce { get; private set; } = new ObservableCollection<SaleModel>();
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsRefreshing = true;

                    ListProductInProcess();
                    //NumProducts();
                    //CostProduct();

                    IsRefreshing = false;
                });
            }
        }
        public ICommand Push => new Command((obj) =>
        {
            var recibo = obj as string;
            Direccionar(recibo);
        });
        public ICommand SelectListPP => new Command((obj) =>
        {
            SaleModel selc = obj as SaleModel;
            ListProductInProcess();
            Shell.Current.GoToAsync($"//Rini/RProductosEnProceso/DetailPP?IdPP={selc.Id}");
            
        });
        public ICommand EntregaCommand => new Command((obj) =>
        {
            SaleModel mod = obj as SaleModel;
            mod.SaleStatus = "Entregado";
            mod.DateSale = DateTime.Now;
            App.Database.SaveSale(mod);
            
            Shell.Current.DisplayAlert("Entrega", "El pedido ha sido entregado", "ok");
            ListProductInProcess();
        });
        public ServicesVM()
        {
            ListProductInProcess();
        }
        private void Direccionar(string recibo)
        {
            switch (recibo)
            {
                case "inicio":
                    Shell.Current.GoToAsync("//Rini"); break;
                case "nuevo":
                    Shell.Current.GoToAsync("//Rini/RServicios/NewService"); break;
                default:
                    break;
            }
        }
        private async void ListProductInProcess()
        {
            ListProdProce.Clear();
            List<string> products = new List<string>();
            var lps = await App.Database.ListSales();
            var respp = (from n in lps
                         where n.SaleStatus == "En proceso"
                         select n).ToList();
            if (respp.Count > 0)
            {
                foreach (var tp in respp.OrderByDescending(x => x.Id))
                {
                    ListProdProce.Add(tp);
                }
            }

        }
    }
}

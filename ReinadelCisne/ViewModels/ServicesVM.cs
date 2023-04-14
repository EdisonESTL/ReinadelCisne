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

        private OrderProduccionModel _seleccionPP;
        public OrderProduccionModel SeleccionPP
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
        public ObservableCollection<OrderProduccionModel> OrderProduccions { get; set; } = new ObservableCollection<OrderProduccionModel>();
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
        public ICommand SelectListPP => new Command(async (obj) =>
        {
            SeleccionPP = obj as OrderProduccionModel;
            ListProductInProcess();
            string action = await Shell.Current.DisplayActionSheet("Cambiar de estado?", "Cancel", null, "Terminado");
            CambiarEstado(action);
        });

        private void CambiarEstado(string action)
        {
            SeleccionPP.EstadoOrder = action;
            App.Database.SaveOrderProduccion(SeleccionPP);

            SeleccionPP.Product.EstadoProducto = action;
            App.Database.SaveProduct(SeleccionPP.Product);

            ListProductInProcess();
        }

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
            OrderProduccions.Clear();

            List<OrderProduccionModel> orders = await App.Database.GetAllOrdersProduccion();
            if (orders != null)
            {

                foreach (var ord in orders.Where(x => x.EstadoOrder == "Aprobada"))
                {
                    OrderProduccions.Add(ord);
                }
            }

        }
    }
}

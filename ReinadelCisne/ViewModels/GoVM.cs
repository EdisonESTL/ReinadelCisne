using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{    
    public class GoVM : BaseVM
    {
        public object _sto;
        public object SelectedItem 
        {
            get { return _sto; }
            set
            {
                _sto = value;
                OnPropertyChanged();
            }
        }

        private double _cuenta = 0.00;
        public double Cuenta
        {
            get { return _cuenta; }
            set
            {
                _cuenta = value;
                OnPropertyChanged();
            }
        }
       
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ProductModel> ListPS { get; private set; } = new ObservableCollection<ProductModel>();
        
        private List<OrderModel> Order { get; set; } = new List<OrderModel>();
        //Actualiza lista de prodcutos a vender
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            ListProductStock();

            IsRefreshing = false;
        });

        //Eleccion de opcion en menu
        public ICommand SelectedCommand => new Command((obj) =>
        {
            ProductModel prods = obj as ProductModel;
            
            
            //ListOrder.Add(prods);
            SumarOrden(prods);
        });

        //Limpiar
        public ICommand ClearCommand => new Command(() =>
        {
            ClearOrder();            
        });

        //Guarda la venta
        public ICommand NewCommand => new Command(() =>
        {
            if (Order != null)
            {                
                SaleModel d = new SaleModel();
                d.DateSale = DateTime.Now;
                d.TotalSale = Cuenta;
                
                App.Database.SaveSale(d);
                d.Orders = new List<OrderModel>();

                foreach (var obj in Order)
                {
                    App.Database.SaveOrder(obj);
                    d.Orders.Add(obj);
                    App.Database.UpdateRealtionSales(d);
                }                
                                
                ClearOrder();
                d = null;
                ListProductStock();
            }            
        });

        //Direcciona a la lista de ventas
        public ICommand RegisterSale => new Command(() => 
        {
            Shell.Current.GoToAsync("GoRegistration");
        });
        
        //Constructor
        public GoVM()
        {
            ListProductStock();
        }
        
        //Lista de productos para vender
        private async void ListProductStock()
        {
            ListPS.Clear();

            List<ProductModel> lps = await App.Database.ListProduct();
            if (lps != null)
            {
                foreach (ProductModel tp in lps)
                {
                    ListPS.Add(tp);
                }
            }

        }

        //Calcula el total de la venta
        private async void SumarOrden(ProductModel selectedPS)
        {
            var res = await Shell.Current.DisplayPromptAsync(selectedPS.NameProduct, "Cuantos desea?", initialValue: "1", maxLength: 2, keyboard: Keyboard.Numeric);
            
            if (!string.IsNullOrEmpty(res))
            {
                OrderModel pedido = new OrderModel
                {
                    ProductModelId = selectedPS.Id,
                    AmountProduct = int.Parse(res)
                };

                Cuenta += selectedPS.PriceProduct * int.Parse(res);

                Order.Add(pedido);
            }
        }

        //Limpia la orden
        private void ClearOrder()
        {
            Cuenta = 0.00;
            Order.Clear();
        }
    }
}

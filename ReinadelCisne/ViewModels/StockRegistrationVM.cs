using ReinadelCisne.Auxiliars;
using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class StockRegistrationVM : BaseVM
    {
        private string _productos;
        public string Productos
        {
            get => _productos;
            set
            {
                _productos = value;
                OnPropertyChanged();
            }
        }
        private string _costo;
        public string Costo
        {
            get => _costo;
            set
            {
                _costo = value;
                OnPropertyChanged();
            }
        }
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
        public ObservableCollection<PassString> ListPS { get; private set; } = new ObservableCollection<PassString>();

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsRefreshing = true;

                    ListProductStock();
                    NumProducts();
                    CostProduct();

                    IsRefreshing = false;
                });
            }
        }
        public ICommand NewStockCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("NewStock");
        });
        


        public Command<ProductModel> Delete { get; }
        public Command<ProductModel> Modify { get; }
        
        //Constructor
        public StockRegistrationVM()
        {
            ListProductStock();
            NumProducts();
            CostProduct();
            Delete = new Command<ProductModel>(DeletePS);
            Modify = new Command<ProductModel>(ModifyPS);
        }

        private async void CostProduct()
        {
            List<ProductModel> lps = await App.Database.ListProduct();
            
            if (lps.Count >= 0)
            {
                var result = lps.Sum(x => x.PriceProduct);
                var costsuma = "$" + result.ToString();

                Costo = costsuma;
            }
            else
            {
                Costo = "No hay productos registrados";
            }
        }
        private async void NumProducts()
        {
            var resp = await App.Database.GetTotalProducts();
            Productos = resp.ToString();
        }

        private async void ListProductStock()
        {
            ListPS.Clear();
            List<string> products = new List<string>();
            List<ProductModel> lps = await App.Database.ListProduct();
            if (lps != null)
            {
                foreach (var tp in lps)
                {
                    PassString pass = new PassString{
                        Data0 = tp.NameProduct,
                        Data1 = "$" + tp.PriceProduct.ToString(),
                        Data2 = tp.UnitProduct.ToString()
                    };
                    ListPS.Add(pass);
                }
            }

        }
        private async void DeletePS(ProductModel obj)
        {
            await App.Database.DeleteProduct(obj);
            await Shell.Current.DisplayAlert("Hola", obj.NameProduct + " con precio: " + obj.PriceProduct + " Ha sido eliminado", "Ok");
            ListProductStock();
        }

        private async void ModifyPS(ProductModel obj)
        {
            await Shell.Current.GoToAsync($"NewStock?IdListOC=0&IdlistRM=0&IdlistWF=0&idProduct={obj.Id}");
        }
    }
}

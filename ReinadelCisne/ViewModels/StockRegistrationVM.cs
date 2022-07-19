using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class StockRegistrationVM : BaseVM
    {
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

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsRefreshing = true;

                    ListProductStock();

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
        public StockRegistrationVM()
        {
            ListProductStock();
            Delete = new Command<ProductModel>(DeletePS);
            Modify = new Command<ProductModel>(ModifyPS);
        }

        private async void ListProductStock()
        {
            ListPS.Clear();

            List<ProductModel> lps = await App.Database.ListProduct();
            if (lps != null)
            {
                foreach (var tp in lps)
                {
                    ListPS.Add(tp);
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

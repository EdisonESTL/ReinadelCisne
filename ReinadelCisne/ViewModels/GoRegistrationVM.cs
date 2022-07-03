using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReinadelCisne.Models;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class GoRegistrationVM : BaseVM
    {
        public ObservableCollection<object> Collection { get; set; } = new ObservableCollection<object>();
        
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
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            ChargeSales();

            IsRefreshing = false;
        });

        public Command<SaleModel> DeleteCommand { get; }

        public GoRegistrationVM()
        {
            ChargeSales();
            DeleteCommand = new Command<SaleModel>(DeleteSale);
        }

        private async void ChargeSales()
        {
            Collection.Clear();
            List<SaleModel> f = await App.Database.ListSales();
            foreach (SaleModel g in f)
            {
                Collection.Add(g);
            }
        }

        private async void DeleteSale(SaleModel sale)
        {
            await App.Database.DeleteSale(sale);
            ChargeSales();
        }
    }
}

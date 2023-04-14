using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using ReinadelCisne.Models;
using ReinadelCisne.Auxiliars;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;
namespace ReinadelCisne.ViewModels
{
    public class OrdenProduccionVM : BaseVM
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
        public ObservableCollection<OrderProduccionModel> OrderProduccions { get; set; } = new ObservableCollection<OrderProduccionModel>();
        public OrdenProduccionVM()
        {
            GetOrdenProduccions();
        }
        public ICommand SelectdCommand => new Command((obj) =>
        {
            var rm = obj as OrderProduccionModel;
            GetOrdenProduccions();
            Shell.Current.GoToAsync($"DetailOrdenProduccion?IdOrder={rm.Id}");
        });
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            GetOrdenProduccions();

            IsRefreshing = false;
        });
        private async void GetOrdenProduccions()
        {
            OrderProduccions.Clear();

            List<OrderProduccionModel> orders = await App.Database.GetAllOrdersProduccion();
            if (orders != null )
            {

                foreach (var ord in orders.Where(x => x.EstadoOrder == "Pendiente"))
                {
                    OrderProduccions.Add(ord);
                }
            }
        }
    }
}

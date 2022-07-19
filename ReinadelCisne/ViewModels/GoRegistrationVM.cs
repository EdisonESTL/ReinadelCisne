using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReinadelCisne.Models;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class GoRegistrationVM : BaseVM
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startdate = DateTime.Today;
        public DateTime StartDate
        {
            get
            {
                return _startdate;
            }
            set
            {
                _startdate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _finishdate = DateTime.Today;
        public DateTime FinishDate
        {
            get
            {
                return _finishdate;
            }
            set
            {
                _finishdate = value;
                OnPropertyChanged();
            }
        }
        
        private AuzListShop _shop;
        public AuzListShop Shop
        {
            get { return _shop; }
            set
            {
                _shop = value;
                OnPropertyChanged();
            }
        }
        private string _salesTotal;
        public string SalesTotal
        {
            get { return _salesTotal; }
            set
            {
                _salesTotal = value;
                OnPropertyChanged();
            }
        }
        float n = 0;
        public ObservableCollection<AuzListShop> Ventas { get; set; } = new ObservableCollection<AuzListShop>();
        public GoRegistrationVM()
        {
            SalesDay();
        }     

        public ICommand SelectedCommand => new Command(() =>
        {
            if (Shop != null)
            {
                SelectShop();
                Shop = null;
            }
        });

        private async void SelectShop()
        {
            await Shell.Current.GoToAsync($"GoRegistrationDetail?SaleId={Shop.IdShop}");
        }
        public ICommand DayCommand => new Command(() =>
        {
            SalesDay();            
        });

        private void SalesDay()
        {
            n = 0;
            Ventas.Clear();
            var sales = App.Database.ListSales().Result;
            var fgt = (from s in sales
                       where s.DateSale.Date == StartDate.Date & s.WayToPay != "por cobrar"
                       select s).ToList();
            mostrarL(fgt);
        }

        public ICommand MounthCommand => new Command(() =>
        {
            n = 0;
            Ventas.Clear();
            var sales = App.Database.ListSales().Result;
            var fgt = (from s in sales
                       where s.DateSale.Date.Month == StartDate.Date.Month & s.WayToPay != "por cobrar"
                       select s).ToList();
            mostrarL(fgt);
        });

        public ICommand WeekCommand => new Command(() =>
        {
            n = 0;
            Ventas.Clear();
            var sales = App.Database.ListSales().Result;
            WeekDay(StartDate, out DateTime di, out DateTime df);

            var fgt = (from s in sales
                       where s.DateSale.Date >= di.Date & StartDate.Date <= df.Date & s.WayToPay != "por cobrar"
                       select s).ToList();
            mostrarL(fgt);
        });

        public ICommand XcobrarCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("GoRegistrationXcobrar");
        });
        private void mostrarL(List<SaleModel> list)
        {
            n = 0;
            foreach (var obj in list)
            {
                AuzListShop auzList = new AuzListShop
                {
                    IdShop = obj.Id,
                    fecha = obj.DateSale.Date.ToString("M"),
                    total = obj.TotalSale.ToString("N2") + "$"
                };

                Ventas.Add(auzList);
                n += (float)obj.TotalSale;
            }
            SalesTotal = n.ToString("N2") + "$";
        }
    }
}

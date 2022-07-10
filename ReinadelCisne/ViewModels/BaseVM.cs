using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ReinadelCisne.Models;

namespace ReinadelCisne.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        //public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void WeekDay(DateTime initDate, out DateTime datei, out DateTime datef)
        {
            DateTime dateig = default;
            DateTime datefg = default;
            string dayDate = initDate.Date.DayOfWeek.ToString();
            string[] WeekDay = new string[] { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            int nday = Array.IndexOf(WeekDay, dayDate);

            for (int i = WeekDay.GetLowerBound(0); i <= WeekDay.GetUpperBound(0); i++)
            {
                var d = WeekDay.GetUpperBound(0) - i;

                if (WeekDay[i] == dayDate)
                {
                    dateig = initDate.AddDays(-i);
                    datefg = initDate.AddDays(d);
                }
            }
            datei = dateig;
            datef = datefg;
        }

        public List<RawMaterialModel> Raws { get; set; }

        public void cargarL(List<RawMaterialModel> rawMaterials)
        {
            Raws = rawMaterials;
        }
       /* private bool _isRefreshing = false;
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

            ListProductStock();

            IsRefreshing = false;
        });*/
    }
}

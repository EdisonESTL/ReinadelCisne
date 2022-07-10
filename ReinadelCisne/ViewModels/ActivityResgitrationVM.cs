using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace ReinadelCisne.ViewModels
{
    class ActivityResgitrationVM : BaseVM
    {
        private DateTime _initDate = DateTime.Today;
        public DateTime InitDate
        {
            get { return _initDate; }
            set
            {
                _initDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ActivityModel> Activities { get; set; } = new ObservableCollection<ActivityModel>();

        public ICommand DayActivity => new Command(() =>
        {
            ActivityDay();
        });
        public ICommand WeekActivity => new Command(() =>
        {
            ActivityWeek();
        });
        public ICommand MonthActivity => new Command(() =>
        {
            ActivityMonth();
        });
        public ActivityResgitrationVM()
        {
            ActivityDay();
        }

        private void ActivityDay()
        {
            Activities.Clear();
            List<ActivityModel> listact = App.Database.ListActivity().Result;
            var f = (from a in listact
                    where a.DateA.Date == InitDate.Date
                    select a).ToList();

            mostrarA(f);
        }
        private void ActivityWeek()
        {
            Activities.Clear();
            List<ActivityModel> listact = App.Database.ListActivity().Result;
            WeekDay(InitDate, out DateTime di, out DateTime df);
            var f = (from a in listact
                     where a.DateA.Date >= di.Date & a.DateA.Date <= df.Date
                     select a).ToList();

            mostrarA(f);
        }
        private void ActivityMonth()
        {
            Activities.Clear();
            List<ActivityModel> listact = App.Database.ListActivity().Result;
            var f = (from a in listact
                     where a.DateA.Month == InitDate.Month
                     select a).ToList();

            mostrarA(f);
        }
        private void mostrarA(List<ActivityModel> lista)
        {
            foreach(var obj in lista)
            {
                Activities.Add(obj);
            }
        }
    }
}

using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class ActivitiesVM : BaseVM, IQueryAttributable
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _timeI = DateTime.Now.TimeOfDay;
        public TimeSpan TimeI
        {
            get => _timeI;
            set
            {
                _timeI = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _timeF = DateTime.Now.TimeOfDay;
        public TimeSpan TimeF
        {
            get => _timeF;
            set
            {
                _timeF = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public ICommand CancelCommand => new Command(() =>
        {
            ClearActivity();
        });
        public ICommand SaveCommand => new Command(() =>
        {
            SaveActivity();
        });
        public ICommand RegistrationCommand => new Command(() =>
        {
            RegistrationActivity();
        });

        private async void RegistrationActivity()
        {
            await Shell.Current.GoToAsync("ActivityRegistration");
        }

        private async void SaveActivity()
        {
            if (!string.IsNullOrEmpty(Description))
            {
                ActivityModel activity = new ActivityModel
                {
                    Id = Id,
                    DateA = Date,
                    TimeI = TimeI,
                    TimeF = TimeF,
                    Description = Description
                };

                await App.Database.SaveActivity(activity);
                await Shell.Current.DisplayAlert("SE GUARDO ACTIVIDAD", "Entre " + TimeI + " y "
                    + TimeF, "Ok");

                ClearActivity();
            }
        }
        private void ClearActivity()
        {
            Id = 0;
            Date = DateTime.Now;
            TimeI = DateTime.Now.TimeOfDay;
            TimeF = DateTime.Now.TimeOfDay;
            Description = string.Empty;
        }
        public ActivitiesVM()
        {

        }

        private string idAct;
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                idAct = HttpUtility.UrlDecode(query["ActModId"]);

                LoadAct(idAct);

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private async void LoadAct(string idAct)
        {
            var f = await App.Database.getActivity(int.Parse(idAct));
            Id = f.Id;
            Date = f.DateA;
            TimeI = f.TimeI;
            TimeF = f.TimeF;
            Description = f.Description;
        }
    }
}

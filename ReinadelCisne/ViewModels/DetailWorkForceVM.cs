using ReinadelCisne.Models;
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
    public class DetailWorkForceVM : BaseVM, IQueryAttributable
    {
        private WorkForceModel _workForce;

        public WorkForceModel WorkForce
        {
            get => _workForce;
            set
            {
                _workForce = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PersonalModel> Personals = new ObservableCollection<PersonalModel>();

        /*public ICommand RefreshCommand
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
        }*/
        /*public ICommand NewStockCommand => new Command(() =>
        {
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock?IdListWF=0&IdListCI=0&IdListRM=0&IdProduct=0");
        });*/
        public ICommand SelectedCommnad => new Command((obj) =>
        {
            PersonalModel pass = obj as PersonalModel;
            //ListProductStock();
            //Shell.Current.GoToAsync($"//Rini/Productos/Kardex?objId={pass.Data3}");
        });
        public ICommand GoBackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("..");
        });
        public DetailWorkForceVM()
        {
            WorkForce = new WorkForceModel();
        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string idWF = HttpUtility.UrlDecode(query["IdWF"]);
            LoadWF(idWF);
            
        }

        private void LoadWF(string idWF)
        {
            try
            {
                WorkForce = App.Database.GetWorkForce(int.Parse(idWF)).Result;

                var personals = WorkForce.Personals;

                foreach(var p in personals)
                {
                    Personals.Add(p);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load animal.");
            }
        }
    }
}

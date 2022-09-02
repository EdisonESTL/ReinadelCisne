using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Windows.Input;
using ReinadelCisne.Models;
using Xamarin.Forms;


namespace ReinadelCisne.ViewModels
{
    public class RawMaterialVm : BaseVM
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
        public ObservableCollection<RawMaterialShModel> RawMaterials { get; set; } = new ObservableCollection<RawMaterialShModel>();
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            ListRMs();

            IsRefreshing = false;
        });

        public RawMaterialVm()
        {
            ListRMs();
        }

        private async void ListRMs()
        {
            RawMaterials.Clear();
            List<RawMaterialModel> lrm = await App.Database.GetMR();

            foreach(var obj in lrm)
            {
                RawMaterials.Add(new RawMaterialShModel()
                {
                    Description = obj.NameRM,
                    Measurament = obj.UnitMeasurementRM,
                    Amount = obj.AmountRM,
                    UnitCost = obj.CostoRM.ToString("N2")
                });
            }
        }
    }
}

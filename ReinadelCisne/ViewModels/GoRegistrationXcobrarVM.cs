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
    public class GoRegistrationXcobrarVM : BaseVM
    {
        private AuxSalesXcobrar _aux;
        public AuxSalesXcobrar Aux
        {
            get => _aux;
            set
            {
                _aux = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AuxSalesXcobrar> SalesXc { get; set; } = new ObservableCollection<AuxSalesXcobrar>();
        public GoRegistrationXcobrarVM()
        {
            GetxCobrar();
        }
        public ICommand SelectedCommand => new Command(() =>
        {
            if (Aux != null)
            {
                CobrarSale();                
            }
        });

        private async void CobrarSale()
        {
            var resp = await Shell.Current.DisplayAlert("Pregunta", Aux.Name + " ha cancelado la venta?", "Si", "No");
            if (resp)
            {
                var mod = await App.Database.GetSale(Aux.Id);
                mod.WayToPay = "efectivo";
                await App.Database.SaveSale(mod);
                Aux = null;
                GetxCobrar();
            }
        }

        private void GetxCobrar()
        {
            SalesXc.Clear();

            var sales = App.Database.ListSales().Result;
            var fgt = (from s in sales
                       where s.WayToPay == "por cobrar"
                       select new AuxSalesXcobrar
                       { 
                           Id = s.Id,
                           Fecha = s.DateSale,
                           Name = s.ClientModel.Name,
                           Total = s.TotalSale
                       }).ToList();

            foreach(var f in fgt)
            {
                SalesXc.Add(f);
            }
        }
    }
}

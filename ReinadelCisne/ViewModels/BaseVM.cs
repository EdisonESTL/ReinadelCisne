using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Threading.Tasks;

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
        /*-----------------------------------------------------*/

        public void CalcularPromedioPonderado(double UltimoSaldo, int unids, double Precio, int UnidsUltimoSaldo, out double vpp)
        {
            vpp = (UltimoSaldo - (unids * Precio)) / UnidsUltimoSaldo - unids;
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
        public ICommand IniCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini");
        });
        public void cargarL(List<RawMaterialModel> rawMaterials)
        {
            Raws = rawMaterials;
        }

        public bool CampsNullsEmpty(List<string> vals)
        {
            int aux = 0;
            foreach(var s in vals)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    aux = 1;
                }
            }

            if(aux == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public BaseVM()
        {
            ObtenerUser();
        }

        /*private string _nameNegocio;
        public string NameNegocio
        {
            get => _nameNegocio;
            set
            {
                _nameNegocio = value;
                OnPropertyChanged();
            }
        }*/
        public UserModel UserModel { get; set; }
        private void ObtenerUser()
        {
            var f = App.Database.GetUser();
            UserModel = f.Result;
            //NameNegocio = UserModel.NegocioUser;
            Console.WriteLine("llegue");
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
        //Convertir Imagens a byte[ ]
        /*public async Task<byte[]> ImageSourceToByteArray(ImageSource imageSource)
        {
            byte[] bytesIamge = new byte[imageSource.]
        }*/
    }
}

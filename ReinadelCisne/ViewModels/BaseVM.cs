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

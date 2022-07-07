using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Models;

namespace ReinadelCisne.ViewModels
{
    public class ShoppingVM : BaseVM
    {
        private DateTime _date = DateTime.Now.Date;
        public DateTime Date
        {
            get { return _date; }
            set 
            { 
                _date = value;
                OnPropertyChanged();
            }
        }

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

        private string _nameEstablishment;
        public string NameEstablishment
        {
            get { return _nameEstablishment; }
            set
            {
                _nameEstablishment = value;
                OnPropertyChanged();
            }
        }

        private string _invoiceNumber;
        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged();
            }
        }

        private float _amount;
        public float Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        private string _measurement;
        public string Measurement
        {
            get { return _measurement; }
            set
            {
                _measurement = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _unitCost;
        public string UnitCost
        {
            get { return _unitCost; }
            set
            {
                _unitCost = value;
                OnPropertyChanged();
            }
        }

        private string _totalInv;
        public string TotalInv
        {
            get { return _totalInv; }
            set 
            { 
                _totalInv = value;
                OnPropertyChanged();
            }
        }
        private float _count = 0;
        public float Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private int _longList = 0;
        public int LongList
        {
            get { return _longList; }
            set
            {
                _longList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<RawMaterialShModel> ListCompra { get; set; } = new ObservableCollection<RawMaterialShModel>();
        
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListRawMl.get

            IsRefreshing = false;
        });

        public ICommand AddCompra => new Command(() =>
        {
            
            var dfg = (float)(Amount * Convert.ToDouble(UnitCost));

            RawMaterialShModel shModel = new RawMaterialShModel
            {
                Amount = Convert.ToDouble(Amount),
                Measurament = Measurement,
                Description = Description,
                UnitCost = UnitCost,
                TotalCost = dfg
            };

            ListCompra.Add(shModel);
            LongList = ListCompra.Count;
            Count += dfg;
            TotalInv = Count.ToString("N2") + "$";

            Amount = 0;
            Measurement = string.Empty;
            Description = string.Empty;
            UnitCost = string.Empty;
            dfg = 0;
        });
        public ShoppingVM()
        {

        }
    }
}

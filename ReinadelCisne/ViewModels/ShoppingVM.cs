using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Threading.Tasks;

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

        public ObservableCollection<RawMaterialModel> ListCompra { get; set; } = new ObservableCollection<RawMaterialModel>();
        
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListRawMl.get

            IsRefreshing = false;
        });
        public ICommand AddCompra => new Command(() =>
        {
            if(Amount != 0 & !string.IsNullOrEmpty(Description) & !string.IsNullOrEmpty(UnitCost))
            {
                var dfg = (float)(Amount * Convert.ToDouble(UnitCost));

                RawMaterialModel shModel = new RawMaterialModel
                {
                    AmountRM = Convert.ToDouble(Amount),
                    UnitMeasurementRM = Measurement,
                    NameRM = Description,
                    CostoRM = float.Parse(UnitCost),
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
            }            
        });
        public ICommand SaveCompra => new Command(async () =>
        {
            if (ListCompra.Count > 0)
            {
                ShoppingModel shopping = new ShoppingModel
                {
                    ShoppingDate = Date,
                    NameEstablishment = NameEstablishment,
                    InvoiceNumber = InvoiceNumber,
                    TotalShop = Count
                };

                await App.Database.SaveShopping(shopping);                

                foreach (var obj in ListCompra)
                {
                    ShoppingListModel shoppingList = new ShoppingListModel();

                    RawMaterialModel rawMaterial = new RawMaterialModel
                    {
                        
                        NameRM = obj.NameRM,
                        UnitMeasurementRM = obj.UnitMeasurementRM
                    };

                    await App.Database.SaveRawMaterial(rawMaterial);

                    shoppingList.Amount = obj.AmountRM;
                    shoppingList.UnitCost = Convert.ToDouble(obj.CostoRM);
                    shoppingList.TotalCost = Convert.ToDouble(obj.AmountRM * Convert.ToDouble(obj.CostoRM));

                    await App .Database.SaveListShop(shoppingList);

                    rawMaterial.shoppingList = new List<ShoppingListModel> { shoppingList };

                    await App .Database.UpdateRealtionRawMat(rawMaterial);

                    shopping.shoppingraw = new List<ShoppingListModel> { shoppingList };

                    await App .Database.UpdateRelationsSh(shopping);
                }                

                await Shell.Current.DisplayAlert("", "si se pudo", "ok");

                //await UpdateStock(shoppingList);
                //await UpdateWeightedAveragePrice(shoppingList);

                ClearShopping();
            }
        });

        private Task UpdateWeightedAveragePrice(ShoppingListModel shoppingList)
        {
            throw new NotImplementedException();
        }

        private Task UpdateStock(ShoppingListModel shoppingList)
        {
            throw new NotImplementedException();
        }

        public ICommand CancelCommand => new Command(() =>
        {
            ClearShopping();
        });

        public ICommand RegistrationComand => new Command(() =>
        {
            Shell.Current.GoToAsync("ShoppingRegister");
        });

        private void ClearShopping()
        {
            InvoiceNumber = string.Empty;
            NameEstablishment = string.Empty;
            Measurement = string.Empty;
            Description = string.Empty;
            UnitCost = string.Empty;
            TotalInv = string.Empty;
            Amount = 0;
            Count = 0;
            LongList = 0;
            ListCompra.Clear();
        }

        public ShoppingVM()
        {

        }
    }
}

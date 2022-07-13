using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Threading.Tasks;
using dotMorten.Xamarin.Forms;

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

        private string _id = "0";
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        
        private float _amount;
        public float Amount
        {
            get => _amount;
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
        public ObservableCollection<RawMaterialModel> NamesRM { get; set; } = new ObservableCollection<RawMaterialModel>();

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
                var totalC = (float)(Amount * Convert.ToDouble(UnitCost));

                RawMaterialModel shModel = new RawMaterialModel
                {
                    Id = int.Parse(Id),
                    AmountRM = Convert.ToDouble(Amount),
                    UnitMeasurementRM = Measurement,
                    NameRM = Description,
                    CostoRM = float.Parse(UnitCost),
                    TotalCost = totalC
                };

                if (IdMOD != null)
                {
                    ListCompra.RemoveAt(int.Parse(IdMOD));
                    ListCompra.Insert(int.Parse(IdMOD), shModel);
                    Count = 0;
                    foreach (var a in ListCompra)
                    {
                        Count += (float)(a.CostoRM * a.AmountRM);
                    }

                    IdMOD = null;
                }
                else
                {
                    ListCompra.Add(shModel);
                    Count += totalC;
                }

                LongList = ListCompra.Count;

                TotalInv = Count.ToString("N2") + "$";

                Measurement = string.Empty;
                Description = string.Empty;
                UnitCost = string.Empty;
                Amount = 0;
            }
        });
        public ICommand SaveCompra => new Command(() =>
        {
            if (ListCompra.Count > 0)
            {
                SaveShopping();
            }
        });
        public ICommand CancelCommand => new Command(() =>
        {
            ClearShopping();
        });
        public ICommand RegistrationComand => new Command(() =>
        {
            Shell.Current.GoToAsync("ShoppingRegister");
        });
        public Command<RawMaterialModel> DeleteCommand { get; set; }
        public Command<RawMaterialModel> ModifyCommand { get; set; }
        public ShoppingVM()
        {
            DeleteCommand = new Command<RawMaterialModel>(DeleteItemShop);
            ModifyCommand = new Command<RawMaterialModel>(ModifyItemShop);
            LoadNames();
        }

        private void LoadNames()
        {
            NamesRM.Clear();
            var a = App.Database.GetMR().Result;

            foreach(var b in a)
            {
                NamesRM.Add(b);
            }
        }

        public string IdMOD;
        private void ModifyItemShop(RawMaterialModel obj)
        {
            IdMOD = Convert.ToString(ListCompra.IndexOf(obj));

            Id = obj.Id.ToString();
            Amount = (float)obj.AmountRM;
            Measurement = obj.UnitMeasurementRM;
            Description = obj.NameRM;
            UnitCost = obj.CostoRM.ToString("N2");
        }
        private void DeleteItemShop(RawMaterialModel obj)
        {
            ListCompra.Remove(obj);
            Count = 0;
            foreach (var a in ListCompra)
            {
                Count += (float)(a.CostoRM * a.AmountRM);
            }
            TotalInv = Count.ToString("N2") + "$";
            LongList = ListCompra.Count;
        }
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
        private async void SaveShopping()
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
                RawMaterialModel rawMaterial = new RawMaterialModel
                {
                    Id = obj.Id,
                    NameRM = obj.NameRM,
                    UnitMeasurementRM = obj.UnitMeasurementRM
                };

                ShoppingListModel shoppingItem = new ShoppingListModel
                {
                    Amount = obj.AmountRM,
                    UnitCost = Convert.ToDouble(obj.CostoRM),
                    TotalCost = Convert.ToDouble(obj.AmountRM * Convert.ToDouble(obj.CostoRM))
                };

                await App.Database.SaveRawMaterial(rawMaterial);
                await App.Database.SaveListShop(shoppingItem);

                shoppingItem.RawMaterial = new RawMaterialModel();
                shoppingItem.RawMaterial = rawMaterial;
                shoppingItem.ShoppingModel = new ShoppingModel();
                shoppingItem.ShoppingModel = shopping;

                await App.Database.UpdateRelationsListShop(shoppingItem);
            }

            //await Shell.Current.DisplayAlert("Éxito", "se guardo la compra", "ok");
            LoadNames();
            UpdateStock();
            //UpdateWeightedAveragePrice();
            var op = App.Database.GetMR().Result;
            var ops = App.Database.ListShoppingList().Result;
            ClearShopping();
        }

        private void UpdateWeightedAveragePrice()
        {
            List<RawMaterialModel> rawsUp = new List<RawMaterialModel>();
            foreach (var rr in ListCompra)
            {
                rawsUp.Add(rr);
            }
        }
        private void UpdateStock()
        {
            List<RawMaterialModel> rawsUp = new List<RawMaterialModel>();
            foreach(var rr in ListCompra)
            {
                rawsUp.Add(rr);
            }
            App.Database.UpdateInvRM(rawsUp);
            var fgh = App.Database.GetMR().Result;
        }
    }
}

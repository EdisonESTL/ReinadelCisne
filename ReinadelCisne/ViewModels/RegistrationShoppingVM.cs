using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReinadelCisne.Models;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace ReinadelCisne.ViewModels
{
    public class RegistrationShoppingVM : BaseVM
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

        private DateTime _finishDate;
        public DateTime FinishDate
        {
            get { return _finishDate; }
            set
            {
                _finishDate = value;
                OnPropertyChanged();
            }
        }

        private string _shoppingsTotal;
        public string ShoppingsTotal
        {
            get { return _shoppingsTotal; }
            set
            {
                _shoppingsTotal = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AuzListShop> shoppings { get; set; } = new ObservableCollection<AuzListShop>();
        private float aux;

        private string _shoppingTotal;
        public string ShoppingTotal
        {
            get { return _shoppingTotal; }
            set
            {
                _shoppingTotal = value;
                OnPropertyChanged();
            }
        }
        private AuzListShop _shop;
        public AuzListShop Shop
        {
            get { return _shop; }
            set 
            { 
                _shop = value;
                OnPropertyChanged();
            }
        }

        public ICommand DayShopping => new Command(() =>
        {
            ShoppingDay();
        });
        public ICommand WeekShopping => new Command(() =>
        {
            ShoppingWeek();
        });
        public ICommand MonthShopping => new Command(() =>
        {
            ShoppingMonth();
        });
        public ICommand SelectedCommand => new Command(() =>
        {
            if(Shop != null)
            {
                SelectShop();
                Shop = null;
            }
        });

        private async void SelectShop()
        {
            await Shell.Current.GoToAsync($"ShoppingDetail?ShopId={Shop.IdShop}");
        }

        private void ShoppingList()
        {
            aux = 0;
            shoppings.Clear();

            var shoppingsList = App.Database.ListShopping().Result;

            mostrarL(shoppingsList);
        }

        private void ShoppingMonth()
        {
            aux = 0;
            shoppings.Clear();

            var shoppingsList = App.Database.ListShopping().Result;

            var ListandDate = (from obj in shoppingsList
                               where obj.ShoppingDate.Date.Month == InitDate.Month
                               select obj).ToList();

            mostrarL(ListandDate);
        }

        private void ShoppingWeek()
        {
            aux = 0;
            shoppings.Clear();

            var shoppingsList = App.Database.ListShopping().Result;

            WeekDay(InitDate, out DateTime di, out DateTime df);

            var ListandDate = (from obj in shoppingsList
                               where obj.ShoppingDate.Date >= di.Date & obj.ShoppingDate.Date <= df.Date
                               select obj).ToList();

            mostrarL(ListandDate);
        }

        private void ShoppingDay()
        {
            aux = 0;
            shoppings.Clear();

            var shoppingsList = App.Database.ListShopping().Result;
            /*var shoppingsList = App.Database.ListShopping().Result;
            var listsshoppings = App.Database.ListShoppingList().Result;
            var rawmaterialsList = App.Database.GetMR().Result;

            var ListandDate = (from lis in listsshoppings
                              join shop in shoppingsList  on lis.ShoppingModelId equals shop.Id
                              join raw in rawmaterialsList on lis.RawMaterialModelId equals raw.Id
                              where shop.ShoppingDate.Date == InitDate.Date
                              select new
                              {                                  
                                  date = shop.ShoppingDate,
                                  material = raw.NameRM,
                                  amount = lis.Amount,
                                  unitcost = lis.UnitCost,
                                  totalcost = lis.TotalCost
                              }).ToList();*/
            var ListandDate = (from obj in shoppingsList
                               where obj.ShoppingDate == InitDate
                               select obj).ToList();

            mostrarL(ListandDate);
        }
        private void mostrarL(List<ShoppingModel> list)
        {
            foreach (var obj in list)
            {
                AuzListShop auzList = new AuzListShop
                {
                    IdShop = obj.Id,
                    fecha = obj.ShoppingDate.Date.ToString("M"),
                    total = obj.TotalShop.ToString("N2") + "$"
                };

                shoppings.Add(auzList);
                aux += obj.TotalShop;
            }
            ShoppingTotal = aux.ToString("N2") + "$";
        }
        
                
        public RegistrationShoppingVM()
        {
            Shop = new AuzListShop();
            ShoppingList();
            Shop = null;
        }
    }
}

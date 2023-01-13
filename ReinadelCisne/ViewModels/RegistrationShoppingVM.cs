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

        private DateTime _finishDate = DateTime.Now;
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
        public ObservableCollection<ShoppingModel> Shoppings { get; set; } = new ObservableCollection<ShoppingModel>();
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

        private bool _onDate = false;
        public bool OnDate
        {
            get => _onDate;
            set
            {
                _onDate = value;
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

        public ICommand BackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/RMateriaPrima");
        });
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
        public ICommand SelectedCommand => new Command((obj) =>
        {
            var sel = obj as ShoppingModel;
            ShoppingList();
            SelectShop(sel);

            /*if (Shop != null)
            {
                Shop = null;
            }*/
        });
        public ICommand NewSCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/RCompras/SelectionPS");
            
        });

        public Command OnFechaCommand => new Command(() =>
        {
            OnDate = !OnDate;
        });
        public Command FechaCommand => new Command(() =>
        {
            Shoppings.Clear();
            ConsultarCompras();
        });
        private void SelectShop(ShoppingModel sel)
        {
            Shell.Current.GoToAsync($"//Rini/RMateriaPrima/RCompras/ShoppingDetail?ShopId={sel.Id}");
        }

        private void ShoppingList()
        {
            aux = 0;
            Shoppings.Clear();

            var shoppingsList = App.Database.ListShopping().Result;

            MostrarListShopping(shoppingsList);
        }

        private void ShoppingMonth()
        {
            aux = 0;
            shoppings.Clear();

            var shoppingsList = App.Database.ListShopping().Result;

            var ListandDate = (from obj in shoppingsList
                               where obj.ShoppingDate.Date.Month == InitDate.Month
                               select obj).ToList();

            MostrarListShopping(ListandDate);
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

            MostrarListShopping(ListandDate);
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

            MostrarListShopping(ListandDate);
        }
        private void MostrarListShopping(List<ShoppingModel> list)
        {
            shoppings.Clear();
            foreach (var obj in list)
            {
                Shoppings.Add(obj);
                aux += obj.TotalShop;
            }
            ShoppingTotal = aux.ToString("N2") + "$";
        }

        private void ConsultarCompras()
        {
            aux = 0;
            shoppings.Clear();

            var shoppingsList = App.Database.ListShopping().Result;
            var ListandDate = (from obj in shoppingsList
                               where obj.ShoppingDate.Date >= InitDate.Date && obj.ShoppingDate.Date <= FinishDate.Date
                               select obj).ToList();
            if(ListandDate.Count >= 0)
            {
                MostrarListShopping(ListandDate);
            }
            else
            {
                Shell.Current.DisplayAlert("Aviso", "no hay compras hechas", "aceptar");
                MostrarListShopping(shoppingsList);
            }

        }

        public RegistrationShoppingVM()
        {
            Shop = new AuzListShop();
            ShoppingList();
            Shop = null;
        }
    }
}

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
    public class ShopsVM : BaseVM
    {
        private DateTime _dateActual = DateTime.Now;
        public DateTime DateActual 
        {
            get => _dateActual;
            set
            {
                _dateActual = value;
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
        public ObservableCollection<object> shopsg { get; set; } = new ObservableCollection<object>();
        public List<DetailShopRMGroup> RMs { get; set; } = new List<DetailShopRMGroup>();
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListProductStock();

            IsRefreshing = false;
        });

        public ICommand DayCommand => new Command(() =>
        {
            LoadShopsDay();
        });
        public ShopsVM()
        {
            
            //LoadShops();
        }

        private void LoadShopsDay()
        {
            var sales = App.Database.ListShopping().Result;
            var shops = App.Database.ListShoppingList().Result;
            var maters = App.Database.GetMR().Result;

            var paso1 = from s in shops
                        join ss in sales on s.ShoppingModelId equals ss.Id
                        where ss.ShoppingDate.Date == DateActual.Date
                        select new
                        {
                            rmId = s.RawMaterialModelId,
                            datem = ss.ShoppingDate,
                            cantm = s.Amount,
                            costu = s.UnitCost,
                            total = s.TotalCost
                        };

            var paso2 = from p in paso1
                        join m in maters on p.rmId equals m.Id
                        select new
                        {
                            material = m.NameRM,
                            measurement = m.UnitMeasurementRM,
                            datem = p.datem,
                            cantm = p.cantm,
                            costu = p.costu,
                            total = p.total
                        };

            var paso3 = from t in paso2
                        group t by t.material;

            List<DetailShopRM> rmdet = new List<DetailShopRM>();
            foreach (var materialGroup in paso3)
            {
                foreach (var rm in materialGroup)
                {
                    DetailShopRM detail = new DetailShopRM
                    {
                        Measurement = rm.measurement,
                        DateShop = rm.datem.ToString("m"),
                        CantRM = rm.cantm.ToString(),
                        CostU = rm.costu.ToString("N2") + "$",
                        CostT = rm.total.ToString("N2") + "$"
                    };

                    rmdet.Add(detail);
                }

                RMs.Add(new DetailShopRMGroup(materialGroup.Key, rmdet));
            }
        }
        private async void LoadShops()
        {
            var sales = await App.Database.ListShopping();
            var shops = await App.Database.ListShoppingList();
            var maters = await App.Database.GetMR();

            var paso1 = from s in shops
                      join ss in sales on s.ShoppingModelId equals ss.Id
                        join m in maters on s.RawMaterialModelId equals m.Id
                        select new
                      {
                          material = m.NameRM,
                          measurement = m.UnitMeasurementRM,
                          datem = ss.ShoppingDate,
                          cantm = s.Amount,
                          costu = s.UnitCost,
                          total = s.TotalCost
                      };

            
            var fgn = from t in paso1
                      group t by t.material;

            /*List<IGrouping<int, ShoppingListModel>> shop = (from s in shops
                       group s by s.RawMaterialModelId into groupShopsRm
                       select groupShopsRm).ToList();*/
            List<DetailShopRM> rmdet = new List<DetailShopRM>();
            foreach (var materialGroup in fgn)
            {
                foreach (var rm in materialGroup)
                {
                    DetailShopRM detail = new DetailShopRM
                    {
                        Measurement = rm.measurement,
                        DateShop = rm.datem.ToString("m"),
                        CantRM = rm.cantm.ToString(),
                        CostU = rm.costu.ToString("N2") + "$",
                        CostT = rm.total.ToString("N2") + "$"
                    };

                    rmdet.Add(detail);
                }

                RMs.Add(new DetailShopRMGroup(materialGroup.Key, rmdet));
            }
        }
    }
}

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
    public class SalesVM : BaseVM
    {
        private int _id;
        public int Id 
        { 
            get 
            { 
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startdate = DateTime.Today;
        public DateTime StartDate
        {
            get
            {
                return _startdate;
            }
            set
            {
                _startdate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _finishdate = DateTime.Today;
        
        public DateTime FinishDate
        {
            get
            {
                return _finishdate;
            }
            set
            {
                _finishdate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<object> Ventas { get; set; } = new ObservableCollection<object>();
        public SalesVM()
        {
        }

        private string _salesTotal;
        public string SalesTotal
        {
            get { return _salesTotal; }
            set { _salesTotal = value;
                OnPropertyChanged(); }
        }

        private string _shopsTotal;
        public string ShopsTotal
        {
            get { return _shopsTotal; }
            set
            {
                _shopsTotal = value;
                OnPropertyChanged();
            }
        }
        private string _salesDiconutTotal;
        public string SalesDiconutTotal
        {
            get { return _salesDiconutTotal; }
            set
            {
                _salesDiconutTotal = value;
                OnPropertyChanged();
            }
        }
        private string _total;
        public string Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged();
            }
        }
        
        float n = 0;
        float shopscount = 0;
        double SalesDiscountCount = 0;
        
        public ICommand DayCommand => new Command(() =>
        {
            n = 0;
            shopscount = 0;
            SalesDiscountCount = 0;
            Ventas.Clear();

            var sales = App.Database.ListSales().Result;
            var orders = App.Database.ListOrders().Result;
            var products = App.Database.ListProduct().Result;

            var paso0 = (from s in sales
                        where s.WayToPay != "por cobrar" & s.Discount == 0
                        select s).ToList();

            var fgt = (from o in orders
                      join s in sales on o.SaleModeld equals s.Id
                      where s.DateSale.Date >= StartDate.Date && s.DateSale.Date <= FinishDate.Date & s.WayToPay != "por cobrar" & s.Discount == 0
                       select new
                      {
                          ProductId = o.ProductModelId,
                          Amount = o.AmountProduct
                      }).ToList();

            var j = from o in fgt
                    group o by o.ProductId
                    into vj
                    select new
                    {
                        ProductId = vj.Key,
                        Amount = vj.Sum(x => x.Amount)
                    };

            var jj = (from obj in j
                     from p in products
                     where obj.ProductId == p.Id
                     select new
                     {
                         Name = p.NameProduct,
                         canT = obj.Amount,
                         cstu = p.PriceProduct,
                         TT = obj.Amount * p.PriceProduct
                     }).ToList();

            foreach(var ji in jj)
            {
                Ventas.Add(ji);
                n = (float)(n + ji.TT);                
            }

            SalesTotal = n.ToString("N2") + "$";

            var SalesDiscou = (from s in sales
                               where s.DateSale.Date == StartDate.Date & s.Discount != 0
                               select s).ToList();
            foreach (var s in SalesDiscou)
            {
                SalesDiscountCount += s.TotalSale;
            }
            SalesDiconutTotal = SalesDiscountCount.ToString() + "$";

            var shoppings = App.Database.ListShopping().Result;

            var paso1 = (from ss in shoppings
                        where ss.ShoppingDate.Date == StartDate.Date
                        select ss).ToList();
            foreach(var p in paso1)
            {
                shopscount = shopscount + p.TotalShop;
            }
            ShopsTotal = shopscount.ToString("N2") + "$";
            
            Total = (n - SalesDiscountCount - shopscount).ToString("N2") + "$";
        });

        public ICommand WeekCommand => new Command(() =>
        {
            n = 0;
            shopscount = 0;
            SalesDiscountCount = 0;
            Ventas.Clear();

            WeekDay(StartDate, out DateTime datei, out DateTime datef);

            var sales = App.Database.ListSales().Result;
            var orders = App.Database.ListOrders().Result;
            var products = App.Database.ListProduct().Result;
            
            var paso0 = (from s in sales
                         where s.WayToPay != "por cobrar" & s.Discount == 0
                         select s).ToList();

            var fgt = (from o in orders
                       join s in sales on o.SaleModeld equals s.Id
                       where s.DateSale.Date >= datei.Date && s.DateSale.Date <= datef.Date & s.WayToPay != "por cobrar" & s.Discount == 0
                       select new
                       {
                           ProductId = o.ProductModelId,
                           Amount = o.AmountProduct
                       }).ToList();

            var j = from o in fgt
                    group o by o.ProductId
                    into vj
                    select new
                    {
                        ProductId = vj.Key,
                        Amount = vj.Sum(x => x.Amount)
                    };

            var jj = (from obj in j
                      from p in products
                      where obj.ProductId == p.Id
                      select new
                      {
                          Name = p.NameProduct,
                          canT = obj.Amount,
                          cstu = p.PriceProduct,
                          TT = obj.Amount * p.PriceProduct
                      }).ToList();

            foreach (var ji in jj)
            {
                Ventas.Add(ji);
                n = (float)(n + ji.TT);
            }
            SalesTotal = n.ToString("N2") + "$";


            var SalesDiscou = (from s in sales
                               where s.Discount != 0 & s.DateSale.Date >= datei.Date && s.DateSale.Date <= datef.Date
                               select s).ToList();
            foreach(var s in SalesDiscou)
            {
                SalesDiscountCount += s.TotalSale;
            }
            SalesDiconutTotal = SalesDiscountCount.ToString() + "$";

            var shoppings = App.Database.ListShopping().Result;

            var paso1 = (from ss in shoppings
                         where ss.ShoppingDate.Date >= datei.Date & ss.ShoppingDate.Date <= datef.Date
                         select ss).ToList();
            foreach (var p in paso1)
            {
                shopscount = shopscount + p.TotalShop;
            }
            ShopsTotal = shopscount.ToString("N2") + "$";
            
            Total = (n - SalesDiscountCount - shopscount).ToString("N2") + "$";
        });

        public ICommand MounthCommand => new Command(() =>
        {
            n = 0;
            shopscount = 0;
            SalesDiscountCount = 0;
            Ventas.Clear();

            var sales = App.Database.ListSales().Result;
            var orders = App.Database.ListOrders().Result;
            var products = App.Database.ListProduct().Result;

            var paso0 = (from s in sales
                         where s.WayToPay != "por cobrar" & s.Discount == 0
                         select s).ToList();

            var fgt = (from o in orders
                       join s in sales on o.SaleModeld equals s.Id
                       where s.DateSale.Month >= StartDate.Month && s.DateSale.Month <= FinishDate.Month & s.WayToPay != "por cobrar" & s.Discount == 0
                       select new
                       {
                           ProductId = o.ProductModelId,
                           Amount = o.AmountProduct
                       }).ToList();

            var j = from o in fgt
                    group o by o.ProductId
                    into vj
                    select new
                    {
                        ProductId = vj.Key,
                        Amount = vj.Sum(x => x.Amount)
                    };

            var jj = (from obj in j
                      from p in products
                      where obj.ProductId == p.Id
                      select new
                      {
                          Name = p.NameProduct,
                          canT = obj.Amount,
                          cstu = p.PriceProduct,
                          TT = obj.Amount * p.PriceProduct
                      }).ToList();

            foreach (var ji in jj)
            {
                Ventas.Add(ji);
                n = (float)(n + ji.TT);
            }
            SalesTotal = n.ToString("N2") + "$";

            var SalesDiscou = (from s in sales
                               where s.Discount != 0 & s.DateSale.Month == StartDate.Month
                               select s).ToList();
            foreach (var s in SalesDiscou)
            {
                SalesDiscountCount += s.TotalSale;
            }
            SalesDiconutTotal = SalesDiscountCount.ToString() + "$";

            var shoppings = App.Database.ListShopping().Result;

            var paso1 = (from ss in shoppings
                         where ss.ShoppingDate.Month == StartDate.Month
                         select ss).ToList();
            foreach (var p in paso1)
            {
                shopscount = shopscount + p.TotalShop;
            }
            ShopsTotal = shopscount.ToString("N2") + "$";

            Total = (n - SalesDiscountCount - shopscount).ToString("N2") + "$";
        });

        //Falta modificar descuentos y por cobrar
        public ICommand YearCommand => new Command(() =>
        {
            n = 0;
            shopscount = 0;
            Ventas.Clear();
            var sales = App.Database.ListSales().Result;
            var orders = App.Database.ListOrders().Result;
            var products = App.Database.ListProduct().Result;
            
            var paso0 = (from s in sales
                         where s.WayToPay != "por cobrar" & s.Discount == 0
                         select s).ToList();

            var fgt = (from o in orders
                       join s in paso0 on o.SaleModeld equals s.Id
                       where s.DateSale.Year >= StartDate.Year && s.DateSale.Year <= FinishDate.Year
                       select new
                       {
                           ProductId = o.ProductModelId,
                           Amount = o.AmountProduct
                       }).ToList();

            var j = from o in fgt
                    group o by o.ProductId
                    into vj
                    select new
                    {
                        ProductId = vj.Key,
                        Amount = vj.Sum(x => x.Amount)
                    };

            var jj = (from obj in j
                      from p in products
                      where obj.ProductId == p.Id
                      select new
                      {
                          Name = p.NameProduct,
                          canT = obj.Amount,
                          cstu = p.PriceProduct,
                          TT = obj.Amount * p.PriceProduct
                      }).ToList();

            foreach (var ji in jj)
            {
                Ventas.Add(ji);
                n = (float)(n + ji.TT);
            }
            SalesTotal = n.ToString("N2") + "$";

            var shoppings = App.Database.ListShopping().Result;

            var paso1 = (from ss in shoppings
                         where ss.ShoppingDate.Year == StartDate.Year
                         select ss).ToList();
            foreach (var p in paso1)
            {
                shopscount = shopscount + p.TotalShop;
            }
            ShopsTotal = shopscount.ToString("N2") + "$";
            Total = (n - shopscount).ToString() + "$";
        });
    }
}

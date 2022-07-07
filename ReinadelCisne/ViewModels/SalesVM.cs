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
        float n = 0;
        public ICommand DayCommand => new Command(() =>
        {
            n = 0;
            Ventas.Clear();
            var sales = App.Database.ListSales().Result;
            var orders = App.Database.ListOrders().Result;
            var products = App.Database.ListProduct().Result;                 

            var fgt = (from o in orders
                      join s in sales on o.SaleModeld equals s.Id
                      where s.DateSale.Date >= StartDate.Date && s.DateSale.Date <= FinishDate.Date
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
                         TT = obj.Amount * p.PriceProduct
                     }).ToList();

            foreach(var ji in jj)
            {
                Ventas.Add(ji);
                n = (float)(n + ji.TT);                
            }

            SalesTotal = n.ToString("N2") + "$";
        });

        public ICommand MounthCommand => new Command(() =>
        {
            n = 0;
            Ventas.Clear();
            var sales = App.Database.ListSales().Result;
            var orders = App.Database.ListOrders().Result;
            var products = App.Database.ListProduct().Result;

            var fgt = (from o in orders
                       join s in sales on o.SaleModeld equals s.Id
                       where s.DateSale.Month >= StartDate.Month && s.DateSale.Month <= FinishDate.Month
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
                          TT = obj.Amount * p.PriceProduct
                      }).ToList();

            foreach (var ji in jj)
            {
                Ventas.Add(ji);
                n = (float)(n + ji.TT);
            }
            SalesTotal = n.ToString("N2") + "$";
        });

        public ICommand YearCommand => new Command(() =>
        {
            n = 0;
            Ventas.Clear();
            var sales = App.Database.ListSales().Result;
            var orders = App.Database.ListOrders().Result;
            var products = App.Database.ListProduct().Result;

            var fgt = (from o in orders
                       join s in sales on o.SaleModeld equals s.Id
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
                          TT = obj.Amount * p.PriceProduct
                      }).ToList();

            foreach (var ji in jj)
            {
                Ventas.Add(ji);
                n = (float)(n + ji.TT);
            }
            SalesTotal = n.ToString("N2") + "$";
        });
    }
}

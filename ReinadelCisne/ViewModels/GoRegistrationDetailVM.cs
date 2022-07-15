using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReinadelCisne.Models;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Web;

namespace ReinadelCisne.ViewModels
{
    public class GoRegistrationDetailVM : BaseVM, IQueryAttributable
    {
        private string _date;
        public string Date { get => _date; set { _date = value; OnPropertyChanged(); } }

        private string _nameEstablishment;
        public string NameEstablishment
        {
            get => _nameEstablishment;
            set
            {
                _nameEstablishment = value;
                OnPropertyChanged();
            }
        }

        private string _invoiceNumber;
        public string InvoiceNumber
        {
            get => _invoiceNumber;
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged();
            }
        }

        private string _totalInv;
        public string TotalInv
        {
            get => _totalInv;
            set
            {
                _totalInv = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<object> ShopsRMs { get; set; } = new ObservableCollection<object>();

        public GoRegistrationDetailVM()
        {

        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string idShop = HttpUtility.UrlDecode(query["SaleId"]);
            ShowShopping(idShop);
        }
        private void ShowShopping(string idShop)
        {
            SaleModel sale = App.Database.GetSale(int.Parse(idShop)).Result;

            Date = sale.DateSale.ToString("D");
            TotalInv = sale.TotalSale.ToString("N2") + "$";

            var listsales = App.Database.ListOrders().Result;

            List<OrderModel> ft = (from lis in listsales
                                   where lis.SaleModeld == sale.Id
                                   select lis).ToList();

            var productlist = App.Database.ListProduct().Result;

            var listshp = (from shp in ft
                           join mat in productlist on shp.ProductModelId equals mat.Id
                           select new
                           {
                               Producto = mat.NameProduct,
                               Cantidad = shp.AmountProduct,
                               CostoU = mat.PriceProduct.ToString("N2") + "$",
                               CostoT = (shp.AmountProduct * mat.PriceProduct).ToString("N2") + "$"
                           }).ToList();
            foreach (var obj in listshp)
            {
                ShopsRMs.Add(obj);
            }
        }
    }
}

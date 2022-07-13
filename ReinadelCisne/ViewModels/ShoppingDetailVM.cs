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
    public class ShoppingDetailVM : BaseVM, IQueryAttributable
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
        
        public ShoppingDetailVM()
        {

        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string idShop = HttpUtility.UrlDecode(query["ShopId"]);
            ShowShopping(idShop);
        }
        private void ShowShopping(string idShop)
        {
            ShoppingModel shopping = App.Database.GetShopping(int.Parse(idShop)).Result;

            Date = shopping.ShoppingDate.ToString("D");
            NameEstablishment = shopping.NameEstablishment;
            InvoiceNumber = shopping.InvoiceNumber;
            TotalInv = shopping.TotalShop.ToString("N2") + "$";

            var listsshoppings = App.Database.ListShoppingList().Result;

            List<ShoppingListModel> ft = (from lis in listsshoppings
                     where lis.ShoppingModelId == shopping.Id
                     select lis).ToList();

            var rawmaterialsList = App.Database.GetMR().Result;

            var listshp = (from shp in ft
                           join mat in rawmaterialsList on shp.RawMaterialModelId equals mat.Id
                           select new
                           {
                               Material = mat.NameRM,
                               UnidMedida = mat.UnitMeasurementRM,
                               Cantidad = shp.Amount,
                               CostoU = shp.UnitCost.ToString("N2") + "$",
                               CostoT = shp.TotalCost.ToString("N2") + "$"
                           }).ToList();
            foreach(var obj in listshp)
            {
                ShopsRMs.Add(obj);
            }
        }
    }
}

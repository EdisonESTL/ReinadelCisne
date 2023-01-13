using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReinadelCisne.Models;
using ReinadelCisne.Auxiliars;
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
        public ObservableCollection<AuxiliarShoppindDetailxRM> ShopsRMs { get; set; } = new ObservableCollection<AuxiliarShoppindDetailxRM>();
        
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

            List<ShoppingListModel> listr = (from lis in listsshoppings
                     where lis.ShoppingModelId == shopping.Id
                     select lis).ToList();

            var kardexRMs = App.Database.GetKardexsRM().Result;

            var fli = (from lit in listr
                      join kardex in kardexRMs on lit.KardexRMModelId equals kardex.Id
                      select new AuxiliarShoppindDetailxRM()
                      {
                          Shopping = lit,
                          RawMaterial = kardex.RawMaterialModell
                      }).ToList();


            foreach(var obj in fli)
            {
                var resp = App.Database.GetOneRM(obj.RawMaterial.Id).Result;
                obj.RawMaterial = resp;
                ShopsRMs.Add(obj);
            }
        }
    }
}

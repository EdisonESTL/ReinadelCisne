using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Linq;
using System.Threading.Tasks;
using dotMorten.Xamarin.Forms;
using ReinadelCisne.Auxiliars;

namespace ReinadelCisne.ViewModels
{
    public class ShoppingProdServVM
    {
        public ObservableCollection<ProductModel> ListCompras { get; set; } = new ObservableCollection<ProductModel>();
        public ObservableCollection<RawMaterialModel> ListCompras2 { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ObservableCollection<PassString> ListPS { get; private set; } = new ObservableCollection<PassString>();
        public ICommand BackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/RCompras");
        });

        public ICommand PushCommand => new Command((obj) =>
        {
            string push = obj as string;
            Cargar(push);
        });

        public ICommand SelectedCommnad => new Command((obj) =>
        {
            PassString pass = obj as PassString;
            if (pass.Data4 == "producto")
            {
                ConsultarProductos();
            }
            Shell.Current.GoToAsync($"//Rini/RCompras/SelectionPS/NewShopping?objId={pass.Data3}&TipoElemento={pass.Data4}");
        });

        /*public static void Refresh<T>(this ObservableCollection<T> value)
        {
            CollectionViewSource.GetDefaultView(value).Refresh();
            
        }*/

        private void Cargar(string push)
        {
            switch (push)
            {
                case "productos":
                    ConsultarProductos();
                    break;
                case "materiaprima":
                    ConsultarMateriaPrima();
                    break;
                default:
                    break;
            }
        }

        private void ConsultarMateriaPrima()
        {
            ListPS.Clear();
            List<string> products = new List<string>();
            List<RawMaterialModel> lps = App.Database.GetMR().Result;
            if (lps != null)
            {
                foreach (var tp in lps.OrderByDescending(x => x.Id))
                {
                    PassString pass = new PassString
                    {
                        Data0 = tp.NameRM,
                        Data1 = "$" + tp.CostoRM.ToString(),
                        Data2 = tp.CantidadRM.ToString(),
                        Data3 = tp.Id.ToString(),
                        Data4 = "materiaprima"
                    };
                    ListPS.Add(pass);
                }
            }
        }

        private void ConsultarProductos()
        {
            ListPS.Clear();
            List<string> products = new List<string>();
            List<ProductModel> lps = App.Database.ListProduct().Result;
            if (lps != null)
            {
                foreach (var tp in lps.OrderByDescending(x => x.Id))
                {
                    PassString pass = new PassString
                    {
                        Data0 = tp.NameProduct,
                        Data1 = "$" + tp.PrecioVentaProduct.ToString(),
                        Data2 = tp.CantProduct.ToString(),
                        Data3 = tp.Id.ToString(),
                        Data4 = "producto"
                    };
                    ListPS.Add(pass);
                }
            }
        }
    }
}

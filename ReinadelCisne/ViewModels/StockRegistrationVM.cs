using ReinadelCisne.Auxiliars;
using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class StockRegistrationVM : BaseVM, IQueryAttributable
    {
        private string _productos;
        public string Productos
        {
            get => _productos;
            set
            {
                _productos = value;
                OnPropertyChanged();
            }
        }
        
        private string _costo;
        public string Costo
        {
            get => _costo;
            set
            {
                _costo = value;
                OnPropertyChanged();
            }
        }
        
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
       
        private GroupsProductModel _groupsSelected;
        public GroupsProductModel GroupsSelected
        {
            get => _groupsSelected;
            set
            {
                _groupsSelected = value;
                OnPropertyChanged();
            }
        }
        
        public ObservableCollection<PassString> ListPS { get; private set; } = new ObservableCollection<PassString>();
        public ObservableCollection<GroupsProductModel> GroupsProducts { get; private set; } = new ObservableCollection<GroupsProductModel>();

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsRefreshing = true;

                    ListProductStock();
                    NumProducts();
                    CostProduct();

                    IsRefreshing = false;
                });
            }
        }
        public ICommand NewStockCommand => new Command(() =>
        {
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock?IdListWF=0&IdListCI=0&IdListRM=0&IdProduct=0");
        });
        public ICommand SelectedCommnad => new Command((obj) =>
        {
            PassString pass = obj as PassString;
            ListProductStock();
            Shell.Current.GoToAsync($"//Rini/Productos/Kardex?objId={pass.Data3}");
        });
        public ICommand GoBackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini");
        });
        public ICommand SearchCommand => new Command(() =>
        {
            FiltrarGrupo();
        });
        public ICommand ProductsCommand => new Command(() =>
        {
            ListProductStock();
        });
        public ICommand VentasCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/Productos/Ventas");
        });
        public Command<PassString> Delete { get; }
        public Command<PassString> Modify { get; }
        
        //Constructor
        public StockRegistrationVM()
        {
            ListProductStock();
            NumProducts();
            CostProduct();
            GroupProds();
            Delete = new Command<PassString>(DeletePS);
            Modify = new Command<PassString>(ModifyPS);
        }

        private void FiltrarGrupo()
        {
            ListPS.Clear();
            var prods = App.Database.ListProduct().Result;

            if (GroupsSelected != null) 
            {
                var filtro = (from prd in prods
                              where prd.GroupProductId == GroupsSelected.Id
                              select prd).ToList();
                if (filtro.Count != 0)
                {
                    foreach (var tp in filtro.OrderByDescending(x => x.Id))
                    {
                        PassString pass = new PassString
                        {
                            Data0 = tp.NameProduct,
                            Data1 = "$" + tp.PrecioVentaProduct.ToString(),
                            //Data2 = tp.CantProduct.ToString(),
                            Data3 = tp.Id.ToString()
                        };
                        ListPS.Add(pass);
                    }
                }
            }

            
        }
        private void GroupProds()
        {
            var groupsP = App.Database.GetGroupsProduct().Result;
            if(groupsP.Count != 0)
            {
                foreach(var gr in groupsP)
                {
                    GroupsProducts.Add(gr);
                }
            }
        }
        private async void CostProduct()
        {
            List<ProductModel> lps = await App.Database.ListProduct();
            
            if (lps.Count >= 0)
            {
                //var result = lps.Sum(x => (x.PrecioVentaProduct * x.CantProduct));
                //var costsuma = "$" + result.ToString();

                //Costo = costsuma;
            }
            else
            {
                Costo = "No hay productos registrados";
            }
        }
        private async void NumProducts()
        {
            var resp = await App.Database.GetTotalProducts();
            Productos = resp.ToString();
        }
        private async void ListProductStock()
        {
            ListPS.Clear();
            GroupsSelected = null;
            List<string> products = new List<string>();
            List<ProductModel> lps = await App.Database.ListProduct();
            if (lps != null)
            {
                foreach (var tp in lps.OrderByDescending(x => x.Id))
                {
                    PassString pass = new PassString{
                        Data0 = tp.NameProduct,
                        Data1 = "$" + tp.PrecioVentaProduct.ToString(),
                        //Data2 = tp.CantProduct.ToString(),
                        Data3 = tp.Id.ToString()
                    };
                    ListPS.Add(pass);
                }
            }

        }
        private async void DeletePS(PassString obj)
        {
            var Id = obj.Data3;
            var resu = App.Database.Get1Product(int.Parse(Id)).Result;
            await App.Database.DeleteProduct(resu);
            await Shell.Current.DisplayAlert("Hola", resu.NameProduct + " con precio: " + resu.PrecioVentaProduct + " Ha sido eliminado", "Ok");
            ListProductStock();
            NumProducts();
            CostProduct();
        }
        private async void ModifyPS(PassString obj)
        {
            await Shell.Current.GoToAsync($"//Rini/Productos/NewStock?IdListOC=0&IdlistRM=0&IdlistWF=0&idProduct={obj.Data3}");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string regreso = HttpUtility.UrlDecode(query["Regreso"]);
                if(regreso == "true")
                {
                    ListProductStock();
                    NumProducts();
                    CostProduct();
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }
    }
}

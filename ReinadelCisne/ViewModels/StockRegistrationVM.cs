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
        private int _productos;
        public int Productos
        {
            get => _productos;
            set
            {
                _productos = value;
                OnPropertyChanged();
            }
        }
        
        private double _costo;
        public double Costo
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
        
        public ObservableCollection<ProductModel> ListPS { get; private set; } = new ObservableCollection<ProductModel>();
        public ObservableCollection<GroupsProductModel> GroupsProducts { get; private set; } = new ObservableCollection<GroupsProductModel>();
        public ObservableCollection<KardexModel> ListProductTerminados { get; set; } = new ObservableCollection<KardexModel>();
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsRefreshing = true;

                    ListProductStock();
                    //NumProducts();
                    //CostProduct();

                    IsRefreshing = false;
                });
            }
        }
        public ICommand NewStockCommand => new Command(() =>
        {
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock?IdObjetoconPrecio=0");
        });
        public ICommand SelectedCommand => new Command((obj) =>
        {
            KardexModel pass = obj as KardexModel;
            ListProductStock();
            Shell.Current.GoToAsync($"//Rini/Productos/Kardex?objId={pass.ProductModel.Id}");
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
        public Command<object> Delete { get; }
        public Command<object> Modify { get; }
        
        //Constructor
        public StockRegistrationVM()
        {
            //ListProductStock();
            //NumProducts();
            //CostProduct();
            GroupProds();
            Delete = new Command<object>(DeletePS);
            Modify = new Command<object>(ModifyPS);
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
                        ListPS.Add(tp);
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
                Costo = 0;
            }
        }
        private void NumProducts()
        {
            var resp = App.Database.GetTotalProducts().Result;
            Productos = resp;
        }
        private async void ListProductStock()
        {
            ListProductTerminados.Clear();
            GroupsSelected = null;

            var ListKardProduct = await App.Database.GetAllKardxProduct();

            if(ListKardProduct != null)
            {
                foreach(var kr in ListKardProduct)
                {
                    if(kr.SaldosProducts.Count > 0 && kr.Cantidad > 0)
                    {
                        if(kr.ProductModel != null)
                        {
                            if (kr.ProductModel.EstadoProducto == "Terminado")
                            {
                                ListProductTerminados.Add(kr);
                            }
                        }
                        
                    }
                }
                Productos = ListProductTerminados.Count;
                Costo = ListProductTerminados.Sum(x => x.ValorPromPond * x.Cantidad);
            }
            #region r
            /*List<ProductModel> lps = await App.Database.ListProduct();

            if (lps != null)
            {
                foreach (var tp in lps.Where(x => x.EstadoProducto == "Terminado").OrderByDescending(x => x.Id))
                {
                    tp.AmountProduct = tp.Kardices.Cantidad;
                    tp.PrecioVentaProduct = tp.Kardices.ValorPromPond;
                    ListPS.Add(tp);
                }

                Productos = ListPS.Count;
                Costo = ListPS.Sum(x => x.PrecioVentaProduct * x.AmountProduct);
            } */
            #endregion

        }
        private async void DeletePS(object obj)
        {
            //string Id = obj.Data3;
            var obja = obj as KardexModel; 

            //var resu = App.Database.Get1Product(int.Parse(Id)).Result;
            await App.Database.DeleteProduct(obja.ProductModel);
            await Shell.Current.DisplayAlert("Hola", obja.ProductModel.NameProduct + " con precio: " + obja.ProductModel.PrecioVentaProduct + " Ha sido eliminado", "Ok");
            ListProductStock();
            //NumProducts();
            //CostProduct();
        }
        private async void ModifyPS(object obj)
        {
            var obja = obj as ProductModel;
            await Shell.Current.GoToAsync($"//Rini/Productos/NewStock?IdListOC=0&IdlistRM=0&IdlistWF=0&idProduct={obja.Id}");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string regreso = HttpUtility.UrlDecode(query["Regreso"]);
                if(regreso == "true")
                {
                    ListProductStock();
                    //NumProducts();
                    //CostProduct();
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }
    }
}

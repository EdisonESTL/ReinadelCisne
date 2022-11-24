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
using System.Web;
using ReinadelCisne.Auxiliars;

namespace ReinadelCisne.ViewModels
{
    public class ShoppingVM : BaseVM, IQueryAttributable
    {
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

        private double _preciocompra;
        public double PrecioCompra
        {
            get => _preciocompra;
            set
            {
                _preciocompra = value;
                OnPropertyChanged();
            }
        }

        private int _cantcompra;
        public int CantCompra
        {
            get => _cantcompra;
            set
            {
                _cantcompra = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date = DateTime.Now.Date;
        public DateTime Date
        {
            get { return _date; }
            set 
            { 
                _date = value;
                OnPropertyChanged();
            }
        }

        

        private string _nameEstablishment;
        public string NameEstablishment
        {
            get { return _nameEstablishment; }
            set
            {
                _nameEstablishment = value;
                OnPropertyChanged();
            }
        }

        private string _invoiceNumber;
        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged();
            }
        }
        /*
        private string _id = "0";
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        
        private float _amount;
        public float Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        private string _measurement;
        public string Measurement
        {
            get { return _measurement; }
            set
            {
                _measurement = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _unitCost;
        public string UnitCost
        {
            get { return _unitCost; }
            set
            {
                _unitCost = value;
                OnPropertyChanged();
            }
        }

        private string _totalInv;
        public string TotalInv
        {
            get { return _totalInv; }
            set 
            { 
                _totalInv = value;
                OnPropertyChanged();
            }
        }
        
        private float _count = 0;
        public float Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private int _longList = 0;
        public int LongList
        {
            get { return _longList; }
            set
            {
                _longList = value;
                OnPropertyChanged();
            }
        } */
        public ObservableCollection<RawMaterialModel> ListCompra { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ObservableCollection<RawMaterialModel> NamesRM { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ObservableCollection<PassString> ListPS { get; private set; } = new ObservableCollection<PassString>();

        private ProductModel _product;
        public ProductModel Product 
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            } 
        }
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListRawMl.get

            IsRefreshing = false;
        });
        public ICommand BackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/RCompras/SelectionPS");
        });
        /*public ICommand AddCompra => new Command(() =>
        {
            if(Amount != 0 & !string.IsNullOrEmpty(Description) & !string.IsNullOrEmpty(UnitCost))
            {
                var totalC = (float)(Amount * Convert.ToDouble(UnitCost));

                RawMaterialModel shModel = new RawMaterialModel
                {
                    Id = int.Parse(Id),
                    AmountRM = Convert.ToDouble(Amount),
                    UnitMeasurementRM = Measurement,
                    NameRM = Description,
                    CostoRM = float.Parse(UnitCost),
                    TotalCost = totalC
                };

                if (IdMOD != null)
                {
                    ListCompra.RemoveAt(int.Parse(IdMOD));
                    ListCompra.Insert(int.Parse(IdMOD), shModel);
                    Count = 0;
                    foreach (var a in ListCompra)
                    {
                        Count += (float)(a.CostoRM * a.AmountRM);
                    }

                    IdMOD = null;
                }
                else
                {
                    ListCompra.Add(shModel);
                    Count += totalC;
                }

                LongList = ListCompra.Count;

                TotalInv = Count.ToString("N2") + "$";

                Measurement = string.Empty;
                Description = string.Empty;
                UnitCost = string.Empty;
                Amount = 0;
            }
        });*/

        public ICommand SaveCompra => new Command(() =>
        {
            if(PrecioCompra != 0 && CantCompra > 0)
            {
                if(Product != null)
                {
                    GuardarProd(Product);
                }
                
            }
        });

        

        public ICommand CancelCommand => new Command(() =>
        {
            //ClearShopping();
            Shell.Current.GoToAsync("..");
        });
        public ICommand NewShopCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/RCompras/NewShopping/SelectionPS");
        });
        public ICommand RegistrationComand => new Command(() =>
        {
            Shell.Current.GoToAsync("ShoppingRegister");
        });
        public Command<RawMaterialModel> DeleteCommand { get; set; }
        public Command<RawMaterialModel> ModifyCommand { get; set; }
        public ShoppingVM()
        {
            //DeleteCommand = new Command<RawMaterialModel>(DeleteItemShop);
            //ModifyCommand = new Command<RawMaterialModel>(ModifyItemShop);
            //LoadNames();
        }

        /*private void LoadNames()
        {
            NamesRM.Clear();
            var a = App.Database.GetMR().Result;

            foreach(var b in a)
            {
                NamesRM.Add(b);
            }
        }*/

        public string IdMOD;
        /*private void ModifyItemShop(RawMaterialModel obj)
        {
            IdMOD = Convert.ToString(ListCompra.IndexOf(obj));

            Id = obj.Id.ToString();
            Amount = (float)obj.AmountRM;
            Measurement = obj.UnitMeasurementRM;
            Description = obj.NameRM;
            UnitCost = obj.CostoRM.ToString("N2");
        }
        private void DeleteItemShop(RawMaterialModel obj)
        {
            ListCompra.Remove(obj);
            Count = 0;
            foreach (var a in ListCompra)
            {
                Count += (float)(a.CostoRM * a.AmountRM);
            }
            TotalInv = Count.ToString("N2") + "$";
            LongList = ListCompra.Count;
        }
        private void ClearShopping()
        {
            InvoiceNumber = string.Empty;
            NameEstablishment = string.Empty;
            Measurement = string.Empty;
            Description = string.Empty;
            UnitCost = string.Empty;
            TotalInv = string.Empty;
            Amount = 0;
            Count = 0;
            LongList = 0;
            ListCompra.Clear();
        }

        public List<RawMaterialModel> rgm { get; set; } = new List<RawMaterialModel>();
        private async void SaveShopping()
        {
            ShoppingModel shopping = new ShoppingModel
            {
                ShoppingDate = Date,
                NameEstablishment = NameEstablishment,
                InvoiceNumber = InvoiceNumber,
                TotalShop = Count
            };

            await App.Database.SaveShopping(shopping);

            foreach (var obj in ListCompra)
            {
                RawMaterialModel rawMaterial = new RawMaterialModel
                {
                    Id = obj.Id,
                    NameRM = obj.NameRM,
                    AmountRM = obj.AmountRM,
                    CostoRM = obj.CostoRM, 
                    UnitMeasurementRM = obj.UnitMeasurementRM
                };

                ShoppingListModel shoppingItem = new ShoppingListModel
                {
                    Amount = obj.AmountRM,
                    ValorUnitario = Convert.ToDouble(obj.CostoRM),
                    TotalCost = Convert.ToDouble(obj.AmountRM * Convert.ToDouble(obj.CostoRM))
                };

                await App.Database.SaveRawMaterial(rawMaterial);
                await App.Database.SaveListShop(shoppingItem);
                
                shoppingItem.RawMaterial = new RawMaterialModel();
                shoppingItem.RawMaterial = rawMaterial;
                shoppingItem.ShoppingModel = new ShoppingModel();
                shoppingItem.ShoppingModel = shopping;

                await App.Database.UpdateRelationsListShop(shoppingItem);
            }

            //await Shell.Current.DisplayAlert("Éxito", "se guardo la compra", "ok");
            LoadNames();
            UpdateStock();
            //UpdateWeightedAveragePrice();
            //var op = App.Database.GetMR().Result;
            //var ops = App.Database.ListShoppingList().Result;
            ClearShopping();
        }

        private void UpdateWeightedAveragePrice()
        {
            List<ShoppingListModel> shops = App.Database.ListShoppingList().Result;
            float sumprice = 0;
            int cc = 0;
            var RMgroups = (from p in shops
                      group p by p.RawMaterialModelId).ToList(); ;

            foreach (var rm in RMgroups)
            {
                cc = rm.Count();

                foreach (var mm in rm)
                {
                    sumprice += (float)mm.ValorUnitario;
                }
                var pond = sumprice / cc;

                var rmup = App.Database.GetOneRM(rm.Key).Result;
                rmup.CostoRM = pond;
                App.Database.UpdateRawMaterial(rmup);
            }
        }
        private void UpdateStock()
        {
            List<RawMaterialModel> rawsUp = new List<RawMaterialModel>();
            foreach(var rr in ListCompra)
            {
                rawsUp.Add(rr);
            }
            App.Database.UpdateInvRM(rawsUp);
            //var fgh = App.Database.GetMR().Result;
        }*/

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string regreso = HttpUtility.UrlDecode(query["objId"]);
                string TipoElemento = HttpUtility.UrlDecode(query["TipoElemento"]);
                cargarLItem(regreso, TipoElemento);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private void cargarLItem(string regreso, string tipoElemento)
        {
            switch (tipoElemento)
            {
                case "producto":
                    RecuperarProducto(regreso);
                    break;
                case "materiaprima":
                    RecuperarMateriaPrima(regreso);
                    break;
                default:
                    break;
            }
        }

        /*private void CargarListaCompra(Task<ProductModel> prod)
        {
            throw new NotImplementedException();
        }*/
        
        private void RecuperarMateriaPrima(string regreso)
        {
            RawMaterialModel recup = App.Database.GetOneRM(int.Parse(regreso)).Result;


            /*PassString pass = new PassString
            {
                Data0 = recup.Id.ToString(),
                Data1 = recup.NameRM,
                Data2 = recup.AmountRM.ToString(),
                Data3 = "$" + recup.CostoRM.ToString(),
                Data4 = "$",
                Data5 = "materiaprima"
            };
            ListPS.Add(pass);*/
        }

        
        private void RecuperarProducto(string regreso)
        {            
            Product = App.Database.Get1Product(int.Parse(regreso)).Result;
            /*Product = recup;
            PassString pass = new PassString
            {
                Data0 = recup.Id.ToString(),
                Data1 = recup.NameProduct,
                Data2 = recup.CantProduct.ToString(),
                Data3 = "$"+recup.PrecioVentaProduct.ToString(),
                Data4 = "$"+recup.CostElaboracionProduct.ToString(),
                Data5 = "producto"
            };
            ListPS.Add(pass);*/
        }

        private void GuardarProd(ProductModel product)
        {
            ProductShoppingModel shopping = new ProductShoppingModel
            {
                ShoppingDate = Date,
                NameEstablishment = NameEstablishment,
                InvoiceNumber = InvoiceNumber,
                TotalShop = (float)(CantCompra * PrecioCompra)
            };

            App.Database.SaveShoppingProduct(shopping);

            ProductShoppingList shoppingList = new ProductShoppingList
            {
                Amount = CantCompra,
                ValorUnitario = PrecioCompra
            };

            foreach (var obj in ListCompra)
            {
                RawMaterialModel rawMaterial = new RawMaterialModel
                {
                    Id = obj.Id,
                    NameRM = obj.NameRM,
                    CantidadRM = obj.CantidadRM,
                    CostoRM = obj.CostoRM,
                    UnitMeasurementRM = obj.UnitMeasurementRM
                };

                ShoppingListModel shoppingItem = new ShoppingListModel
                {
                    Amount = obj.CantidadRM,
                    ValorUnitario = Convert.ToDouble(obj.CostoRM),
                    TotalCost = Convert.ToDouble(obj.CantidadRM * Convert.ToDouble(obj.CostoRM))
                };

                App.Database.SaveRawMaterial(rawMaterial);
                App.Database.SaveListShop(shoppingItem);

                /*shoppingItem.RawMaterial = new RawMaterialModel();
                shoppingItem.RawMaterial = rawMaterial;*/
                shoppingItem.ShoppingModel = new ShoppingModel();
                //shoppingItem.ShoppingModel = sh

                App.Database.UpdateRelationsListShop(shoppingItem);
            }

            //await Shell.Current.DisplayAlert("Éxito", "se guardo la compra", "ok");
            //LoadNames();
            //UpdateStock();
            //UpdateWeightedAveragePrice();
            //var op = App.Database.GetMR().Result;
            //var ops = App.Database.ListShoppingList().Result;
            //ClearShopping();
        }
    }
}

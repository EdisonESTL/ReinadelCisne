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
        private RawMaterialModel _rawMat;
        public RawMaterialModel RawMat
        {
            get => _rawMat;
            set
            {
                _rawMat = value;
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
        private DateTime _date = DateTime.Now;
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
            Shell.Current.GoToAsync("//Rini/RMateriaPrima/RCompras/SelectionRM");
        });
        

        public ICommand SaveCompra => new Command(() =>
        {
            if (PrecioCompra != 0 && CantCompra > 0)
            {
                GuardarComp();
                Shell.Current.DisplayAlert("Éxito", "se ha guardado", "ok");
                Shell.Current.GoToAsync("//Rini/RMateriaPrima");
            }
            
            
        });

        
        public ICommand CancelCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/RMateriaPrima/RCompras/SelectionRM");
        });
        public ICommand NewShopCommand => new Command(() =>
        {
            if (PrecioCompra != 0 && CantCompra > 0)
            {
                GuardarComp();
                Shell.Current.GoToAsync("//Rini/RCompras/NewShopping/SelectionPS");
            }
            
        });
        public ICommand RegistrationComand => new Command(() =>
        {
            Shell.Current.GoToAsync("ShoppingRegister");
        });
        public Command<RawMaterialModel> DeleteCommand { get; set; }
        public Command<RawMaterialModel> ModifyCommand { get; set; }
        public ShoppingVM()
        {
        }

        private void GuardarComp()
        {
            List<KardexRMModel> ListKar = App.Database.GetKardexsRM().Result;
            KardexRMModel Kardex = (from a in ListKar
                          where a.IdRawMaterial == RawMat.Id
                          select a).FirstOrDefault();

            #region Guardar compra
            ShoppingModel shopping = new ShoppingModel
            {
                ShoppingDate = DateTime.Now,
                TotalShop = (float)(PrecioCompra * CantCompra)
            };
            var cc = App.Database.SaveShopping(shopping);
            cc.Wait();
            ShoppingListModel shoppingList = new ShoppingListModel
            {
                Date = DateTime.Now,
                Amount = CantCompra,
                ValorUnitario = PrecioCompra,
                TotalCost = CantCompra * PrecioCompra
            };
            var ff = App.Database.SaveListShop(shoppingList);
            ff.Wait();

            shoppingList.ShoppingModel = shopping;
            shoppingList.KardexRMModel = Kardex;

            App.Database.UpdateRelationsListShop(shoppingList);

            Kardex.ShoppingModell.Add(shoppingList);
            App.Database.UpdateRelationsKardexRM(Kardex);

         
            #endregion

            ActualizarSaldos(Kardex, shopping.Id);

        }

        private void ActualizarSaldos(KardexRMModel kardex, int id)
        {
            var saldos = App.Database.GetSaldosxKardex(kardex).Result;
            var ultsaldo = saldos.LastOrDefault();

            double newCant = ultsaldo.Cantidad + CantCompra;
            double newSaldo = ultsaldo.SaldoTotal + (CantCompra * PrecioCompra);

            SaldosRMModel saldosRM = new SaldosRMModel()
            {
                Date = Date,
                Cantidad = newCant,
                SaldoTotal = newSaldo,
                ValorUnitario = newSaldo / newCant,
                IdReconcimiento = id,
                NombreReconocimiento = "Compra"
            };

            App.Database.SaveSaldoRM(saldosRM);

            kardex.SaldosRMs.Add(saldosRM);

            App.Database.UpdateRelationsKardexRM(kardex);

        }

        public string IdMOD;
        
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string idRM = HttpUtility.UrlDecode(query["IdRM"]);
                CargarLItem(idRM);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private void CargarLItem(string idRM)
        {
            var rM = App.Database.GetOneRM(int.Parse(idRM)).Result;
            var krd = App.Database.GetKardexXRM(rM).Result;
            var saldo = App.Database.GetSaldosxKardex(krd).Result;

            var ultsal = saldo.LastOrDefault();

            rM.CantidadRM = ultsal.Cantidad;
            rM.CostoRM = (float)ultsal.ValorUnitario;
            rM.TotalCost = (float)ultsal.SaldoTotal;

            RawMat = rM;
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

                shoppingItem.ShoppingModel = new ShoppingModel();

                App.Database.UpdateRelationsListShop(shoppingItem);
            }

        }
    }
}

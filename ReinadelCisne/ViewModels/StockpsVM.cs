using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class StockpsVM : BaseVM, IQueryAttributable
    {
        private ProductModel _objMod;
        public ProductModel ObjModify { get => _objMod; set { _objMod = value; OnPropertyChanged(); } }

        private GroupsProductModel _groupProd;
        public GroupsProductModel GroupProd { get => _groupProd; set { _groupProd = value; OnPropertyChanged(); } }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _namePS;
        public string NamePS
        {
            get => _namePS;
            set
            {
                _namePS = value;
                OnPropertyChanged();
            }
        }

        private string _descripcionPS;
        public string DescripcionPS
        {
            get => _descripcionPS;
            set
            {
                _descripcionPS = value;
                OnPropertyChanged();
            }
        }

        private string _pricePS;
        public string PricePS
        {
            get => _pricePS;
            set
            {
                _pricePS = value;
                OnPropertyChanged();
            }
        }

        private string _unidadmedidaPS = "Unidad";
        public string UnidaddMedidaPS
        {
            get => _unidadmedidaPS;
            set
            {
                _unidadmedidaPS = value;
                OnPropertyChanged();
            }
        }

        private double _costounitarioPS;
        public double CostoUnitarioPS
        {
            get => _costounitarioPS;
            set
            {
                _costounitarioPS = value;
                OnPropertyChanged();
            }
        }
        
        private int _unitsPS = 1;
        public int UnitPS
        {
            get => _unitsPS;
            set
            {
                _unitsPS = value;
                OnPropertyChanged();
            }
        }
        
        private double _utilityPS = 1;
        public double UtilityPS
        {
            get => _utilityPS;
            set
            {
                _utilityPS = value;
                OnPropertyChanged();
            }
        }   
                
        private double _rawMaterialT;
        public double RawMaterialT 
        {
            get { return _rawMaterialT; }
            set 
            { 
                _rawMaterialT = value;
                OnPropertyChanged();
            }
        }

        private double _workForceT = 0;
        public double WorkForceT
        {
            get { return _workForceT; }
            set
            {
                _workForceT = value;
                OnPropertyChanged();
            }
        }

        private double _otherCostT = 0;
        public double OtherCostT
        {
            get { return _otherCostT; }
            set
            {
                _otherCostT = value;
                OnPropertyChanged();
            }
        }

        
        public int IdListRM;
        public int IdListWF;
        public int IdListOC;

        public ObservableCollection<GroupsProductModel> GroupsProducts { get; set; } = new ObservableCollection<GroupsProductModel>();
        /*-----------------------------------------------*/
        #region Icommands
        public ICommand Save { get; }
        public ICommand Cancel { get; }
        public ICommand Registration { get; }
        public ICommand RawMaterialCommand { get; set; }
        public ICommand WorkForceCommand { get; set; }
        public ICommand OtherCostCommand { get; set; }
        public ICommand CalculatePriceCommand => new Command(() =>
        {
            CalculateCost();
        });
        public ICommand SumarCommand => new Command(() =>
        {
            UnitPS += 1;
        });
        public ICommand RestarCommand => new Command(() =>
        {
            if (UnitPS >= 0)
            {
                UnitPS -= 1;
            }
        });
        public ICommand GoBackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/Productos");
        }); 
        //Nuevo Grupo
        public ICommand NewGroup => new Command(async () =>
        {
            string result = await Shell.Current.DisplayPromptAsync("Nuevo Grupo", "¿Cuál es el nombre del grupo?");

            if (!string.IsNullOrEmpty(result))
            {
                var resp = App.Database.GetGroupsProduct().Result;
                var coincidencias = resp.Where(x => x.Description == result).ToList();

                if (coincidencias.Count == 0)
                {
                    CrearNuevoGrupo(result);
                    await Shell.Current.DisplayAlert("Éxito", "El nuevo grupo ha sido guardado", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Ya existe un grupo con ese nombre", "OK");
                }
            }

        });

        public ICommand CostoProduccionCommand => new Command(async () =>
        {
            string cnt = await Shell.Current.DisplayPromptAsync("Pregunta", "¿Cuántas unidades va a fabricar?");
            if (UnitPS > 0)
            {
                cnt = UnitPS.ToString();
            }
            if (!string.IsNullOrEmpty(cnt))
            {
                UnitPS = int.Parse(cnt);
                await Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM=0&IdWF=0");

            }
        });
        #endregion
        /*---------------------------------------------------*/
        public StockpsVM()
        {
            
            Save = new Command(SavePS);
            Cancel = new Command(CancelPS);
            Registration = new Command(RegistrationPS);
            RawMaterialCommand = new Command(RawMaterial);
            WorkForceCommand = new Command(WorkForce);
            OtherCostCommand = new Command(OtherCost);

            ListaGruposProductos();
        }
        private void CrearNuevoGrupo(string result)
        {
            GroupsProductModel groups = new GroupsProductModel
            {
                Description = result
            };
            
            App.Database.SaveGRoupProduct(groups);
            ListaGruposProductos();
        }
        private void ListaGruposProductos()
        {
            GroupsProducts.Clear();
            var grupos = App.Database.GetGroupsProduct();

            foreach(var a in grupos.Result)
            {
                GroupsProducts.Add(a);
            }
            //GroupsProducts = new ObservableCollection<GroupsProductModel>(grupos.Result);
        }
        private void CalculateCost()
        {
            double ManufacturingCosto  = Convert.ToDouble(RawMaterialT) + WorkForceT + OtherCostT; //Costo de fabricación
            double ManufacturingUnitCosto = ManufacturingCosto / UnitPS;  //Costo unitario de fabricación
            double UnitSalePrice = ManufacturingUnitCosto + (ManufacturingUnitCosto * (UtilityPS / 100));
            PricePS = UnitSalePrice.ToString("N2");
        }
        private void RegistrationPS()
        {           
            Shell.Current.GoToAsync("StockRegistration");
        }
        private void CancelPS()
        {
            NamePS = string.Empty;
            DescripcionPS = string.Empty;
            PricePS = string.Empty;
            UnidaddMedidaPS = string.Empty;
            CostoUnitarioPS = 0;
            UnitPS = 0;


            PricePS = string.Empty;

            IdListRM = 0;
            IdListWF = 0;
            IdListOC = 0;
        }
        private void SavePS()
        {            
            if (!string.IsNullOrEmpty(NamePS) && !string.IsNullOrEmpty(PricePS))
            {
                //double precio = double.Parse(PricePS);

                ProductModel product = new ProductModel()
                {
                    Id = Id,
                    NameProduct = NamePS,
                    DateTime = DateTime.Now,
                    DescriptionProduct = DescripcionPS,
                    UnidadMedida = UnidaddMedidaPS,
                    PrecioVentaProduct = double.Parse(PricePS),
                    CostElaboracionProduct = CostoUnitarioPS
                };

                var resp = App.Database.SaveProduct(product);
                resp.Wait();

                //Trabajo con materia prima
                if (IdListRM != 0)
                {
                    var Lrm = App.Database.GetListRM(IdListRM).Result;
                    product.ListRMModel = Lrm;
                }
                
                //Trabajo con mano de obra
                if (IdListWF != 0)
                {
                    var Lwf = App.Database.GetListWF(IdListWF).Result;
                    product.ListWFModel = Lwf;
                }

                //trabajo con otros costos
                if (IdListOC != 0)
                {
                    var Loc = App.Database.GetListOC(IdListOC).Result;
                    product.ListOCModel = Loc;
                }

                product.Groups = GroupProd;
                //Actualizo relación de productos
                App.Database.UpdateRelationsRM(product);

                if (resp != null)
                {
                    Shell.Current.DisplayAlert("Éxito", "Se guardo " + product.NameProduct, "ok");
                    CrearKardex(product, resp.Result);
                    product.Id = 0;
                    CancelPS();
                    Shell.Current.GoToAsync($"//Rini/Productos?Regreso=true");
                }
                else
                {
                    Shell.Current.DisplayAlert("Error", "No se ha podido guardar", "ok");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Ingrese el nombre del producto y el precio de venta", "ok");
            }
        }

        private void CrearKardex(ProductModel product, int resul)
        {
            //var conob = App.Database.Get1Product(product.Id).Result;
            if (resul == 2)
            {
                KardexModel kardex = new KardexModel
                {
                    Date = DateTime.Now,
                    //Valor = product.PrecioVentaProduct * product.CantProduct,
                    //Cantidad = product.CantProduct,
                    ValorPromPond = product.PrecioVentaProduct 
                };

                App.Database.SaveMovKardex(kardex);

                kardex.ProductModel = product;
                App.Database.UpdateRelationKardexProduct(kardex);
                Shell.Current.DisplayAlert("Exito", "Se creo kardex", "ok");
            }
            if(resul == 1)
            {
                try
                {
                    var bb = ObjModify.Kardices;
                    var d = bb[0];

                    d.Valor = product.PrecioVentaProduct;
                    //d.Cantidad = product.CantProduct;
                    d.ValorPromPond = product.PrecioVentaProduct;
                    App.Database.SaveMovKardex(d);

                    /*
                    var rr = App.Database.GetFirstKardex(product);
                    rr.Wait();
                    KardexModel rest = rr.Result;
                    rest.ValorUnitario = product.PrecioVentaProduct;
                    rest.Cantidad = product.CantProduct;
                    rest.ValorPromPond = product.PrecioVentaProduct;

                    App.Database.SaveMovKardex(rest);

                    rest.ProductModel = product;
                    App.Database.UpdateRelationKardexProduct(rest);*/
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to load idproduct.");
                }
                
                //obtener id kardex
                //obtener el primer kardex
            }
            
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string idListWF = HttpUtility.UrlDecode(query["IdListWF"]);    //Recepcion del Id de lista de otros costos            
            string idListOC = HttpUtility.UrlDecode(query["IdListCI"]);    //Recepcion del Id de lista de materiales            
            string idListRM = HttpUtility.UrlDecode(query["IdListRM"]); //Recepción Id de lista de fuerza de trabajo
            string idProduct = HttpUtility.UrlDecode(query["IdProduct"]); //Recepción Id del producto para modificar

            Enrutar(idListOC, idListRM, idListWF, idProduct);            
        }
        private void Enrutar(string idListOC, string idListRM, string idListWF, string idProduct)
        {
            try
            {
                if (int.Parse(idListOC) >= 0 &&
                int.Parse(idListRM) >= 0 &&
                int.Parse(idListWF) >= 0)
                {
                    LoadDates(idListOC, idListRM, idListWF);    //Carga Otros Costos
                }
                if (int.Parse(idProduct) != 0)
                {
                    LoadProductS(idProduct);      //carga producto a modificar
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
                      
        }
        private async void LoadDates(string idListOC, string idListRM, string idListWF)
        {
            OtherCostT = 0;
            if(int.Parse(idListOC) > 0)
            {
                ListOCModel listOC = await App.Database.GetListOC(int.Parse(idListOC));
                OtherCostT = listOC.OtherCostsxProduct.Sum(x => x.CostOC);
            }
            
            if(int.Parse(idListWF) > 0)
            {
                WorkForceT = 0;
                ListWFModel l = await App.Database.GetListWF(int.Parse(idListWF));
                WorkForceT = l.PersonalxProduct.Sum(x => x.Pay);
            }
            
            if(int.Parse(idListRM) > 0)
            {
                RawMaterialT = 0;
                ListRMModel li = await App.Database.GetListRM(int.Parse(idListRM));
                RawMaterialT = li.ListMaterialxProduct.Sum(x => x.TotalCost);
            }            

            CostoUnitarioPS = (OtherCostT + WorkForceT + RawMaterialT) / UnitPS;
        }
        private async void LoadProductS(string idProd)
        {
            if(int.Parse(idProd) != 0)
            {               
                int di = int.Parse(idProd);
                ObjModify = await App.Database.Get1Product(di);
                Show(ObjModify);
            }
        }
        private void Show(ProductModel product)
        {
            Id =  product.Id;
            NamePS = product.NameProduct;
            /*
            if(product.ListRMModelId != 0) 
            {
                LoadRawMaterials(Convert.ToString(product.ListRMModelId));
            }
            if(product.ListWFModelId!= 0) 
            {
                LoadWorkForce(Convert.ToString(product.ListWFModelId));
            }
            if(product.ListOCModelId != 0)
            {
                LoadOtherCosts(Convert.ToString(product.ListOCModelId));
            }  */                    
            
            //UnitPS = product.CantProduct;
            //UtilityPS = product.UtilityProduct;
            PricePS = product.PrecioVentaProduct.ToString("N2");
        }
        private void RawMaterial()
        {
            //var f = App.Database.GetListRM(IdListRM).Result;
            Shell.Current.GoToAsync($"InRawMaterial?IdlistRM={IdListRM}");
        }
        private void WorkForce()
        {
            Shell.Current.GoToAsync($"InWorkForce?IdListWF={IdListWF}");
        }
        private void OtherCost()
        {
            Shell.Current.GoToAsync($"InOtherCost?IdListOC={IdListOC}");
        }
    }
}

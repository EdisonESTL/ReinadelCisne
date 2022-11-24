using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private string _unidadmedidaPS;
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
                
        private string _rawMaterialT;
        public string RawMaterialT 
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
        /*-----------------------------------------------*/
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
            if(UnitPS >= 0)
            {
                UnitPS -= 1;
            }            
        });
        public ICommand GoBackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("..");
        });
        /*---------------------------------------------------*/
        public StockpsVM()
        {
            
            Save = new Command(SavePS);
            Cancel = new Command(CancelPS);
            Registration = new Command(RegistrationPS);
            RawMaterialCommand = new Command(RawMaterial);
            WorkForceCommand = new Command(WorkForce);
            OtherCostCommand = new Command(OtherCost);            
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
                    CantProduct = UnitPS,
                    UnidadMedida = UnidaddMedidaPS,
                    PrecioVentaProduct = double.Parse(PricePS),
                    CostElaboracionProduct = CostoUnitarioPS
                };

                var resp = App.Database.SaveProduct(product);

                //Trabajo con materia prima
                if (IdListRM == 0)
                {
                    product.ListRMModel = null;
                }
                else
                {
                    var Lrm = App.Database.GetListRM(IdListRM).Result;
                    product.ListRMModel = Lrm;
                }

                //Trabajo con mano de obra
                if (IdListWF == 0)
                {
                    product.ListWFModel = null;
                }
                else
                {
                    var Lwf = App.Database.GetListWF(IdListWF).Result;
                    product.ListWFModel = Lwf;
                }

                //trabajo con otros costos
                if (IdListOC == 0)
                {
                    product.ListOCModel = null;
                }
                else
                {
                    var Loc = App.Database.GetListOC(IdListOC).Result;
                    product.ListOCModel = Loc;
                }
                

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
                Shell.Current.DisplayAlert("Error", "Ingrese el nombre del producto y el precio", "ok");
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
                    Valor = product.PrecioVentaProduct * product.CantProduct,
                    Cantidad = product.CantProduct,
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
                    d.Cantidad = product.CantProduct;
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
            try
            {                
                string idListOC = HttpUtility.UrlDecode(query["IdListOC"]);    //Recepcion del Id de lista de otros costos            
                string idListRM = HttpUtility.UrlDecode(query["IdlistRM"]);    //Recepcion del Id de lista de materiales            
                string idListWF = HttpUtility.UrlDecode(query["IdlistWF"]); //Recepción Id de lista de fuerza de trabajo
                string idProduct = HttpUtility.UrlDecode(query["idProduct"]); //Recepción Id del producto para modificar

                Enrutar(idListOC, idListRM, idListWF, idProduct);
                
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }
        private void Enrutar(string idListOC, string idListRM, string idListWF, string idProduct)
        {
            if (int.Parse(idListOC) != 0)
            {
                LoadOtherCosts(idListOC);    //Carga Otros Costos
            }
            if (int.Parse(idListRM) != 0)
            {
                LoadRawMaterials(idListRM);    //Carga Materia Prima
            }
            if(int.Parse(idListWF) != 0)
            {
                LoadWorkForce(idListWF);       //Carga mano de obra
            }
            if (int.Parse(idProduct) != 0)
            {
                LoadProductS(idProduct);      //carga producto a modificar
            }            
        }
        private async void LoadOtherCosts(string idListOC)
        {
            OtherCostT = 0;
            ListOCModel listOC = await App.Database.GetListOC(int.Parse(idListOC));
            List<OtherCostModel> otherCosts = listOC.OtherCosts;

            foreach(var a in otherCosts)
            {
                OtherCostT += a.CostOC;
            }
            IdListOC = int.Parse(idListOC);
        }
        private async void LoadWorkForce(string id)
        {
            WorkForceT = 0;
            ListWFModel l = await App.Database.GetListWF(int.Parse(id));
            List<WorkForceModel> b = l.WorkForces;

            foreach (var a in b)
            {
                WorkForceT = WorkForceT + (a.Amount * a.UnitSalary);
            }
            IdListWF = int.Parse(id);
        }
        private async void LoadRawMaterials(string id)
        {
            RawMaterialT = string.Empty;
            //float d = 0;
            ListRMModel l = await App.Database.GetListRM(int.Parse(id));
            //List<RawMaterialModel> b = l.RawMaterials;
            /*
            foreach(var a in b)
            {
                d += (float)(a.CostoRM * a.AmountRM);
            }*/
            RawMaterialT = l.Total.ToString("N2");
            IdListRM = int.Parse(id);
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
            }                      
            
            UnitPS = product.CantProduct;
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

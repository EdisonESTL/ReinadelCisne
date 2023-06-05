using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using ReinadelCisne.Models;
using ReinadelCisne.Auxiliars;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;

namespace ReinadelCisne.ViewModels
{
    public class StockpsVM : BaseVM, IQueryAttributable
    {
        public float _max1;
        public float _max2;
        public float _min1;
        public float _min2;

        private bool _verPuntoEquilibrio = false;
        public bool VerPuntoEquilibrio
        {
            get => _verPuntoEquilibrio;
            set
            {
                if(value != _verPuntoEquilibrio)
                {
                    _verPuntoEquilibrio = value;
                    OnPropertyChanged();
                }
            }
        }
        public float Max1 
        { 
            get => _max1; 
            set {
                if (value != _max1)
                {
                    _max1 = value;
                    OnPropertyChanged();
                }
            } 
        }
        public float Max2
        {
            get => _max2;
            set
            {
                if (value != _max2)
                {
                    _max2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public float Min1
        {
            get => _min1;
            set
            {
                if (value != _min1)
                {
                    _min1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public float Min2
        {
            get => _min2;
            set
            {
                if (value != _min2)
                {
                    _min2 = value;
                    OnPropertyChanged();
                }
            }
        }
        private ProductModel _objMod;
        public ProductModel ObjModify 
        { 
            get => _objMod; 
            set 
            { 
                if(value != _objMod)
                {
                    _objMod = value;
                    OnPropertyChanged();
                }
                
            }
        }

        private GroupsProductModel _groupProd;
        public GroupsProductModel GroupProd { get => _groupProd; set { _groupProd = value; OnPropertyChanged(); } }

        private LineChart _oferta;
        public LineChart Oferta
        {
            get => _oferta;
            set
            {
                if (value != _oferta)
                {
                    _oferta = value;
                    OnPropertyChanged();
                }
            }
        }

        private LineChart _demanda;
        public LineChart Demanda
        {
            get => _demanda;
            set
            {
                if (value != _demanda)
                {
                    _demanda = value;
                    OnPropertyChanged();
                }
            }
        }

        private MultinesChartAuxiliar _multines;
        public MultinesChartAuxiliar Multines
        {
            get => _multines;
            set
            {
                if (value != _multines)
                {
                    _multines = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _pricePS = 0;
        public double PricePS
        {
            get => _pricePS;
            set
            {
                if(value != _pricePS)
                {
                    _pricePS = value;
                    OnPropertyChanged();
                    //CalcularPorcentajGanacia();
                }
                
            }
        }

        #region DatosPuntoEquilibrio

        private int _unitsPS = 0; //Cantidad1Oferta
        public int UnitPS
        {
            get => _unitsPS;
            set
            {
                _unitsPS = value;
                OnPropertyChanged();
                PuntosLinea1.Clear();
                CalcularOferta();
                //OfertaCalculo();
                InitDataChart();
                PuntoEquilibrio();
            }
        }

        private double _costounitarioPS = 0; //Precio1Oferta
        public double CostoUnitarioPS
        {
            get => _costounitarioPS;
            set
            {
                _costounitarioPS = value;
                OnPropertyChanged();
                PuntosLinea1.Clear();
                CalcularOferta();
                //OfertaCalculo();
                InitDataChart();
                PuntoEquilibrio();
            }
        }
        
        private double _precio2Oferta = 0;
        public double Precio2Oferta
        {
            get => _precio2Oferta;
            set
            {
                if (value != _precio2Oferta)
                {
                    _precio2Oferta = value;
                    OnPropertyChanged();
                    PuntosLinea1.Clear();
                    CalcularOferta();
                    //OfertaCalculo();
                    InitDataChart();
                    PuntoEquilibrio();
                }
            }
        }

        private int _cantidad2Oferta = 0;
        public int Cantidad2Oferta
        {
            get => _cantidad2Oferta;
            set
            {
                if (value != _cantidad2Oferta)
                {
                    _cantidad2Oferta = value;
                    OnPropertyChanged();
                    PuntosLinea1.Clear();
                    CalcularOferta();
                    //OfertaCalculo();
                    InitDataChart();
                    PuntoEquilibrio();
                }
            }
        }

        private double _precio1Demanda = 0;
        public double Precio1Demanda
        {
            get => _precio1Demanda;
            set
            {
                if (value != _precio1Demanda)
                {
                    _precio1Demanda = value;
                    OnPropertyChanged();
                    PuntosLinea2.Clear();
                    CalcularDemanda();
                    InitDataChart();
                    PuntoEquilibrio();
                }
            }
        }
        private double _precio2Demanda = 0;
        public double Precio2Demanda
        {
            get => _precio2Demanda;
            set
            {
                if (value != _precio2Demanda)
                {
                    _precio2Demanda = value;
                    OnPropertyChanged();
                    PuntosLinea2.Clear();
                    CalcularDemanda();
                    InitDataChart();
                    PuntoEquilibrio();
                }
            }
        }

        private int _cantidad1Demanda= 0;
        public int Cantidad1Demanda
        {
            get => _cantidad1Demanda;
            set
            {
                if (value != _cantidad1Demanda)
                {
                    _cantidad1Demanda = value;
                    OnPropertyChanged();
                    PuntosLinea2.Clear();
                    CalcularDemanda();
                    InitDataChart();
                    PuntoEquilibrio();
                }
            }
        }
        private int _cantidad2Demanda = 0;
        public int Cantidad2Demanda
        {
            get => _cantidad2Demanda;
            set
            {
                if (value != _cantidad2Demanda)
                {
                    _cantidad2Demanda = value;
                    OnPropertyChanged();
                    PuntosLinea2.Clear();
                    CalcularDemanda();
                    InitDataChart();
                    PuntoEquilibrio();
                }
            }
        }
        private float _qPromedio = 0;
        public float QPromedio
        {
            get => _qPromedio;
            set
            {
                _qPromedio = value;
                OnPropertyChanged();
            }
        }
        private float _pPromedio = 0;
        public float PPromedio
        {
            get => _pPromedio;
            set
            {
                _pPromedio = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private string[] months = new string[] { "1", "2", "3" };
        private float[] valuemont = new float[] { 187, 187 };
        private float[] valuemont2 = new float[] { 0, 71 };
        private List<float> PuntosLinea1 { get; set; } = new List<float>();
        private List<float> PuntosLinea2 { get; set; } = new List<float>();

        private SKColor blueColor = SKColor.Parse("#09C");
        private SKColor redColor = SKColor.Parse("#CC0000");

        double porcntGanacia = 0;
        public double PorcentGanancia
        {
            get => porcntGanacia;
            set
            {
                if (value != porcntGanacia)
                {
                    porcntGanacia = value;
                    OnPropertyChanged();
                    CalcularPrecioVenta();
                }
            }
        }

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
        //private IEnumerable<ChartEntry> entries;

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
        public ICommand ViewPuntoQeCommand => new Command(() =>
        {
            if(VerPuntoEquilibrio )
            {
                VerPuntoEquilibrio = false; 
            }
            else
            {
                VerPuntoEquilibrio = true;
            }
        });
        public ICommand CostoProduccionCommand => new Command(async () =>
        {
            if (!string.IsNullOrEmpty(NamePS))
            {
                string cnt = await Shell.Current.DisplayPromptAsync("Pregunta", "¿Cuántas unidades va a fabricar?");


                if (!string.IsNullOrEmpty(cnt))
                {
                    UnitPS = int.Parse(cnt);
                    await Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?ListMaterialId=0&IdWF=0&IdListaMaquinaria=0&CantProd={UnitPS}&NameProd={NamePS}");

                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Campo vacio", "El campo NOMBRE esta vacio, por favor " +
                    "ingrese un nombre al producto y vuelva a intentar.", "Ok");
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
            CalcularDemanda();
            PuntoEquilibrio();
        }
        private void PuntoEquilibrio()
        {
            //Valores oferta
            //var P1o = CostoUnitarioPS;
            var P1o = PricePS;
            var P2o = Precio2Oferta;
            var Q1o = UnitPS;
            var Q2o = Cantidad2Oferta;

            //Valores Demanda
            var P1d = Precio1Demanda;
            var P2d = Precio2Demanda;
            var Q1d = Cantidad1Demanda;
            var Q2d = Cantidad2Demanda;

            if (P1d == 0 && P2d == 0 && Q1d == 0 && Q2d == 0)
            {
                QPromedio = 0;
                PPromedio = 0;
            }
            else
            {
                var Q = (P1d - P1o + ((P2o - P1o) / (Q2o - Q1o) * Q1o) - ((P2d - P1d) / (Q2d - Q1d) * Q1d)) / (((P2o - P1o) / (Q2o - Q1o)) - ((P2d - P1d) / (Q2d - Q1d)));
                var P = ((P2o - P1o) / (Q2o - Q1o)) * (Q - Q1o) + P1o;

                QPromedio = (float)Math.Round(Q);
                PPromedio = (float)P;
            }

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

            foreach (var a in grupos.Result)
            {
                GroupsProducts.Add(a);
            }
            //GroupsProducts = new ObservableCollection<GroupsProductModel>(grupos.Result);
        }
        private void CalculateCost()
        {
            double ManufacturingCosto = Convert.ToDouble(RawMaterialT) + WorkForceT + OtherCostT; //Costo de fabricación
            double ManufacturingUnitCosto = ManufacturingCosto / UnitPS;  //Costo unitario de fabricación
            double UnitSalePrice = ManufacturingUnitCosto + (ManufacturingUnitCosto * (UtilityPS / 100));
            PricePS = UnitSalePrice;
        }
        private void RegistrationPS()
        {
            Shell.Current.GoToAsync("StockRegistration");
        }
        private void CancelPS()
        {
            NamePS = string.Empty;
            DescripcionPS = string.Empty;
            PricePS = 0;
            UnidaddMedidaPS = string.Empty;
            CostoUnitarioPS = 0;
            UnitPS = 0;
            CostoUnitarioPS = 0;

            //PricePS = string.Empty;

            IdListRM = 0;
            IdListWF = 0;
            IdListOC = 0;
        }
        private void SavePS()
        {
            if (!string.IsNullOrEmpty(NamePS) && PricePS > 0)
            {
                //Si el producto viene de costos producción
                if(ObjModify != null && ObjModify.EstadoProducto == null)
                {
                    //Actualizo valores ingresados
                    ObjModify.AmountProduct = UnitPS;
                    ObjModify.NameProduct = NamePS;
                    if(ObjModify.DateTime == null)
                    {
                        ObjModify.DateTime = DateTime.Now;
                    }
                    ObjModify.DescriptionProduct = DescripcionPS;
                    ObjModify.UnidadMedida = UnidaddMedidaPS;
                    ObjModify.PrecioVentaProduct = PricePS;
                    ObjModify.CostElaboracionProduct = CostoUnitarioPS;

                    ObjModify.EstadoProducto = "ordenProduccion";

                    var resp = App.Database.SaveProduct(ObjModify);
                    resp.Wait();

                    ObjModify.Groups = GroupProd;
                    App.Database.UpdateRelationsProduct(ObjModify);

                    //Creación de Kardex y confirmación de creación
                    if (resp != null)
                    {
                        CrearKardex(ObjModify);
                        Shell.Current.DisplayAlert("Éxito", "Se guardo " + ObjModify.NameProduct, "ok");
                        //Limpio todo
                        ObjModify = new ProductModel();
                        CancelPS();
                        //Regreso a productos
                        Shell.Current.GoToAsync($"//Rini/Productos?Regreso=true");
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Error", "No se ha podido guardar", "ok");
                    }
                }
                //Si el objeto viene para ser modificado
                else if(ObjModify != null && ObjModify.EstadoProducto != null)
                {
                    //Actualizo valores ingresados
                    ObjModify.AmountProduct = UnitPS;
                    ObjModify.NameProduct = NamePS;
                    if (ObjModify.DateTime == null)
                    {
                        ObjModify.DateTime = DateTime.Now;
                    }
                    ObjModify.DescriptionProduct = DescripcionPS;
                    ObjModify.UnidadMedida = UnidaddMedidaPS;
                    ObjModify.PrecioVentaProduct = PricePS;
                    ObjModify.CostElaboracionProduct = CostoUnitarioPS;

                    var resp = App.Database.SaveProduct(ObjModify);
                    resp.Wait();

                    ObjModify.Groups = GroupProd;
                    App.Database.UpdateRelationsProduct(ObjModify);
                    //Creación de Kardex y confirmación de creaaión
                    if (resp != null)
                    {
                        Shell.Current.DisplayAlert("Éxito", "Se guardo " + ObjModify.NameProduct, "ok");
                        //Limpio todo
                        ObjModify = new ProductModel();
                        CancelPS();
                        //Regreso a productos
                        Shell.Current.GoToAsync($"//Rini/Productos?Regreso=true");
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Error", "No se ha podido guardar", "ok");
                    }
                } 
                //Si se crea un producto sin calcular costos produccion
                else
                {
                    //Creo nuevo objeto
                    ProductModel product = new ProductModel()
                    {
                        Id = Id,
                        NameProduct = NamePS,
                        AmountProduct = UnitPS,
                        DateTime = DateTime.Now,
                        DescriptionProduct = DescripcionPS,
                        UnidadMedida = UnidaddMedidaPS,
                        PrecioVentaProduct = PricePS,
                        CostElaboracionProduct = CostoUnitarioPS
                    };

                    product.EstadoProducto = "Terminado";

                    var resp = App.Database.SaveProduct(product);
                    resp.Wait();

                    product.Groups = GroupProd;
                    //Actualizo relación de productos
                    App.Database.UpdateRelationsProduct(product);

                    //Creación de Kardex y confirmación de creación
                    if (resp != null)
                    {
                        CrearKardex(product);
                        Shell.Current.DisplayAlert("Éxito", "Se guardo " + product.NameProduct, "ok");
                        //Limpio todo
                        product.Id = 0;
                        CancelPS();
                        //Regreso a productos
                        Shell.Current.GoToAsync($"//Rini/Productos?Regreso=true");
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Error", "No se ha podido guardar", "ok");
                    }
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Ingrese el nombre del producto y el precio de venta", "ok");
            }
        }

        private void CrearKardex(ProductModel product)
        {
            //Creo Kardex el kardex contiene los ultimos valores
            KardexModel kardex = new KardexModel
            {
                Date = DateTime.Now,
                Valor = product.PrecioVentaProduct * product.AmountProduct,
                Cantidad = product.AmountProduct,
                ValorPromPond = product.PrecioVentaProduct
            };

            var kf = App.Database.SaveMovKardex(kardex);
            kf.Wait();

            kardex.ProductModel = product;
            App.Database.UpdateRelationKardexProduct(kardex);

            product.Kardices = kardex;
            App.Database.UpdateRelationsProduct(product);
            GuardarSaldoInicial(kardex);
            //var resp1 = App.Database.GetAllKardxProduct().Result;
            Shell.Current.DisplayAlert("Exito", "Se creo kardex", "ok");            
        }

        private void GuardarSaldoInicial(KardexModel kardex)
        {
            SaldosKardexProductModel saldosKardex = new SaldosKardexProductModel
            {
                Date = DateTime.Now,
                Cantidad = kardex.Cantidad,
                ValorUnitario = kardex.ValorPromPond,
                SaldoTotal = kardex.Valor,
                NombreReconocimiento = "Estado inicial"
            };

            App.Database.SaveSaldoKardxPr(saldosKardex);

            saldosKardex.KardexProductModel = kardex;
            App.Database.UpdateRelationsSaldosKrdxProd(saldosKardex);

            kardex.SaldosProducts = new List<SaldosKardexProductModel> { saldosKardex };
            App.Database.UpdateRelationKardexProduct(kardex);


        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            /*string idListWF = HttpUtility.UrlDecode(query["IdListWF"]);    //Recepcion del Id de lista de otros costos            
            string idListOC = HttpUtility.UrlDecode(query["IdListCI"]);    //Recepcion del Id de lista de materiales            
            string idListRM = HttpUtility.UrlDecode(query["IdListRM"]); //Recepción Id de lista de fuerza de trabajo
            */
            string idProduct = HttpUtility.UrlDecode(query["IdObjetoconPrecio"]); //Recepción Id del producto para modificar

            //Enrutar(idListOC, idListRM, idListWF, idProduct);            
            Enrutar(idProduct);
        }
        private void Enrutar(string idProduct)
        {
            try
            {
                var idProductconPrecio = int.Parse(idProduct);

                if (idProductconPrecio != 0)
                {
                    LoadProductS(idProductconPrecio);      //carga producto a modificar
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
            if (int.Parse(idListOC) > 0)
            {
                ListOCModel listOC = await App.Database.GetListOC(int.Parse(idListOC));
                OtherCostT = listOC.OtherCostsxProduct.Sum(x => x.CostOC);
            }

            if (int.Parse(idListWF) > 0)
            {
                WorkForceT = 0;
                ListWFModel l = await App.Database.GetListWF(int.Parse(idListWF));
                WorkForceT = l.PersonalxProduct.Sum(x => x.Pay);
            }

            if (int.Parse(idListRM) > 0)
            {
                RawMaterialT = 0;
                ListRMModel li = await App.Database.GetListRM(int.Parse(idListRM));
                RawMaterialT = li.ListMaterialxProduct.Sum(x => x.TotalCost);
            }

            CostoUnitarioPS = (OtherCostT + WorkForceT + RawMaterialT) / UnitPS;
        }
        private async void LoadProductS(int idProd)
        {
            if (idProd != 0)
            {
                ObjModify = await App.Database.Get1Product(idProd);
                Show(ObjModify);
            }
        }
        private void Show(ProductModel product)
        {
            Id = product.Id;
            NamePS = product.NameProduct;
            DescripcionPS = product.DescriptionProduct;
            PricePS = product.PrecioVentaProduct;
            UnidaddMedidaPS = product.UnidadMedida;
            CostoUnitarioPS = Math.Round(product.CostElaboracionProduct, 2);
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

        private void InitDataChart()
        {
            
            //Oferta
            List<ChartEntry> Linea1 = new List<ChartEntry>();
            foreach (var punto in PuntosLinea1)
            {
                ChartEntry entry = new ChartEntry(punto)
                {
                    Color = blueColor,
                    //ValueLabel = $"{punto.ToString()}", 
                    //Label = "Cantidad"
                };

                Linea1.Add(entry);
            }

            List<ChartEntry> Linea2 = new List<ChartEntry>();
            foreach (var punto in PuntosLinea2)
            {
                ChartEntry entry = new ChartEntry(punto)
                {
                    Color = redColor,
                    //ValueLabel = i.ToString(), 
                    //Label = i.ToString()
                };

                Linea2.Add(entry);
            }
            Oferta = new LineChart { Entries = Linea1,
                LabelTextSize = 22,
                LabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty,
                LineMode = LineMode.Straight,
                LineAreaAlpha = 0
            };
            Demanda = new LineChart { Entries = Linea2,
                LabelTextSize = 22,
                LabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty,
                LabelColor = SKColor.Empty,
                LineMode = LineMode.Straight,
                LineAreaAlpha = 0
            };
            //Demanda = new LineChart
            /*foreach (var punto in PuntosLinea1)
            {
                ChartEntry entry = new ChartEntry(punto)
                {
                    Color = blueColor,
                    //ValueLabel = i.ToString(), 
                    //Label = i.ToString()
                };

                //Linea1.Add(entry);
                i++;
            }
            Max1 = PuntosLinea1.Max();
            Min1 = PuntosLinea1.Min();
            i = 0;
            foreach (var punto in PuntosLinea2)
            {
                ChartEntry entry = new ChartEntry(punto)
                {
                    Color = redColor,
                    //ValueLabel = i.ToString(),
                    //Label = ""
                    //Label = i.ToString()
                };
                Linea2.Add(entry);
                i++;
            } 
            Max2 = PuntosLinea2.Max();
            Min2 = PuntosLinea2.Min();
            */
            //entries.Add(Linea2);

            /*Oferta = new LineChart { Entries = Linea1, 
                LabelTextSize = 22, 
                LabelOrientation = Orientation.Horizontal, 
                BackgroundColor = SKColor.Empty,
                LineMode = LineMode.Straight
            };*/
            /*Demanda = new LineChart { Entries = Linea2, 
                LabelTextSize = 22, 
                LabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty, 
                LabelColor = SKColor.Empty,
                LineMode = LineMode.Straight
            };*/

            #region MyRegion
            /*Multines = new MultinesChartAuxiliar
                {
                    MultiLineEntries = entries,
                    LabelTextSize = 9f,
                    LabelOrientation = Orientation.Horizontal,
                    LineAreaAlpha = 30,
                    PointAreaAlpha = 60,
                    LegendNames = new List<string> { "Oferta", "Demanda" },
                    IsAnimated = false,
                    LineMode = LineMode.Straight
                };*/
            #endregion
        }

        private void CalcularOferta()
        {

            double m = 0, intery = 0, interx1 = 0, tang = 0, ang = 0, xMax;
            double P1x = 0, Q1y = 0, P2x, Q2y, Y, x, yy, yy2;

            try
            {
                //P1x = CostoUnitarioPS;
                P1x = PricePS;
                Q1y = UnitPS;
                P2x = Precio2Oferta;
                Q2y = Cantidad2Oferta;

                Y = 1;

                if (P1x < P2x)
                {
                    Max1 = (float)P2x;
                }
                else
                {
                    Max1 = (float)P1x;
                }
                if (Q1y < Q2y)
                {
                    xMax = (float)Q2y;
                }
                else
                {
                    xMax = (float)Q1y;
                }

                double P2P1x = P2x - P1x;
                double Q2Q1y = Q2y - Q1y;

                //Formula para la pendiente
                if(P2P1x == 0 || Q2Q1y == 0)
                {
                    m = 0;
                }
                else
                {
                    m = P2P1x / Q2Q1y;
                }

                //Formula para sacar a ecuación general
                yy = m * (P1x * -1);
                yy2 = yy + ((Q1y * -1) * -1);

                double A, B, C, ix, iy;
                //condicion si x es negativo
                if (m < 0)
                {
                    x = m * -1;
                    A = x;

                    Y = 1;
                    B = Y;

                    yy2 = yy2 * -1;
                    C = yy2;

                    interx1 = (yy2 * -1) / Math.Abs(m);
                    ix = interx1;

                    intery = yy2 * -1;
                    iy = intery;
                }
                else
                {
                    A = m;

                    Y = -1;
                    B = Y;

                    C = yy2;

                    interx1 = (yy2 * -1) / Math.Abs(m);
                    ix = interx1;

                    iy = yy2;
                }

                tang = (Math.Atan(m) * 180) / Math.PI;

                if (tang < 0)
                {
                    ang = tang + 180;
                    ang = tang;
                } else
                {
                    var tg = tang;
                }

                //Consiguiendo puntos
                double punto1x, punto1y, punto2x, punto2y;

                //Resolviendo ecuación cuando x tiende a -1000
                punto1x = (((A * 0) + C) * -1);
                punto1y = (punto1x / B);
                PuntosLinea1.Add((float)punto1y);
               

                //Resolviendo ecuación cuando x tiende a 1000
                punto2x = (((A * 300) + C) * -1);
                punto2y = punto2x / B;
                PuntosLinea1.Add((float)punto2y);
                
            }
            catch
            {
                // OverflowException e;
                Debug.WriteLine("Exception: " );
            }
            #region MyRegion
            /*double P2P1 = P2 - P1;
                double Q2Q1 = Q2 - Q1;

                double PartA = P2P1 / Q2Q1;
                double PartB = PartA * Q1;

                double PartC = PartB - P1;*/
            /*
            double CantidadMaxima;
            //double CantidadMinima;
            
            if (Q2 > Q1)
            {
                CantidadMaxima = Q2;
            }
            else
            {
                CantidadMaxima = Q1;
            }

            for (int i = 0; i <= CantidadMaxima; i++)
            {
                /*var Py = (((P2 - P1) / (Q2 - Q1)) * (i - Q1)) + P1;
                PuntosLinea1.Add((float)Py);/
                if(i == Q1)
                {
                    PuntosLinea1.Insert((int)Q1, (float)P1);
                }
                if(i == Q2)
                {
                    PuntosLinea1.Insert((int)Q2, (float)P2);
                }
            }*/
            #endregion

        }
        private void OfertaCalculo()
        {
            
            float P1x, P2x;
            int Q1y, Q2y;

            P1x = (float)CostoUnitarioPS;
            Q1y = UnitPS;
            P2x = (float)Precio2Oferta;
            Q2y = Cantidad2Oferta;

            float b = 0;
            float m = 0;
            int inicio = 0;
            float final = 100;
            float incremento = 3;
            if(P1x != 0 && P2x != 0 && Q1y != 0 && Q2y != 0)
            {
                m = (P2x - P1x) / (Q2y - Q1y);

                for (float x = inicio; x <= final; x = incremento + x)
                {
                    double y = m * x + b;
                    if (x == inicio)
                    {
                        //int xx = Convert.ToInt32(x);
                        PuntosLinea1.Add((float)y);
                    } else if (x == final)
                    {
                        PuntosLinea1.Add((float)y);
                    }
                }
            }
            else
            {
                m = 0;
            }            

            
        }
        private void CalcularDemanda()
        {
            double m = 0, intery = 0, interx1 = 0, tang = 0, ang = 0, xMax;
            double P1x = 0, Q1y = 0, P2x, Q2y, Y, x, yy, yy2;

            try
            {
                P1x = Precio2Demanda;
                Q1y = Cantidad2Demanda;
                P2x = Precio1Demanda;
                Q2y = Cantidad1Demanda;

                Y = 1;

                if (P1x < P2x)
                {
                    Max2 = (float)P2x;
                }
                else
                {
                    Max2 = (float)P1x;
                }
                if (Q1y < Q2y)
                {
                    xMax = (float)Q2y;
                }
                else
                {
                    xMax = (float)Q1y;
                }
                double P2P1x = P2x - P1x;
                double Q2Q1y = Q2y - Q1y;

                //Formula para la pendiente
                if (P2P1x == 0 || Q2Q1y == 0)
                {
                    m = 0;
                }
                else
                {
                    m = P2P1x / Q2Q1y;
                }

                //Formula para sacar a ecuación general
                yy = m * (P1x * -1);
                yy2 = yy + ((Q1y * -1) * -1);

                double A, B, C, ix, iy;
                //condicion si x es negativo
                if (m < 0)
                {
                    x = m * -1;
                    A = x;

                    Y = 1;
                    B = Y;

                    yy2 = yy2 * -1;
                    C = yy2;

                    interx1 = (yy2 * -1) / Math.Abs(m);
                    ix = interx1;

                    intery = yy2 * -1;
                    iy = intery;
                }
                else
                {
                    A = m;

                    Y = -1;
                    B = Y;

                    C = yy2;

                    interx1 = (yy2 * -1) / Math.Abs(m);
                    ix = interx1;

                    iy = yy2;
                }

                tang = (Math.Atan(m) * 180) / Math.PI;

                if (tang < 0)
                {
                    ang = tang + 180;
                    ang = tang;
                }

                //Consiguiendo puntos
                double punto1x, punto1y, punto2x, punto2y;

                //Resolviendo ecuación cuando x tiende a -1000
                punto1x = ((A * 0) + C) * -1;
                punto1y = punto1x / B;
                PuntosLinea2.Add((float)punto1y);


                //Resolviendo ecuación cuando x tiende a 1000
                punto2x = ((A * 300) + C) * -1;
                punto2y = punto2x / B;
                PuntosLinea2.Add((float)punto2y);
            }
            catch
            {
                //OverflowException e;
                Debug.WriteLine("Exception: ");
            }
            #region MyRegion
            //double CantidadMaxima;

            /*if (Q2 > Q1)
            {
                CantidadMaxima = Q2;
            }
            else
            {
                CantidadMaxima = Q1;
            }*/

            //Diferencias




            /*for (double x = 0; x <= CantidadMaxima; x += P2P1)
            {
                for (double y = 0; y <= CantidadMaxima; y += Q2Q1)
                {
                    double yp = 
                }
            }*/
            //Formula para sacar la ecuacion general
            //double yy, yy2, X, Y, tang, ang;
            //double A, B, C, interx1, xp, intery, yp;
            //pen = (P2-P1)/ (Q2-Q1); 
            //yy = pen * (P1 * -1);
            //yy2 = yy + ((Q1 * -1) * -1);



            //tang = ((Math.Atan(pen)) * 180) / Math.PI; //Angulo tangente
            /*
            if (tang < 0)
            {
                ang = tang + 180;
                var angulo = ang;
            /}
            else
            {
                var angulo = tang;
            }
            */
            //Resolucion ecuacion general
            //var pnd = (-1 * (A / B)); //resolver pendiente
            //var textpendie = pen;
            //double ang2;
            //var tang2 = ((Math.Atan(pnd)) * 180) / Math.PI;
            /*if (tang2 < 0)
            {
                ang2 = tang2 + 180;
            }
            *//*********************************************/
            #endregion

        }
        private void CalcularPrecioVenta()
        {
            var cal = CostoUnitarioPS + (CostoUnitarioPS * (porcntGanacia / 100));
            if (cal > 0)
            {
                PricePS = cal;
            }
            else
            {
                PricePS = 0;
            }
        }
        private void CalcularPorcentajGanacia()
        {
            var cal = ((PricePS - CostoUnitarioPS) / CostoUnitarioPS) * 100;
            if (cal > 0)
            {
                porcntGanacia = cal;
            }
            else
            {
                porcntGanacia = 0;
            }
        }
    }
}
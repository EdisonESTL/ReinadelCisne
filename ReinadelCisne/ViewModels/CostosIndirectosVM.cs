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
    public class CostosIndirectosVM : BaseVM, IQueryAttributable
    {
        private OtherCostModel _costIndirect;
        private double _totalMOI;
        private double _totalWFI;
        private double _precioProduct;
        private string _nameProdj;
        private int _cantProdj;

        ListRMModel _listaMaterialxProducto;
        public ListRMModel ListaMaterialxProducto
        {
            get => _listaMaterialxProducto;
            set
            {
                if(value != _listaMaterialxProducto)
                {
                    _listaMaterialxProducto = value;
                    OnPropertyChanged();
                }
            }
        }

        ListWFModel _listaPersonalxProducto;
        public ListWFModel ListaPersonalxProducto
        {
            get => _listaPersonalxProducto;
            set
            {
                if (value != _listaPersonalxProducto)
                {
                    _listaPersonalxProducto = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NameProd
        {
            get => _nameProdj;
            set
            {
                _nameProdj = value;
                OnPropertyChanged();
            }
        }
        public int CantProd
        {
            get => _cantProdj;
            set
            {
                _cantProdj = value;
                OnPropertyChanged();
            }
        }
        private bool _verMateriaPrima = true;
        public bool VerMateriaPrima
        {
            get => _verMateriaPrima;
            set
            {
                _verMateriaPrima = value;
                OnPropertyChanged();
            }
        }

        private bool _verManoObra = false;
        public bool VerManoObra
        {
            get => _verManoObra;
            set
            {
                _verManoObra = value;
                OnPropertyChanged();
            }
        }

        private bool _verCostosIndirectos = false;
        public bool VerCostosIndirectos
        {
            get => _verCostosIndirectos;
            set
            {
                _verCostosIndirectos = value;
                OnPropertyChanged();
            }
        }

        private double _totalWF;
        public double TotalWF
        {
            get => _totalWF;
            set
            {
                _totalWF = value;
                OnPropertyChanged();
            }
        }

        private float _totalRM;
        public float TotalRM
        {
            get => _totalRM;
            set
            {
                _totalRM = value;
                OnPropertyChanged();
            }
        }

        private float _totalRMI;
        public float TotalRMI
        {
            get => _totalRMI;
            set
            {
                _totalRMI = value;
                OnPropertyChanged();
            }
        }
        private double _totalCI;
        public double TotalCI
        {
            get => _totalCI;
            set
            {
                _totalCI = value;
                OnPropertyChanged();
            }
        }
        public double TotalMOI
        {
            get => _totalMOI;
            set
            {
                _totalMOI = value;
                OnPropertyChanged();
            }
        }
        public double TotalWFI
        {
            get => _totalWFI;
            set
            {
                _totalWFI = value;
                OnPropertyChanged();
            }
        }

        public double TotalMaquinaria
        {
            get => _totalMaquinaria;
            set
            {
                if(value != _totalMaquinaria)
                {
                    _totalMaquinaria = value;
                    OnPropertyChanged();
                }
                
            }
        }

        private int IdListRM { get; set; }
        private int IdListWF { get; set; }
        private int IdListCI { get; set; }
        private WorkForceModel _workForce;
        private double _totalMaquinaria;

        public WorkForceModel WorkForce
        {
            get => _workForce;
            set
            {
                _workForce = value;
                OnPropertyChanged();
            }
        }

        public OtherCostModel CostIndirect
        {
            get => _costIndirect;
            set
            {
                _costIndirect = value;
                OnPropertyChanged();
            }
        }

        public double PrecioProduct
        {
            get => _precioProduct;
            set
            {
                if (value != _precioProduct)
                {
                    _precioProduct = value;
                    OnPropertyChanged();
                }
            }
        }


        public ObservableCollection<RawMaterialModel> RawMaterials { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ObservableCollection<OtherCostModel> OtherCosts { get; set; } = new ObservableCollection<OtherCostModel>();

        //public List<AuxiliarRMxCP> MaterialProduccion { get; set; } = new List<AuxiliarRMxCP>();
        public ObservableCollection<AuxiliarRMxCP> MaterialProduccion { get; set; } = new ObservableCollection<AuxiliarRMxCP>();
        public ObservableCollection<AuxiliarRMxCP> MaterialProduccionIndirecto { get; set; } = new ObservableCollection<AuxiliarRMxCP>();
        public ObservableCollection<PersonalModel> WorkForces { get; set; } = new ObservableCollection<PersonalModel>();
        public ObservableCollection<PersonalModel> WorkForcesIndiecto { get; set; } = new ObservableCollection<PersonalModel>();

        public ICommand PushCommand => new Command((obj) =>
        {
            var push = obj as string;
            Direccionar(push);
        });
        public ICommand AddRM => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/Productos/NewStock/CostoProduccion/SelectRM");
        });
        public ICommand AddWF => new Command(() =>
        {
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion/SelectWF?ModWfId=0");
        });
        public ICommand AddCI => new Command(() =>
        {
            if (!string.IsNullOrEmpty(CostIndirect.DescriptionOC) && CostIndirect.CostOC > 0)
            {
                OtherCosts.Add(CostIndirect);

                SumarCI();
            }
            else
            {
                Shell.Current.DisplayAlert("Campos vacios", "", "ok");
            }
        });
        public ICommand DeleteWFCommand => new Command((obj) =>
        {
            var rec = obj as PersonalModel;
            TotalWF = TotalWF - rec.Pay;
            WorkForces.Remove(rec);
            App.Database.DeletePersonal(rec);
        });
        public ICommand ModifyWFCommand => new Command((obj) =>
        {
            var rec = obj as PersonalModel;
            WorkForces.Remove(rec);
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion/SelectWF?ModWfId={rec.Id}");
        });
        public ICommand DeleteCICommand => new Command((obj) =>
        {
            var rec = obj as OtherCostModel;
            TotalCI = TotalCI - rec.CostOC;
            OtherCosts.Remove(rec);
        });
        public ICommand ModifyCICommand => new Command((obj) =>
        {
            var rec = obj as OtherCostModel;
            CostIndirect = rec;
            OtherCosts.Remove(rec);
        });
        public ICommand BackCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"//Rini/Productos/NewStock?IdObjetoconPrecio=0");
        });
        private void ReducirMaterial(RawMaterialModel rmRecibo, string cantUso)
        {
            double total = rmRecibo.CantidadRM - Convert.ToDouble(cantUso);
            rmRecibo.CantidadRM = total;
            RawMaterialModel auxrm = rmRecibo;
            RawMaterials.Remove(rmRecibo);
            RawMaterials.Add(auxrm);
            auxrm = null;
        }
        private void SumarRM()
        {
            float totalRM = MaterialProduccion.Sum(x => x.Total);
            TotalRM = totalRM;
        }
        private void SumarCI()
        {
            TotalWFI = WorkForcesIndiecto.Sum(x => x.Pay);
            TotalMOI = MaterialProduccionIndirecto.Sum(x => x.Total);
            TotalCI = OtherCosts.Sum(x => x.CostOC) + TotalWFI + TotalMOI;
            LimpiarCI();
        }
        private void LimpiarCI()
        {
            CostIndirect = null;
            CostIndirect = new OtherCostModel();
        }
        private void SumarWF()
        {
            var totalWF = WorkForces.Sum(x => x.Pay);
            TotalWF = totalWF;
            LimpiarWF();
        }
        private void Direccionar(string push)
        {
            switch (push)
            {
                case "VerMT":
                    Shell.Current.GoToAsync("SelectRM");
                    break;
                case "VerWF":
                    Shell.Current.GoToAsync($"SelectWF?ModWfId=0");
                    break;
                case "VerCI":
                    Shell.Current.GoToAsync("SelectCIFView");
                    break;
                case "VerMaqui":
                    Shell.Current.GoToAsync($"SelectMaquinariaView?CantProduc={CantProd}");
                    break;
                case "Borrar":
                    LimpiarWF();
                    break;
                case "BorrarCI":
                    LimpiarCI();
                    break;
                case "Cancel":
                    MaterialProduccion.Clear();
                    LimpiarWF();
                    LimpiarCI();
                    Shell.Current.GoToAsync("..");
                    break;
                case "GuardarTodo":
                    GuardarT();
                    break;
                default:
                    break;
            }
        }

        ProductModel _productoconPrecio;
        public ProductModel ProductoconPrecio
        {
            get => _productoconPrecio;
            set
            {
                if(value != _productoconPrecio)
                {
                    _productoconPrecio = value;
                    OnPropertyChanged();
                }
            }
        } 
        private void GuardarT()
        {
            /*GuardarRawMaterial();
            GuardarWorkForce();
            GuardarCostosInd();*/
            if (PrecioProduct > 0)
            {
                //Lleno un producto con los datos y lo guardo
                ProductoconPrecio = new ProductModel()
                {
                    NameProduct = NameProd,
                    AmountProduct = CantProd,                    
                    CostElaboracionProduct = PrecioProduct
                };
                var guardado = App.Database.SaveProduct(ProductoconPrecio);
                guardado.Wait();

                ProductoconPrecio.ListRMModel = ListaMaterialxProducto;
                ProductoconPrecio.ListWFModel = ListaPersonalxProducto;
                App.Database.UpdateRelationsRM(ProductoconPrecio);

                Shell.Current.DisplayAlert("Costos Guardados", "Valores guardados correctamente", "ok");
                Shell.Current.GoToAsync($"//Rini/Productos/NewStock?IdObjetoconPrecio={ProductoconPrecio.Id}");
            }          

        }
        private void GuardarCostosInd()
        {
            ListOCModel listOCModel = new ListOCModel()
            {
                Total = (float)TotalCI
            };
            var re = App.Database.SaveListOC(listOCModel);
            re.Wait();
            foreach (var c in OtherCosts)
            {
                App.Database.SaveOtherCost(c);
                c.ListOCModel = listOCModel;
                App.Database.UpdateRelationOC(c);
            }
            IdListCI = listOCModel.Id;
        }
        private void GuardarWorkForce()
        {
            ListWFModel listWFModel = new ListWFModel()
            {
                Total = (float)TotalWF
            };

            var gr = App.Database.SaveListWF(listWFModel);
            gr.Wait();

            foreach (var w in WorkForces)
            {
                w.ListWF = listWFModel;
                App.Database.UpdateRelationsPersonal(w);
            }

            foreach (var w in WorkForcesIndiecto)
            {
                w.ListWF = listWFModel;
                App.Database.UpdateRelationsPersonal(w);
            }

            IdListWF = listWFModel.Id;

        }
        private void GuardarRawMaterial()
        {
            ListRMModel ListRM = new ListRMModel()
            {
                Total = TotalRM
            };

            var rmm = App.Database.SaveListRM(ListRM);
            rmm.Wait();

            foreach (var a in MaterialProduccion)
            {
                var kr = App.Database.GetKardexXRM(a.RawMaterial).Result;

                ItemsListRMModel itemsListRM = new ItemsListRMModel()
                {
                    Amount = a.CantUsar,
                    UnitCost = a.RawMaterial.CostoRM,
                    TotalCost = a.Total
                };

                itemsListRM.ListRMModel = ListRM;
                itemsListRM.KardexRMModel = kr;

                App.Database.UpdateRelationItemRM(itemsListRM);

            }
            IdListRM = ListRM.Id;
        }
        private void LimpiarWF()
        {
            WorkForce = null;
            WorkForce = new WorkForceModel();
        }
        private void ListarRM()
        {
            RawMaterials.Clear();

            var resp = App.Database.GetKardexsRM().Result;

            if (resp.Count > 0)
            {
                foreach (var obj in resp)
                {
                    if (obj.SaldosRMs.Count > 0 && obj.RawMaterialModell != null)
                    {
                        var respi = App.Database.GetSaldosxKardex(obj).Result;
                        var ord = respi.OrderByDescending(x => x.Date).First();

                        RawMaterialModel rrm = App.Database.GetOneRM(obj.RawMaterialModell.Id).Result;
                        rrm.CantidadRM = ord.Cantidad;
                        rrm.CostoRM = (float)ord.ValorUnitario;
                        rrm.TotalCost = (float)ord.SaldoTotal;

                        RawMaterials.Add(rrm);
                    }
                }
            }

        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string idRM = HttpUtility.UrlDecode(query["ListMaterialId"]);
            string idWF = HttpUtility.UrlDecode(query["IdWF"]);
            string idMaquinaria = HttpUtility.UrlDecode(query["IdListaMaquinaria"]);
            string CantidadProd = HttpUtility.UrlDecode(query["CantProd"]);
            string NameProd = HttpUtility.UrlDecode(query["NameProd"]);
            AddRawMaterial(idRM);
            AddWorkForce(idWF);
            AddMaquinaria(idMaquinaria);
            AddCostosIndirectos();
            FillDate(NameProd, CantidadProd);
        }

        private void AddCostosIndirectos()
        {
            try
            {
                var Costosindirectos = App.Database.GetAllOtherCost().Result;
                var totalcostosInd = Costosindirectos.Sum(x => x.CostOC);
                TotalCI = totalcostosInd + TotalWFI + TotalRMI;
                CalculoPrecioProducto();

            }
            catch
            {
                Console.WriteLine("Fallo en llenado de datos");
            }
        }

        private void AddMaquinaria(string idMaquinaria)
        {
            try
            {
                var IdListaMaquinaria = int.Parse(idMaquinaria);
                if(IdListaMaquinaria > 0)
                {
                    var maqui = App.Database.Get1ListFA(IdListaMaquinaria).Result;
                    TotalMaquinaria = maqui.ValorTotalMaquinas + maqui.ValorTotalDepreciaciones;
                    CalculoPrecioProducto();
                }
            }
            catch
            {
                Console.WriteLine("Fallo en llenado de datos");
            }
        }

        private void FillDate(string nameProd, string cantidadProd)
        {
            try
            {
                if(!string.IsNullOrEmpty(nameProd) && !string.IsNullOrEmpty(cantidadProd))
                {
                    NameProd = nameProd;
                    CantProd = int.Parse(cantidadProd);
                    CalculoPrecioProducto();
                }                
            }
            catch
            {
                Console.WriteLine("Fallo en llenado de datos");
            }
        }

        private void AddWorkForce(string idWF)
        {
            try
            {
                var IdListPersonalProducto = int.Parse(idWF);
                if (IdListPersonalProducto > 0)
                {

                    ListaPersonalxProducto = App.Database.GetListWF(IdListPersonalProducto).Result;
                    var ManoObra = ListaPersonalxProducto.PersonalxProduct;

                    foreach(var mo in ManoObra)
                    {
                        var mm = App.Database.GetPersonal(mo.Id).Result;

                        if(mm.WorkForce.Type == "Directo")
                        {
                            TotalWF = TotalWF + mm.Pay;
                            CalculoPrecioProducto();
                        }
                        if (mm.WorkForce.Type == "Indirecto")
                        {
                            TotalWFI = TotalWFI + mm.Pay;
                            CalculoPrecioProducto();
                        }
                    }                    

                    //foreach()

                    /*if(rm.WorkForce.Type == "Directo")
                    {
                        WorkForces.Add(rm);
                        SumarWF();
                    }
                    else
                    {
                        WorkForcesIndiecto.Add(rm);
                        SumarCI();
                    } */
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to AddWorkForce");
            }
        }

        private async void AddRawMaterial(string idRM)
        {
            try
            {
                if (idRM != "0")
                {
                    double matDir = 0;
                    double matIndir = 0;
                    ListRMModel ListaMaterialProducto = await App.Database.GetListRM(int.Parse(idRM));
                    List<ItemsListRMModel> Material = await App.Database.GetItemsListRMxListRm(ListaMaterialProducto);
                                        
                    foreach(ItemsListRMModel material in Material)
                    {
                        //var Mate = material.KardexRMModel.RawMaterialModell;
                        var Mate = App.Database.GetMaterialxKardex(material.KardexRMModel).Result;
                        var tipoMaterial = Mate.TypeRM;

                        if (tipoMaterial == "Directo")
                        {
                            matDir = material.TotalCost + matDir;
                        }
                        if (tipoMaterial == "Indirecto")
                        {
                            matIndir = material.TotalCost + matIndir;
                        }
                    }
                    TotalRM = (float)matDir;
                    TotalRMI = (float)matIndir;
                    CalculoPrecioProducto();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to add raw material");
            }
        }

        private void CalculoPrecioProducto()
        {
            var CostosProductos = TotalWF + TotalRM + TotalCI + TotalMaquinaria;
            var imprevistos = CostosProductos * 0.10;
            var totalproducto = CostosProductos + imprevistos;

            PrecioProduct = totalproducto / CantProd;
        }

        public CostosIndirectosVM()
        {            
            WorkForce = new WorkForceModel();
            CostIndirect = new OtherCostModel();
            ProductoconPrecio = new ProductModel();
            CalculoPrecioProducto();
        }
    }
}

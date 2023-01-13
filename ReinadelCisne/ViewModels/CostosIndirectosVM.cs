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

        private int IdListRM { get; set; }
        private int IdListWF { get; set; }
        private int IdListCI { get; set; }
        private WorkForceModel _workForce;
        public WorkForceModel WorkForce
        {
            get => _workForce;
            set
            {
                _workForce = value;
                OnPropertyChanged();
            }
        }

        private OtherCostModel _costIndirect;
        private double _totalMOI;
        private double _totalWFI;

        public OtherCostModel CostIndirect
        {
            get => _costIndirect;
            set
            {
                _costIndirect = value;
                OnPropertyChanged();
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
                    VerMateriaPrima = true;
                    VerCostosIndirectos = false;
                    VerManoObra = false;
                    ListarRM();
                    break;
                case "VerWF":
                    VerMateriaPrima = false;
                    VerCostosIndirectos = false;
                    VerManoObra = true;
                    break;
                case "VerCI":
                    VerMateriaPrima = false;
                    VerCostosIndirectos = true;
                    VerManoObra = false;
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
        private void GuardarT()
        {
            GuardarRawMaterial();
            GuardarWorkForce();
            GuardarCostosInd();
            Shell.Current.DisplayAlert("Costos Guardados", "Valores guardados correctamente", "ok");
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock?IdListWF={IdListWF}&IdListCI={IdListCI}&IdListRM={IdListRM}&IdProduct={0}");
        }
        private void GuardarCostosInd()
        {
            ListOCModel listOCModel = new ListOCModel()
            {
                Total = (float)TotalCI
            };
            var re = App.Database.SaveListOC(listOCModel);
            re.Wait();
            foreach(var c in OtherCosts)
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

            foreach(var a in MaterialProduccion)
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
            string idRM = HttpUtility.UrlDecode(query["IdRM"]);
            string idWF = HttpUtility.UrlDecode(query["IdWF"]);
            AddRawMaterial(idRM);
            AddWorkForce(idWF);
        }

        private void AddWorkForce(string idWF)
        {
            try
            {
                if (idWF != "0")
                {
                    PersonalModel rm = App.Database.GetPersonal(int.Parse(idWF)).Result;
                    if(rm.WorkForce.Type == "Directo")
                    {
                        WorkForces.Add(rm);
                        SumarWF();
                    }
                    else
                    {
                        WorkForcesIndiecto.Add(rm);
                        SumarCI();
                    }                    
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private async void AddRawMaterial(string idRM)
        {            
            try
            {
                if(idRM != "0")
                {
                    var ident = int.Parse(idRM);
                    RawMaterialModel rm = App.Database.GetOneRM(ident).Result;

                    string resp = await Shell.Current.DisplayPromptAsync(rm.NameRM, "¿Cuanto "+ rm.UMedidaRM.Description +" va a usar?");
                    
                    if (!string.IsNullOrEmpty(resp))
                    {
                        var krd = App.Database.GetKardexXRM(rm).Result;
                        var respi = App.Database.GetSaldosxKardex(krd).Result;
                        var ord = respi.OrderByDescending(x => x.Date).FirstOrDefault();

                        rm.CantidadRM = ord.Cantidad;
                        rm.CostoRM = (float)Math.Round(ord.ValorUnitario, 2);
                        rm.TotalCost = (float)ord.SaldoTotal;

                        AuxiliarRMxCP auxiliarRs = new AuxiliarRMxCP()
                        {
                            RawMaterial = rm,
                            CantUsar = float.Parse(resp),
                            Total = rm.CostoRM * float.Parse(resp)
                        };

                        if(rm.TypeRM == "Directo")
                        {
                            MaterialProduccion.Add(auxiliarRs);
                            SumarRM();
                        }
                        else
                        {
                            MaterialProduccionIndirecto.Add(auxiliarRs);
                            SumarCI();
                        }
                        ReducirMaterial(rm, resp);
                        
                    }
                }                
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        public CostosIndirectosVM()
        {
            
            WorkForce = new WorkForceModel();
            CostIndirect = new OtherCostModel();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Collections.ObjectModel;
using System.Web;

namespace ReinadelCisne.ViewModels
{
    public class InRawMaterialVM : BaseVM, IQueryAttributable
    {
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

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }
        private string _nameRM;
        public string NameRM
        {
            get { return _nameRM; }
            set
            {
                _nameRM = value;
                OnPropertyChanged();
            }
        }

        private string _descriptionRM;
        public string DescriptionRM
        {
            get { return _descriptionRM; }
            set
            {
                _descriptionRM = value;
                OnPropertyChanged();
            }
        }

        private string _costRM;
        public string CostRM
        {
            get { return _costRM; }
            set
            {
                _costRM = value;
                OnPropertyChanged();
            }
        }
        private string _unitMeasurementRM;
        public string UnitMeasurementRM
        {
            get { return _unitMeasurementRM; }
            set
            {
                _unitMeasurementRM = value;
                OnPropertyChanged();
            }
        }        
                
        private double _amountRm ;
        public double AmountRm
        {
            get => _amountRm;
            set
            {
                _amountRm = value;
                OnPropertyChanged();
            }
        } //poner double

        private string _typeRM;
        public string TypeRM
        {
            get { return _typeRM; }
            set { _typeRM = value;
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

        private float _count = 0;
        public float Count
        {
            get { return _count; }
            set { _count = value;
                OnPropertyChanged();
            }
        }

        private int _longList = 0;
        public int LongList
        {
            get { return _longList; }
            set { _longList = value;
                OnPropertyChanged();
            }
        }

        private GroupsRMModel _groupRM;
        public GroupsRMModel GroupRM
        {
            get => _groupRM;
            set
            {
                _groupRM = value;
                OnPropertyChanged();
            }
        }

        private UMedidasRMModel _uMedidaRM;
        public UMedidasRMModel UMedidaRM
        {
            get => _uMedidaRM;
            set
            {
                _uMedidaRM = value;
                OnPropertyChanged();
            }
        }

        public string IdMOD;
        public int IdList;

        public ObservableCollection<RawMaterialModel> ListRawMl { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ObservableCollection<RawMaterialModel> RawsExist { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ObservableCollection<GroupsRMModel> GroupsRMs { get; set; } = new ObservableCollection<GroupsRMModel>();
        public ObservableCollection<UMedidasRMModel> UMedidasRMs { get; set; } = new ObservableCollection<UMedidasRMModel>();

        public ICommand goback => new Command(() =>
               {
                   Shell.Current.GoToAsync("//Rini/RMateriaPrima");
               });
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListRawMl.get

            IsRefreshing = false;
        });
        public ICommand SumarCommand => new Command(() =>
        {
            AmountRm += 1;
        });
        public ICommand RestarCommand => new Command(() =>
        {
            if (AmountRm > 0)
            {
                AmountRm -= 1;
            }
        });
        public ICommand NewGrCommand => new Command(async () =>
        {
            string result = await Shell.Current.DisplayPromptAsync("Nuevo Grupo", "Cuál es el nombre?");

            if (!string.IsNullOrEmpty(result))
            {
                var resp = App.Database.GetGroupRM().Result;
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
        public ICommand NewUMedCommand => new Command(async () =>
        {
            string result = await Shell.Current.DisplayPromptAsync("Nuevo Medida", "Cuál es el nombre?");

            if (!string.IsNullOrEmpty(result))
            {
                var resp = App.Database.GetUMedidadRM().Result;

                var coincidencias = resp.Where(x => x.Description == result).ToList();

                if(coincidencias.Count == 0)
                {
                    CrearNuevaMedida(result);
                    await Shell.Current.DisplayAlert("Éxito", "La nueva medida ha sido guardada", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Ya existe unidad de medida con ese nombre", "OK");
                }                
            }
            
        });
        public ICommand CancelRM => new Command(() =>
        {
            Limpiar();
            
        });        
        public ICommand FinishRM => new Command(() =>
        {
            string resp = ValidarCampos();
            switch(resp){
                case "todobien":
                    GuardarMP();
                    break;
                case "camposvacios":
                    Shell.Current.DisplayAlert("Losiento", "Hay campos vacios", "Ok");
                    break;
                default:
                    break;
            }            

        });

        private void GuardarMP()
        {            
            if (GroupRM != null && UMedidaRM != null)
            {
                if (int.Parse(Id) > 0)
                {
                    RawMaterialModel rawMateriall = new RawMaterialModel()
                    {
                        Id = int.Parse(Id),
                        DateTime = Date, //Fecha creación rm
                        NameRM = NameRM,
                        DescriptionRM = DescriptionRM,
                        TypeRM = TypeRM
                    };
                    var resp = App.Database.SaveRawMaterial(rawMateriall);
                    resp.Wait();

                    rawMateriall.GroupRM = GroupRM;
                    rawMateriall.UMedidaRM = UMedidaRM;

                    App.Database.UpdateRealtionRawMat(rawMateriall);

                    /*UMedidaRM.rawMaterialModels = new List<RawMaterialModel>() { rawMateriall };
                    App.Database.UpdateRelationUMedidasRM(UMedidaRM);

                    GroupRM.rawMaterialModels = new List<RawMaterialModel>() { rawMateriall };
                    App.Database.UpdateRelationGroupRM(GroupRM);*/

                    ActualizarSaldos(rawMateriall);
                }
                else
                {
                    RawMaterialModel rawMateriall = new RawMaterialModel()
                    {
                        Id = int.Parse(Id),
                        DateTime = Date, //Fecha creación rm
                        NameRM = NameRM,
                        DescriptionRM = DescriptionRM,
                        TypeRM = TypeRM
                    };
                    var resp = App.Database.SaveRawMaterial(rawMateriall);
                    resp.Wait();

                    rawMateriall.GroupRM = GroupRM;
                    rawMateriall.UMedidaRM = UMedidaRM;

                    App.Database.UpdateRealtionRawMat(rawMateriall);

                   /* UMedidaRM.rawMaterialModels = new List<RawMaterialModel>() { rawMateriall };
                    App.Database.UpdateRelationUMedidasRM(UMedidaRM);

                    GroupRM.rawMaterialModels = new List<RawMaterialModel>() { rawMateriall };
                    App.Database.UpdateRelationGroupRM(GroupRM);*/

                    CrearKardexRM(rawMateriall);
                }                
            }
            else
            {
                Shell.Current.DisplayAlert("Error, Campos vacios", "Asigne a un grupo o cree uno nuevo, y asigne", "ok");
            }
        }

        private void ActualizarSaldos(RawMaterialModel rawMateriall)
        {
            var kardx = App.Database.GetKardexXRM(rawMateriall).Result;
            var sald = App.Database.GetSaldosxKardex(kardx).Result;

            var kaardxResp = (from s in sald
                              where s.Date.Date == rawMateriall.DateTime.Date
                              select s).FirstOrDefault();

            kaardxResp.Cantidad = AmountRm;
            kaardxResp.ValorUnitario = Convert.ToDouble(CostRM);
            kaardxResp.SaldoTotal = AmountRm * Convert.ToDouble(CostRM);

            App.Database.SaveSaldoRM(kaardxResp);

            Limpiar();
            Shell.Current.DisplayAlert("Éxito", "Se guardo " + rawMateriall.NameRM + " correctamente", "ok");
            Shell.Current.GoToAsync("//Rini/RMateriaPrima");
        }

        private void CrearKardexRM(RawMaterialModel rawMaterial)
        {
            KardexRMModel kardexRM = new KardexRMModel()
            {
                Date = DateTime.Now, //Fecha reación k                
            };

            App.Database.SaveKardesxRM(kardexRM);

            kardexRM.RawMaterialModell = rawMaterial;
            kardexRM.ShoppingModell = new List<ShoppingListModel>();

            App.Database.UpdateRelationsKardexRM(kardexRM);

            GuardarSaldoInicial(kardexRM);
        }

        private void GuardarSaldoInicial(KardexRMModel kardexRM)
        {
            SaldosRMModel saldosRMi = new SaldosRMModel()
            {
                Date = DateTime.Now, //Fecha primer saldo
                Cantidad = AmountRm,
                ValorUnitario = float.Parse(CostRM),
                SaldoTotal = (float)(AmountRm * float.Parse(CostRM)),
                NombreReconocimiento = "Estado inicial"
            };

            App.Database.SaveSaldoRM(saldosRMi);

            saldosRMi.KardexRMModel = kardexRM;

            App.Database.UpdateRelationsSaldosRM(saldosRMi);

            kardexRM.SaldosRMs = new List<SaldosRMModel> { saldosRMi };
            App.Database.UpdateRelationsKardexRM(kardexRM);
            
            Shell.Current.DisplayAlert("Éxito", "Se guardo " + kardexRM.RawMaterialModell.NameRM + " correctamente", "ok");
            Limpiar();
            Shell.Current.GoToAsync("//Rini/RMateriaPrima");
        }

        private string ValidarCampos()
        {
            if(string.IsNullOrEmpty(NameRM) &&
                string.IsNullOrEmpty(DescriptionRM) &&
                string.IsNullOrEmpty(CostRM) &&
                AmountRm == 0)
            {
                return "camposvacios";
            }
            else
            {
                return "todobien";
            }
        }

        public Command<RawMaterialModel> DeleteCommand { get; set; }     
        public Command<RawMaterialModel> ModifyCommand { get; set; }     
        
        public InRawMaterialVM()
        {
            LoadGU();
            LoadUMedidas();
        }

        private void CrearNuevaMedida(string result)
        {
            UMedidasRMModel uMedidas = new UMedidasRMModel()
            {
                Description = result
            };

            App.Database.SaveUMedidaRM(uMedidas);
            LoadUMedidas();
        }

        private void LoadUMedidas()
        {
            UMedidasRMs.Clear();
            var resp = App.Database.GetUMedidadRM().Result;
            foreach(var n in resp)
            {
                UMedidasRMs.Add(n);
            }
        }

        private void CrearNuevoGrupo(string result)
        {
            
                GroupsRMModel group = new GroupsRMModel()
                {
                    Description = result
                };
                App.Database.SaveGroupRM(group);
                LoadGU();
            
        }
        private void LoadGU()
        {
            GroupsRMs.Clear();

            var resp = App.Database.GetGroupRM().Result;

            foreach(var n in resp)
            {
                GroupsRMs.Add(n);

            }

        }
                
        private void Limpiar()
        {
            Id = string.Empty;
            IdList = 0;
            GroupRM = null;
            NameRM = string.Empty;
            DescriptionRM = string.Empty;
            UMedidaRM = null;
            CostRM = string.Empty;
            AmountRm = 0;
            ListRawMl.Clear();
            Count = 0;
            LongList = 0;
            TypeRM = string.Empty;
        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string idRMmodificar = HttpUtility.UrlDecode(query["modificarRM"]);
                CargarRMmodificar(idRMmodificar);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private void CargarRMmodificar(string idRMmodificar)
        {
            RawMaterialModel rmMod = App.Database.GetOneRM(int.Parse(idRMmodificar)).Result;
            KardexRMModel kardexRM = App.Database.GetKardexXRM(rmMod).Result;
            List<SaldosRMModel> saldosRM = App.Database.GetSaldosxKardex(kardexRM).Result;
            
            var rwsp = (from a in saldosRM
                        where a.Date.Date == rmMod.DateTime.Date
                        select a).FirstOrDefault();

            Id = rmMod.Id.ToString();
            Date = rmMod.DateTime;
            NameRM = rmMod.NameRM;
            DescriptionRM = rmMod.DescriptionRM;
            TypeRM = rmMod.TypeRM;
            GroupRM = rmMod.GroupRM;
            UMedidaRM = rmMod.UMedidaRM;
            AmountRm = rwsp.Cantidad;
            CostRM = rwsp.ValorUnitario.ToString(); ;

        }
    }
}

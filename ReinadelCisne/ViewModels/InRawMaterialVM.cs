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
                   //Shell.Current.GoToAsync($"..?IdlistRM=0");
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
        /*public ICommand AddRM => new Command(() =>
        {
            if(!string.IsNullOrEmpty(NameRM) & AmountRm != 0 & CostRM != null)
            {
                RawMaterialModel rawMaterial = new RawMaterialModel()
                {
                    Id = int.Parse(Id),
                    NameRM = NameRM,
                    UnitMeasurementRM = UnitMeasurementRM,
                    CostoRM = float.Parse(CostRM),
                    AmountRM = AmountRm,
                    TypeRM = TypeRM
                };

                if (IdMOD != null)
                {
                    ListRawMl.RemoveAt(int.Parse(IdMOD));
                    ListRawMl.Insert(int.Parse(IdMOD), rawMaterial);
                    Shell.Current.DisplayAlert("Éxito", rawMaterial.NameRM + " Ha sido modificado", "Ok");
                    Count = 0;
                    foreach (var a in ListRawMl)
                    {
                        Count += (float)(a.CostoRM * a.AmountRM);
                    }

                    IdMOD = null;
                }
                else
                {
                    ListRawMl.Add(rawMaterial);
                    Shell.Current.DisplayAlert("Éxito", rawMaterial.NameRM + " Ha sido añadido", "Ok");
                    Count += (float)(rawMaterial.CostoRM * rawMaterial.AmountRM);
                }

                LongList = ListRawMl.Count;

                NameRM = string.Empty;
                UnitMeasurementRM = string.Empty;
                CostRM = string.Empty;
                AmountRm = 0;
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Ingrese la cantidad, nombre de materia prima y el precio unitario", "ok");
            }
            
        });*/
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
            /*var ghj = ListRawMl.Count;
            if (ghj != 0)
            {
                ListRMModel b = new ListRMModel
                {
                    Id = IdList,
                    Total = Count
                };
                App.Database.SaveListRM(b);

                foreach (var f in ListRawMl)
                {
                    ItemsListRMModel itemsList = new ItemsListRMModel
                    {
                        Amount = f.AmountRM,
                        UnitCost = f.CostoRM,
                        TotalCost = f.CostoRM * f.AmountRM
                    };

                    App.Database.SaveItemListRM(itemsList);

                    itemsList.ListRMModel = new ListRMModel();
                    itemsList.ListRMModel = b;

                    itemsList.RawMaterial = new RawMaterialModel();
                    itemsList.RawMaterial = f

                    App.Database.UpdateRelationItemRM(itemsList);
                }               

                var gy = App.Database.GetAllRMList();
                gy.Wait();
                var fgh = gy.Result;
                List<ItemsListRMModel> itms = App.Database.GetAllItems().Result;
                var fd = b.Id;
                Shell.Current.GoToAsync($"..?IdListOC=0&IdlistRM={b.Id}&IdlistWF=0&idProduct=0");
            }
            else
            {
                Shell.Current.GoToAsync($"..?IdListOC=0&IdlistRM=0&IdlistWF=0&idProduct=0");
            }*/

        });

        private void GuardarMP()
        {
            RawMaterialModel rawMateriall = new RawMaterialModel()
            {
                Id = int.Parse(Id),
                DateTime = DateTime.Now,
                NameRM = NameRM,
                CantidadRM = AmountRm,
                DescriptionRM = DescriptionRM,
                TypeRM = TypeRM,
                CostoRM = float.Parse(CostRM),
                TotalCost = (float)(AmountRm * float.Parse(CostRM))
            };
            rawMateriall.CantidadRM = AmountRm;
            var resp = App.Database.SaveRawMaterial(rawMateriall);

            if (GroupRM != null && UMedidaRM != null)
            {
                rawMateriall.GroupRM = GroupRM;
                rawMateriall.UMedidaRM = UMedidaRM;

                App.Database.UpdateRealtionRawMat(rawMateriall);
            }

            if (resp != null)
            {
                CrearKardexRM(rawMateriall);
                Shell.Current.DisplayAlert("Éxito", "Se guardo " + rawMateriall.NameRM, "ok");
                Limpiar();
                //Shell.Current.GoToAsync($"//Rini/Productos?Regreso=true");
            }
        }

        private void CrearKardexRM(RawMaterialModel rawMaterial)
        {
            KardexRMModel kardexRM = new KardexRMModel()
            {
                Date = DateTime.Now,
                ValorUnitario = rawMaterial.CostoRM,
                Cantidad = rawMaterial.CantidadRM,
                Valor = rawMaterial.CostoRM * rawMaterial.CantidadRM
            };

            App.Database.SaveKardesxRM(kardexRM);

            kardexRM.RawMaterialModell = rawMaterial;

            App.Database.UpdateRelationsKardexRM(kardexRM);
            Shell.Current.DisplayAlert("Exito", "Se creo kardex", "ok");
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
            //DeleteCommand = new Command<RawMaterialModel>(DeleteRM);
           //ModifyCommand = new Command<RawMaterialModel>(ModifyRM);
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


            /*GroupsRMs.Add(new GroupsRMModel()
            {
                Description = "nuevo"
            });*/
        }

        private void LoadNames()
        {
            RawsExist.Clear();
            var a = App.Database.GetMR().Result;

            foreach (var b in a)
            {
                RawsExist.Add(b);
            }
        }
        /*private void ModifyRM(RawMaterialModel obj)
        {
            IdMOD = Convert.ToString(ListRawMl.IndexOf(obj));

            Id = obj.Id.ToString();
            NameRM = obj.NameRM;
            UnitMeasurementRM = obj.UnitMeasurementRM;
            CostRM = obj.CostoRM.ToString("N2");
            AmountRm = obj.AmountRM;
        }*/
        /*private void DeleteRM(RawMaterialModel obj)
        {
            ListRawMl.Remove(obj);
            Count -= (float)(obj.CostoRM * obj.AmountRM);
            LongList = ListRawMl.Count;
            if (Convert.ToString(IdList) != null)
            {
                App.Database.DeleteRawMaterial(obj);
            }            
        }*/
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
        }
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string idListRM = HttpUtility.UrlDecode(query["IdlistRM"]);

                //LoadLS(idListRM);

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        /*private void LoadLS(string idListRM)
        {
            Count = 0;
            List<RawMaterialModel> rm = App.Database.GetMR().Result;
            List<ItemsListRMModel> itms = App.Database.GetAllItems().Result;
            var t = App.Database.GetListRM(int.Parse(idListRM)).Result;

            var its = (from i in itms
                      where i.ListRMModelId == int.Parse(idListRM)
                      select i).ToList();

            var tsr = (from it in its
                       join r in rm on it.RawMaterialModelId equals r.Id
                       select new RawMaterialModel
                       {
                           AmountRM = it.Amount,
                           NameRM = r.NameRM,
                           UnitMeasurementRM = r.UnitMeasurementRM,
                           CostoRM = (float)it.UnitCost
                       }).ToList();
            foreach(var d in tsr)
            {
                ListRawMl.Add(d);
                Count += (float)(d.CostoRM * d.AmountRM);
            }

            IdList = int.Parse(idListRM);
            LongList = t.itemsListRMModels.Count;
        }*/
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Windows.Input;
using ReinadelCisne.Models;
using ReinadelCisne.Auxiliars;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace ReinadelCisne.ViewModels
{
    public class SelectRawMaterialVM : BaseVM
    {
        #region MyRegion
        private bool _listRM;
        private bool _listRMselec;
        public bool ListRM
        {
            get => _listRM;
            set
            {
                _listRM = value;
                OnPropertyChanged();
            }
        }
        public bool ListRMselect
        {
            get => _listRMselec;
            set
            {
                _listRMselec = value;
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

        

        private List<GroupsRMModel> searchResults = App.Database.GetGroupRM().Result;
        public List<GroupsRMModel> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                searchResults = value;
                OnPropertyChanged();
            }
        }

        /*ObservableCollection<object> selectedRM;
        public ObservableCollection<object> SelectedRM
        {
            get => selectedRM;
            set
            {
                if(selectedRM != value)
                {
                    selectedRM = value;
                    OnPropertyChanged();
                }
            }
        }*/
        List<AuxiliarRMSelected> _rMSelected;

        public ObservableCollection<RawMaterialModel> RawMaterials { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ObservableCollection<RawMaterialModel> RawMaterialsCopy { get; set; } = new ObservableCollection<RawMaterialModel>();
        public List<AuxiliarRMSelected> RMSelected { get => _rMSelected; set { _rMSelected = value; OnPropertyChanged(); } }
        public ObservableCollection<AuxiliarRMSelected> RMselections { get; set; } = new ObservableCollection<AuxiliarRMSelected>();
        public ObservableCollection<GroupsRMModel> GroupsRMs { get; set; } = new ObservableCollection<GroupsRMModel>();
        public ObservableCollection<KardexRMModel> KardexRMs { get; set; } = new ObservableCollection<KardexRMModel>();

        public ICommand BackCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?ListMaterialId=0&IdWF=0&IdListaMaquinaria=0&CantProd=&NameProd=");
        });
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListRMs();
            CopyListRMs();

            IsRefreshing = false;
        });
        

        public ICommand SelectdCommand => new Command(async (obj) =>
        {
            //Recibo obj como RawmaterialModel
            RawMaterialModel rm = obj as RawMaterialModel;

            //Pregunto cuanto va a usar
            var cantUs = await Shell.Current.DisplayPromptAsync(rm.NameRM, "¿Cuántas/os " + rm.UMedidaRM.Description + ", va a usar?", "Ok", "Cancelar");
            var transf = Convert.ToDouble(cantUs);

            //Solo si la respuesta es mayor a 0 ejecuta
            if (transf > 0)
            {
                //Respaldo material seleccionado 
                // RawMaterialModel items = rm;

                //Retiro de la vista el material seleccionado
                RawMaterialsCopy.Remove(rm);
                //Modifico valores 
                rm.CantidadRM = rm.CantidadRM - transf;
                rm.TotalCost = (float)(rm.CostoRM * transf);
                RawMaterialsCopy.Add(rm);
                //Guardo material y cantidad restada
                AuxiliarRMSelected auxiliarRM = new AuxiliarRMSelected { CantReducida = transf, RawMaterial = rm };
                RMSelected.Add(auxiliarRM);
                SumarTotal();
            }
            //ListRMs();
            //Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM={rm.Id}&IdWF=0");
        });
        private void SumarTotal()
        {
            TotalValue = 0;
            TotalElements = 0;
            TotalValue = RMSelected.Sum(x => x.RawMaterial.CostoRM * x.CantReducida);
            TotalElements = RMSelected.Count();
            //RawMaterialsCopy = CopyListRMs();
        }
        
        private void CargarElecciones()
        {
            if (RMSelected.Count > 0)
            {

                foreach (var ec in RMSelected)
                {
                    RMselections.Add(ec);
                }
            }

        }  

        private void CopyListRMs()
        {
            List<RawMaterialModel> ayf = new List<RawMaterialModel>();
            foreach (var b in RawMaterialsCopy)
            {
                ayf.Add(b);
            }

            RawMaterialsCopy.Clear();
            foreach (var a in ayf)
            {
                RawMaterialsCopy.Add(a);
            }
        }
        #endregion
        public IList<GroupsRMModel> GrupofromRawMaterial { get => GroupRawMaterialData.GroupsRMs; }
        public IList<RawMaterialModel> ListRawMaterial
        {
            get
            {
                return RawMaterialData.RawMaterials;
                //OnPropertyChanged();
            }
        }
        public ObservableCollection<GroupsRMModel> GrupsRawMaterial { get; set; } = new ObservableCollection<GroupsRMModel>();
        public ObservableCollection<RawMaterialModel> ListRawMaterialsSelected { get; set; } = new ObservableCollection<RawMaterialModel>();
        
        GroupsRMModel grupoSeleccionado;
        public GroupsRMModel GrupoSeleccionado
        {
            get => grupoSeleccionado;
            set
            {
                if (grupoSeleccionado != value)
                {
                    grupoSeleccionado = value;
                    OnPropertyChanged();
                    //BuscarRawMaterial(grupoSeleccionado);
                }
            }
        }

        RawMaterialModel rawMaterialSeleccionada;
        public RawMaterialModel RawMaterialSelected
        {
            get => rawMaterialSeleccionada;
            set
            {
                if(rawMaterialSeleccionada != value)
                {
                    rawMaterialSeleccionada = value;
                    OnPropertyChanged();
                }
            }
        }

        double cantUsarMaterial;
        public double CantUsarMaterial
        {
            get => cantUsarMaterial;
            set
            {
                if(cantUsarMaterial != value)
                {
                    cantUsarMaterial = value;
                    OnPropertyChanged();
                    RawMaterialSelected.CantidadRM = RawMaterialSelected.CantidadRM - value;
                }
            }
        }

        private double _totalValue;
        public double TotalValue
        {
            get => _totalValue;
            set
            {
                if (_totalValue != value)
                {
                    _totalValue = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _totalElements;
        public int TotalElements
        {
            get => _totalElements;
            set
            {
                if (_totalElements != value)
                {
                    _totalElements = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand PushCommand => new Command((obj) =>
        {
            string BotonSeleccionado = obj as string;
            Direccionar(BotonSeleccionado);
        });

        public ICommand AddRawMaterialSelected => new Command(async () =>
        {
            //Creo lista de materiales
            ListRMModel listRM = new ListRMModel()
            {
                Total = ListRawMaterialsSelected.Sum(x => x.TotalCost)
            };
            await App.Database.SaveListRM(listRM);
             
            //
            foreach (var rawmaterialelegido in ListRawMaterialsSelected)
            {
                ItemsListRMModel ItemRawMaterial = new ItemsListRMModel
                {
                    Date = DateTime.Now,
                    Amount = rawmaterialelegido.CantidadRM,
                    UnitCost = rawmaterialelegido.CostoRM,
                    TotalCost = rawmaterialelegido.TotalCost
                };

                await App.Database.SaveItemListRM(ItemRawMaterial);

                var kardexRawMaterial = await App.Database.GetKardexXRM(rawmaterialelegido);

                ItemRawMaterial.ListRMModel = listRM;
                ItemRawMaterial.KardexRMModel = kardexRawMaterial;

                await App.Database.UpdateRelationItemRM(ItemRawMaterial);
            }

            var route = $"//Rini/Productos/NewStock/CostoProduccion?ListMaterialId={listRM.Id}&IdWF=0&IdListaMaquinaria=0&CantProd=&NameProd=";
            await Shell.Current.GoToAsync(route);
        });
        private void Direccionar(string botonSeleccionado)
        {
            switch (botonSeleccionado)
            {
                case "nuevo":
                    CargarMaterialSeleccionado();
                    CantUsarMaterial = 0;
                    break;
                case "inicio":
                    Shell.Current.GoToAsync("//Rini");
                    break;
                case "compras":
                    Shell.Current.GoToAsync("//Rini/RMateriaPrima/RCompras");
                    break;
                case "todo":
                    CopyListRMs();
                    break;
                case "selecciones":
                    ListRM = false;
                    ListRMselect = !ListRM;
                    if (ListRM == true)
                    {
                        CargarElecciones();
                    }
                    if (ListRM == true)
                    {
                        ListRMselect = !ListRM;
                        CopyListRMs();
                    }
                    break;
                default:
                    break;
            }
        }

        private void CargarMaterialSeleccionado()
        {
            if (cantUsarMaterial != 0)
            {
                RawMaterialSelected.CantidadRM = cantUsarMaterial;
                RawMaterialSelected.TotalCost = (float)(cantUsarMaterial * RawMaterialSelected.CostoRM);

                ListRawMaterialsSelected.Add(RawMaterialSelected);
                TotalElements = ListRawMaterialsSelected.Count();
                TotalValue = ListRawMaterialsSelected.Sum(x => x.TotalCost);

            }
            else
            {
                Shell.Current.DisplayAlert("Falta cantidad para usar", "Ingrese cuanto, va a usar", "ok");
            }
            
        }

        
        public SelectRawMaterialVM()
        {
        }
    }
}

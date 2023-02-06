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
    public class FixedAssetsVM : BaseVM
    {
        int _totalElementos;
        public int TotalElementos
        {
            get => _totalElementos;
            set
            {
                if (value != _totalElementos)
                {
                    _totalElementos = value;
                    OnPropertyChanged();
                }
            }
        }

        double _totalValores;
        public double TotalValores
        {
            get => _totalValores;
            set
            {
                if (value != _totalValores)
                {
                    _totalValores = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isRefreshing = false;
        private GroupsFixedAssetsModel _groupFASelected;
        private string _groupSelectedFA;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public GroupsFixedAssetsModel GroupFASelected
        {
            get => _groupFASelected;
            set
            {
                if (_groupFASelected != value)
                {
                    _groupFASelected = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GroupSelectedFA
        {
            get => _groupSelectedFA;
            set
            {
                _groupSelectedFA = value;
                OnPropertyChanged();
            }
        }
        public string SeleccionarGroupFA
        {
            get => _groupSelectedFA;
            set
            {                
                _groupSelectedFA = value;
                OnPropertyChanged();
                GroupSelectedFA = _groupSelectedFA;
            }
        }
        
        private List<GroupsFixedAssetsModel> _searchResults = App.Database.GetAllGroupsFixedAssets().Result;
        public List<GroupsFixedAssetsModel> SearchResults
        {
            get
            {
                return _searchResults;
            }
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FixedAssetsModel> FixedAssets { get; set; } = new ObservableCollection<FixedAssetsModel>();
        public ObservableCollection<GroupsFixedAssetsModel> GroupsFixedAssets { get; set; } = new ObservableCollection<GroupsFixedAssetsModel>();

        public ICommand PerformSearch => new Command(() =>
        {
            BuscarGR(GroupSelectedFA);
            //SearchResults = App.Database.GetESpecificGroup(vars);
        });
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            ListFixedAssets();

            IsRefreshing = false;
        });
        public ICommand PushCommand => new Command((obj) =>
        {
            string butt = obj as string;
            Direccionar(butt);
        });
        public ICommand SelectdCommand => new Command((obj) =>
        {
            var fa = obj as FixedAssetsModel;
            ListFixedAssets();
            //Shell.Current.GoToAsync($"NewAset?IdAF={fa.Id}");
        });

        public ICommand EliminarCommand => new Command((obj) =>
        {
            FixedAssetsModel fa = obj as FixedAssetsModel;
            FixedAssets.Remove(fa);
            App.Database.DeleteFixedAssets(fa);
            Shell.Current.DisplayAlert("Activo Eliminado", "Activo eliminado exitosamente", "ok");

        });

        public ICommand ModificarCommand => new Command((obj) =>
        {
            FixedAssetsModel fa = obj as FixedAssetsModel;

            //Shell.Current.GoToAsync($"//Rini/RMateriaPrima/NewMP?modificarRM={raw.Id}");
        });

        private void Direccionar(string butt)
        {
            switch (butt)
            {
                case "nuevo":
                    Shell.Current.GoToAsync("NewAset");
                    break;
                case "inicio":
                    Shell.Current.GoToAsync("//Rini");
                    break;
                case "compras":
                    Shell.Current.GoToAsync("//Rini/RMateriaPrima/RCompras");
                    break;
                case "todo":
                    ListFixedAssets();
                    break;
                default:
                    break;
            }
        }
        private void BuscarGR(string groupSelected)
        {
            if(groupSelected != null)
            {                
                //var rf = FixedAssets.Where(x => x.Grupo != groupSelected).ToList();
                var asset = App.Database.GetAssetsxGroup(groupSelected);
                asset.Wait();
                if (asset != null)
                {
                    FixedAssets.Clear();

                    foreach (var a in asset.Result)
                    {
                        FixedAssets.Add(a);
                    }
                }
                else
                {
                    Shell.Current.DisplayAlert("No hay resultados", "No hay resultados para su busqueda", "Ok");
                }
                

                /*var resp2 = App.Database.GetAllFixedAssets().Result;

                var g = (from a in FixedAssets
                         where a.Grupo == groupSelected
                         select a).ToList();
                if (g.Count > 0)
                {
                    //FixedAssets = new ObservableCollection<FixedAssetsModel>(g);
                    //FixedAssets.Clear();
                    foreach (var n in g)
                    {
                        FixedAssets.Add(n);
                        FixedAssets.CollectionChanged();
                    }
                }
                else
                {
                    FixedAssets.Clear();
                }*/
            }  

        }
        
        private void ListFixedAssets()
        {
            SeleccionarGroupFA = null;
            FixedAssets.Clear();
            var asf = App.Database.GetAllFixedAssets().Result;
            foreach (var a in asf)
            {
                FixedAssets.Add(a);
            }
            TotalElementos = FixedAssets.Count;
            TotalValores = FixedAssets.Sum(x => x.ValorUnit * x.Amount);
        }
        public FixedAssetsVM()
        {
            ListFixedAssets();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Windows.Input;
using ReinadelCisne.Models;
using Xamarin.Forms;
using System.Linq;


namespace ReinadelCisne.ViewModels
{
    public class RawMaterialVm : BaseVM
    {
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
        public ObservableCollection<RawMaterialModel> RawMaterials { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ObservableCollection<GroupsRMModel> GroupsRMs { get; set; } = new ObservableCollection<GroupsRMModel>();


        private GroupsRMModel _groupSelected;
        public GroupsRMModel GroupSelected
        {
            get => _groupSelected;
            set
            {
                if(_groupSelected != value)
                {
                    _groupSelected = value;
                    OnPropertyChanged();
                    
                }
            }
        }

        private void BuscarGR(GroupsRMModel groupSelected)
        {
            var resp = App.Database.GetESpecificGroup(groupSelected).Result;
            var resp2 = App.Database.GetMR().Result;

            var g = (from a in resp2
                     where a.UGroupModel == groupSelected.Id
                     select a).ToList();

            if(g != null)
            {
                RawMaterials.Clear();
                foreach (var n in g)
                {
                    RawMaterials.Add(n);
                }
            }
            
        }

        public ICommand PerformSearch => new Command(() =>
        {
            BuscarGR(GroupSelected);
            //SearchResults = App.Database.GetESpecificGroup(vars);
        });
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            ListRMs();

            IsRefreshing = false;
        });

        public ICommand PushCommand => new Command((obj) =>
        {
            string butt = obj as string;
            Direccionar(butt);
        });

        private void Direccionar(string butt)
        {
            switch (butt)
            {
                case "nuevo":
                    Shell.Current.GoToAsync("//Rini/RMateriaPrima/NewMP");
                    break;
                case "inicio":
                    Shell.Current.GoToAsync("//Rini");
                    break;
                case "compras":
                    Shell.Current.GoToAsync("//Rini/RMateriaPrima/RCompras");
                    break;
                case "todo":
                    ListRMs();
                    break;
                default:
                    break;
            }
        }

        private void ListGroupsRM()
        {
            List<GroupsRMModel> result = App.Database.GetGroupRM().Result;


            if (result.Count > 0)
            {
                foreach(var n in result)
                {
                    GroupsRMs.Add(n);
                }
            }


        }

        public RawMaterialVm()
        {
            ListRMs();
            ListGroupsRM();
        }

        private async void ListRMs()
        {
            GroupSelected = null;
            RawMaterials.Clear();
            List<RawMaterialModel> lrm = await App.Database.GetMR();
            if(lrm != null)
            {
                foreach (var obj in lrm)
                {
                    RawMaterials.Add(obj);
                    /*RawMaterials.Add(new RawMaterialShModel()
                    {
                        Description = obj.NameRM,
                        Measurament = obj.UMedidaRM.Description,
                        Amount = obj.AmountRM,
                        UnitCost = obj.CostoRM.ToString("N2")
                    });*/
                }
            }
            
        }
    }
}

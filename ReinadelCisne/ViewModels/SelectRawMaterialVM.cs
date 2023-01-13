﻿using System;
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
    public class SelectRawMaterialVM : BaseVM
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

        private GroupsRMModel _groupSelected;
        public GroupsRMModel GroupSelected
        {
            get => _groupSelected;
            set
            {
                if (_groupSelected != value)
                {
                    _groupSelected = value;
                    OnPropertyChanged();

                }
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
        public ObservableCollection<KardexRMModel> KardexRMs { get; set; } = new ObservableCollection<KardexRMModel>();

        public ICommand BackCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM=0&IdWF=0");
        });
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
        public ICommand SelectdCommand => new Command((obj) =>
        {
            var rm = obj as RawMaterialModel;
            ListRMs();
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM={rm.Id}&IdWF=0");
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
        private void BuscarGR(GroupsRMModel groupSelected)
        {
            if (groupSelected != null)
            {
                var resp = App.Database.GetESpecificGroup(groupSelected).Result;
                var resp2 = App.Database.GetMR().Result;

                var g = (from a in resp2
                         where a.UGroupModel == groupSelected.Id
                         select a).ToList();

                if (g != null)
                {
                    RawMaterials.Clear();
                    foreach (var n in g)
                    {
                        RawMaterials.Add(n);
                    }
                }
            }

        }
        private void ListGroupsRM()
        {
            List<GroupsRMModel> result = App.Database.GetGroupRM().Result;


            if (result.Count > 0)
            {
                foreach (var n in result)
                {
                    GroupsRMs.Add(n);
                }
            }


        }
        private void ListRMs()
        {
            GroupSelected = null;
            RawMaterials.Clear();

            var resp = App.Database.GetKardexsRM().Result;

            if (resp.Count > 0)
            {
                foreach (var obj in resp)
                {
                    if (obj.SaldosRMs.Count > 0 && obj.RawMaterialModell != null)
                    {
                        var respi = App.Database.GetSaldosxKardex(obj).Result;
                        var ord = respi.OrderByDescending(x => x.Date).FirstOrDefault();

                        RawMaterialModel rrm = App.Database.GetOneRM(obj.RawMaterialModell.Id).Result;
                        rrm.CantidadRM = ord.Cantidad;
                        rrm.CostoRM = (float)ord.ValorUnitario;
                        rrm.TotalCost = (float)ord.SaldoTotal;

                        RawMaterials.Add(rrm);
                    }
                }
            }

        }

        public SelectRawMaterialVM()
        {
            ListRMs();
            ListGroupsRM();
        }
    }
}

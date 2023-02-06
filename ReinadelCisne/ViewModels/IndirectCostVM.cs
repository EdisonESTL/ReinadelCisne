using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ReinadelCisne.Models;
using Xamarin.Forms;
using System.Linq;

namespace ReinadelCisne.ViewModels
{
    public class IndirectCostVM : BaseVM
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
        private OtherCostModel _cif;
        public OtherCostModel Cif
        {
            get => _cif;
            set
            {
                _cif = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<OtherCostModel> OtherCosts { get; set; } = new ObservableCollection<OtherCostModel>();
        public ICommand PushCommand => new Command((obj) =>
        {
            string opc = obj as string;
            Direccionar(opc);
        });
        public ICommand EliminarCommand => new Command((obj) =>
        {
            var ccts = obj as OtherCostModel;
            App.Database.DeleteOtherCost(ccts);
            OtherCosts.Remove(ccts);
            Shell.Current.DisplayAlert("Elemento Eliminado", "Valor eliminado correctamente", "ok");

        });

        public ICommand ModificarCommand => new Command((obj) =>
        {
            var ccts = obj as OtherCostModel;
            Cif = ccts;
            CargarIC();
        });
        private void Direccionar(string opc)
        {
            switch (opc)
            {
                case "regresar":
                    Shell.Current.GoToAsync("//Rini");
                    break;
                case "nuevoP":
                    Shell.Current.GoToAsync("//Rini/Productos/NewStock");
                    break;
                case "save":
                    SaveCostIF();
                    break;
                default:
                    break;
            }
        }

        private void SaveCostIF()
        {
            if(Cif.DescriptionOC != null && Cif.CostOC != 0)
            {
                var ciff = App.Database.SaveOtherCost(Cif);
                ciff.Wait();

                CargarIC();
                Cif = new OtherCostModel();
                Shell.Current.DisplayAlert("Registro completo", "El costo de indirecto de fabricación ha sido guardado", "Ok");

            }
        }

        public IndirectCostVM()
        {
            CargarIC();
            Cif = new OtherCostModel();
        }

        private void CargarIC()
        {
            OtherCosts.Clear();
            var iC = App.Database.GetAllOtherCost().Result;

            foreach(var i in iC)
            {
                OtherCosts.Add(i);
            }

            TotalElementos = OtherCosts.Count;
            TotalValores = OtherCosts.Sum(x => x.CostOC);
        }
    }
}

using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class CostosCostitucionVM : BaseVM
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

        private CostosConstitucionModel _costosConstitucion;
        public CostosConstitucionModel ConstitucionModel
        {
            get => _costosConstitucion;
            set
            {
                _costosConstitucion = value;
                OnPropertyChanged();
            }
        }
        private string _name;
        private double _valor;
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
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CostosConstitucionModel> ListCostosCostitucions { get; set; } = new ObservableCollection<CostosConstitucionModel>();

        public ICommand Push => new Command((action) =>
        {
            var accion = action as string;
            Direccionar(accion);
        });

        public ICommand SelectdCommand => new Command((obj) =>
        {
            var ccts = obj as CostosConstitucionModel;
            ConstitucionModel = ccts;
            GetCostosConstitucion();
            
        });
        public ICommand EliminarCommand => new Command((obj) =>
        {
            var ccts = obj as CostosConstitucionModel;
            App.Database.DeleteCostosConstitucion(ccts);
            ListCostosCostitucions.Remove(ccts);
            TotalElementos = ListCostosCostitucions.Count;
            TotalValores = ListCostosCostitucions.Sum(x => x.Valor);
            Shell.Current.DisplayAlert("Elemento Eliminado", "Valor eliminado correctamente", "ok");

        });

        public ICommand ModificarCommand => new Command((obj) =>
        {
            var ccts = obj as CostosConstitucionModel;
            ConstitucionModel = ccts;
            GetCostosConstitucion();
        });
        private void Direccionar(string accion)
        {
            switch (accion)
            {
                case "save":
                    SaveCostosConstitucion();
                    break;
                case "cancel":
                    Shell.Current.GoToAsync("..");
                    break;
                case "cc":
                    GetCostosConstitucion();
                    break;
                default:
                    break;
                    
            }
        }

        private void SaveCostosConstitucion()
        {
            if(ConstitucionModel.Name != null && ConstitucionModel.Valor != 0)
            {
                CostosConstitucionModel costosConstitucion = new CostosConstitucionModel
                {
                    Id = ConstitucionModel.Id,
                    Name = ConstitucionModel.Name,
                    Valor = ConstitucionModel.Valor
                };

                var guar = App.Database.SaveCostosCostitucion(costosConstitucion);
                guar.Wait();

                GetCostosConstitucion();
                ConstitucionModel = new CostosConstitucionModel();
                Shell.Current.DisplayAlert("Registro completo", "El costo de constitución ha sido guardado", "Ok");
            }
        }

        public CostosCostitucionVM()
        {
            ConstitucionModel = new CostosConstitucionModel();
            GetCostosConstitucion();
        }

        private void GetCostosConstitucion()
        {
            ListCostosCostitucions.Clear();
            List<CostosConstitucionModel> coscont = App.Database.GetAllCostosConstitucion1().Result;
            foreach(CostosConstitucionModel a in coscont)
            {
                ListCostosCostitucions.Add(a);
            }
            TotalElementos = ListCostosCostitucions.Count;
            TotalValores = ListCostosCostitucions.Sum(x => x.Valor);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReinadelCisne.Models;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Web;

namespace ReinadelCisne.ViewModels
{
    public class SelectMaquinariaVM : BaseVM, IQueryAttributable
    {
        double cantProducto;
        public double CantProducto
        {
            get => cantProducto;
            set
            {
                if(cantProducto != value)
                {
                    cantProducto = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _typeContrato;
        public string TypeContrato
        {
            get { return _typeContrato; }
            set
            {
                _typeContrato = value;
                OnPropertyChanged();
            }
        }
        FixedAssetsModel assetsModel;
        public FixedAssetsModel AssetsModel
        {
            get => assetsModel;
            set
            {
                if(value != assetsModel)
                {
                    assetsModel = value;
                    OnPropertyChanged();
                    CalculoDepreciacionEleccion();
                }
            }
        }        

        double cantUso;
        public double CantUso
        {
            get => cantUso;
            set
            {
                if(value != cantUso)
                {
                    cantUso = value;
                    OnPropertyChanged();
                }
            }
        }

        double depAnio;
        public double DepAnual
        {
            get => depAnio;
            set
            {
                if (value != depAnio)
                {
                    depAnio = value;
                    OnPropertyChanged();
                }
            }
        }
        double depMonth;
        public double DepMes
        {
            get => depMonth;
            set
            {
                if (value != depMonth)
                {
                    depMonth = value;
                    OnPropertyChanged();
                }
            }
        }
        double depDia;
        public double DepDia
        {
            get => depDia;
            set
            {
                if (value != depDia)
                {
                    depDia = value;
                    OnPropertyChanged();
                }
            }
        }
        double depHora;
        public double DepHora
        {
            get => depHora;
            set
            {
                if (value != depHora)
                {
                    depHora = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<FixedAssetsModel> FixedAssets { get; set; } = new ObservableCollection<FixedAssetsModel>();
        public ObservableCollection<ListFixedAssetsXproductModel> FixedAssetsSelected { get; set; } = new ObservableCollection<ListFixedAssetsXproductModel>();
        public ICommand BackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?ListMaterialId=0&IdWF=0&IdListaMaquinaria=0&CantProd=&NameProd="); 
        });
        public ICommand PushCommand => new Command((obj) =>
        {
            string butt = obj as string;
            Direccionar(butt);
        });

        public SelectMaquinariaVM()
        {
            CargarAssets();
        }
        private void Direccionar(string butt)
        {
            switch (butt)
            {
                case "nuevo":
                    AddMaquinaria();
                    break;
                case "guardar":
                    SaveMaquinaria();
                    break;
                default:
                    break;
            }
        }

        private void SaveMaquinaria()
        {
            ListFAxProductModel listFAx = new ListFAxProductModel
            {
                ValorTotalMaquinas = FixedAssetsSelected.Sum(x => x.FixedAssets.ValorUnit),
                ValorTotalDepreciaciones = FixedAssetsSelected.Sum(x => x.PrecioXuso)
            };

            var sveList = App.Database.SaveListFAxProd(listFAx);
            sveList.Wait();

            foreach(var maq in FixedAssetsSelected)
            {
                App.Database.SaveListSIxedAssetsProd(maq);

                maq.ListFAxProduct = listFAx;

                App.Database.UpdateRelationListFAxProd(maq);
            }
            var route = $"//Rini/Productos/NewStock/CostoProduccion?ListMaterialId=0&IdWF=0&IdListaMaquinaria={listFAx.Id}&CantProd=&NameProd=";
            Shell.Current.GoToAsync(route);
        }

        private void AddMaquinaria()
        {
            if (cantUso > 0)
            {
                double depreciacion = 0;
                //var assRec = new FixedAssetsModel();
                //assRec = App.Database.Get1FixedAsset(assetsModel).Result;

                switch (assetsModel.Grupo)
                {
                    case "Muebles y enseres":
                        depreciacion = CalculoDepreciacion1();
                        break;
                    case "Maquinaria":
                        depreciacion = CalculoDepreciacion1();
                        break;
                    case "Equipos de computo y Sw":
                        depreciacion = CalculoDepreciacion3();
                        break;
                    case "Vehiculos":
                        depreciacion = CalculoDepreciacion2();
                        break;
                    case "Edificios":
                        depreciacion = CalculoDepreciacion1();
                        break;
                    default:
                        break;
                }


                ListFixedAssetsXproductModel listFixed = new ListFixedAssetsXproductModel
                {
                    TipoUso = TypeContrato,
                    CantidadUso = cantUso,
                    //PrecioXuso = depreciacion/CantProducto,
                    PrecioXuso = depreciacion,
                    FixedAssets = assetsModel
                };

                FixedAssetsSelected.Add(listFixed);
            }
        }

        private double CalculoDepreciacion3()
        {
            //Calculo depreciación anual,
            //Equipos de cómputo y software 33.33% anual.

            DepAnual = assetsModel.ValorUnit * 0.3333;
            DepMes = Math.Round(DepAnual, 2) / 12;
            DepDia = Math.Round(DepAnual, 2) / 360;
            DepHora = Math.Round(DepDia, 3) / 24;

            switch (TypeContrato)
            {
                case "Horas":
                    return (Math.Round(DepHora, 3) * cantUso);
                case "Dias":
                    return (Math.Round(DepDia, 2) * cantUso);
                case "Meses":
                    return (Math.Round(DepMes, 2) * cantUso);
                default:
                    return 0;
            }
        }

        private double CalculoDepreciacion2()
        {
            //Calculo depreciación anual,
            //Vehículos, equipos de transporte y equipo caminero móvil 20% anual.

            DepAnual = assetsModel.ValorUnit * 0.20;
            DepMes = Math.Round(DepAnual, 2) / 12;
            DepDia = Math.Round(DepAnual, 2) / 360;
            DepHora = Math.Round(DepDia, 3) / 24;

            switch (TypeContrato)
            {
                case "Horas":
                    return (Math.Round(DepHora, 3) * cantUso);
                case "Dias":
                    return (Math.Round(DepDia, 2) * cantUso);
                case "Meses":
                    return (Math.Round(DepMes, 2) * cantUso);
                default:
                    return 0;
            }
        }

        private double CalculoDepreciacion1()
        {
            //Calculo depreciación anual,
            //Instalaciones, maquinarias, equipos y muebles 10% anual.

            DepAnual = assetsModel.ValorUnit * 0.10;
            DepMes = Math.Round(DepAnual, 2) / 12;
            DepDia = Math.Round(DepAnual, 2) / 360;
            DepHora = Math.Round(depDia, 3) / 24;

            switch (TypeContrato)
            {
                case "Horas":
                    return (Math.Round(DepHora, 2) * cantUso);
                case "Dias":
                    return (Math.Round(DepDia, 2) * cantUso);
                case "Meses":
                    return (Math.Round(DepMes, 3) * cantUso);
                default:
                    return 0;
            }
        }
        private void CalculoDepreciacionEleccion()
        {
            double depreciacion = 0;
            //var assRec = new FixedAssetsModel();
            //assRec = App.Database.Get1FixedAsset(assetsModel).Result;

            switch (assetsModel.Grupo)
            {
                case "Muebles y enseres":
                    depreciacion = CalculoDepreciacion1();
                    break;
                case "Maquinaria":
                    depreciacion = CalculoDepreciacion1();
                    break;
                case "Equipos de computo y Sw":
                    depreciacion = CalculoDepreciacion3();
                    break;
                case "Vehiculos":
                    depreciacion = CalculoDepreciacion2();
                    break;
                case "Edificios":
                    depreciacion = CalculoDepreciacion1();
                    break;
                default:
                    break;
            }
        }

        private void CargarAssets()
        {
            var asf = App.Database.GetAllFixedAssets().Result;
            foreach (var a in asf)
            {
                FixedAssets.Add(a);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string CantProduc = HttpUtility.UrlDecode(query["CantProduc"]);
            CargarCantidadProducto(CantProduc);
        }
        private void CargarCantidadProducto(string canProduc)
        {
            try
            {
                CantProducto = double.Parse(canProduc);

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }

        }
    }
}

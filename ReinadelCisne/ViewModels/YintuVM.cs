using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using ReinadelCisne.Models;
using ReinadelCisne.Auxiliars;
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
    public class YintuVM : BaseVM
    {
        private string _nameNegocio;
        public string NameNegocio
        {
            get => _nameNegocio;
            set
            {
                _nameNegocio = value;
                OnPropertyChanged();
            }
        }

        private string _nameOwner;
        public string NameOwner
        {
            get => _nameOwner;
            set
            {
                _nameOwner = value;
                OnPropertyChanged();
            }
        }

        public ICommand PushCommand { get; set; }

        public YintuVM()
        {
            PushCommand = new Command<string>((textbtn) => Direcc(textbtn));
            NameOwner = UserModel.NameUser;
            NameNegocio = UserModel.NegocioUser;
        }

        private void Direcc(string textbtn)
        {
            switch (textbtn)
            {
                case "productos":
                    Shell.Current.GoToAsync("//Rini/Productos"); break;                    
                case "productosProceso":
                    Shell.Current.GoToAsync("//Rini/RProductosEnProceso"); break;
                case "ventas":
                    Shell.Current.GoToAsync("//Rini/RVentas"); break;
                case "compras":
                    Shell.Current.GoToAsync("//Rini/RCompras"); break;
                case "ingresos":
                    Shell.Current.GoToAsync("//Rini/RIngresoEgreso"); break;
                case "materiaprima":
                    Shell.Current.GoToAsync("//Rini/RMateriaPrima"); break;
                case "reportes":
                    Shell.Current.GoToAsync("//Rini/RReportes"); break;
                case "ventanilla":
                    Shell.Current.GoToAsync("//Rini/RVentanilla"); break;
                case "ajustes":
                    Shell.Current.GoToAsync("//Rini/RAjustes"); break;
                case "manoobra":
                    Shell.Current.GoToAsync($"//Rini/ManoObra?save=false"); break;
                case "costosindirectos":
                    Shell.Current.GoToAsync("//Rini/OtherCost"); break;
                case "mas":
                    Shell.Current.GoToAsync("//Rini/CostosConstitucion"); break;
                case "assets":
                    Shell.Current.GoToAsync("//Rini/Assets"); break;
                case "nuevo":
                    YintuNuevo(); break;
                case "ordenProduccion":
                    Shell.Current.GoToAsync("//Rini/OrdenProduccion"); break;
                default:
                    break;
            }
        }

        private async void YintuNuevo()
        {
            string action = await Shell.Current.DisplayActionSheet("Nuevo", "Cancelar", null, "Producto", "Pedido", "Materia prima", "Mano de obra", "Costo indirecto");

            switch (action)
            {
                case "Producto":
                    await Shell.Current.GoToAsync("//Rini/Productos/NewStock");
                    break;
                case "Pedido":
                    await Shell.Current.GoToAsync("//Rini/RVentanilla");
                    break;
                case "Materia prima":
                    await Shell.Current.GoToAsync("//Rini/RMateriaPrima/NewMP");
                    break;
                case "Mano de obra":
                    await Shell.Current.GoToAsync("//Rini/ManoObra");
                    break;
                case "Costo indirecto":
                    await Shell.Current.GoToAsync("//Rini/OtherCost");
                    break;
                default:
                    break;
            }
        }
    }
}

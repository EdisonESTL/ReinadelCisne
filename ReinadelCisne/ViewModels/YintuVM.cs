using System;
using System.Collections.Generic;
using System.Text;
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
                case "servicios":
                    Shell.Current.GoToAsync("//Rini/RServicios"); break;
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
                    Shell.Current.GoToAsync("//Rini/ManoObra"); break;
                case "costosindirectos":
                    Shell.Current.GoToAsync("//Rini/OtherCost"); break;
                case "mas":
                    break;
                case "nuevo":
                    YintuNuevo(); break;
                default:
                    break;
            }
        }

        private void YintuNuevo()
        {
            Shell.Current.DisplayActionSheet("Nuevo", "Cancelar", null, "Producto", "Servicio", "Compra", "Venta", "Materia prima", "Ingreso-Egreso extra");
        }
    }
}

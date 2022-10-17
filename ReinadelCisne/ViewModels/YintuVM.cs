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
                    Shell.Current.GoToAsync("//Rserv"); break;
                case "ventas":
                    Shell.Current.GoToAsync("//Rven"); break;
                case "compras":
                    Shell.Current.GoToAsync("//Rcomp"); break;
                case "ingresos":
                    Shell.Current.GoToAsync("//Ring"); break;
                case "materiaprima":
                    Shell.Current.GoToAsync("//Rmat"); break;
                case "reportes":
                    Shell.Current.GoToAsync("//Rrep"); break;
                case "ventanilla":
                    Shell.Current.GoToAsync("//Rvent"); break;
                default:
                    break;
            }
        }
    }
}

using ReinadelCisne.Auxiliars;
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
    public class ServicesVM
    {
        public ICommand Push => new Command((obj) =>
        {
            var recibo = obj as string;
            Direccionar(recibo);
        });

        private void Direccionar(string recibo)
        {
            switch (recibo)
            {
                case "inicio":
                    Shell.Current.GoToAsync("//Rini"); break;
                case "nuevo":
                    Shell.Current.GoToAsync("//Rini/RServicios/NewService"); break;
                default:
                    break;
            }
        }
    }
}

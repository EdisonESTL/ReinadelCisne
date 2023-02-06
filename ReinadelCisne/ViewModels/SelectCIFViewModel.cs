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
    public class SelectCIFViewModel
    {
        public ICommand BackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?ListMaterialId=0&IdWF=0&IdListaMaquinaria=0&CantProd=&NameProd=");
        });
    }
}

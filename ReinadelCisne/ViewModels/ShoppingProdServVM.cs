using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Models;
using System.Linq;
using System.Threading.Tasks;
using dotMorten.Xamarin.Forms;
using ReinadelCisne.Auxiliars;

namespace ReinadelCisne.ViewModels
{
    public class ShoppingProdServVM
    {
        public ObservableCollection<RawMaterialModel> ListCompras2 { get; set; } = new ObservableCollection<RawMaterialModel>();
        public ICommand BackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("//Rini/RMateriaPrima/RCompras");
        });
        public ICommand PushCommand => new Command((obj) =>
        {
            string push = obj as string;
            Cargar(push);
        });

        public ICommand SelectedCommnad => new Command((obj) =>
        {
            RawMaterialModel pass = obj as RawMaterialModel;
            ConsultarMateriaPrima();
            Shell.Current.GoToAsync($"//Rini/RMateriaPrima/RCompras/SelectionRM/NewShopping?IdRM={pass.Id}");
        });


        private void Cargar(string push)
        {
            switch (push)
            {
                case "productos":
                    break;
                case "materiaprima":
                    ConsultarMateriaPrima();
                    break;
                default:
                    break;
            }
        }

        private void ConsultarMateriaPrima()
        {
            ListCompras2.Clear();
            List<RawMaterialModel> lps = App.Database.GetMR().Result;
            if (lps.Count > 0)
            {
                foreach (var tp in lps.OrderByDescending(x => x.Id))
                {
                    ListCompras2.Add(tp);
                }
            }
        }

    
        public ShoppingProdServVM()
        {
            ConsultarMateriaPrima();
        }
    }
}

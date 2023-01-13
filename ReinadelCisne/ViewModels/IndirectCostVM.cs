using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ReinadelCisne.Models;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class IndirectCostVM
    {
        public ObservableCollection<OtherCostModel> OtherCosts { get; set; } = new ObservableCollection<OtherCostModel>();
        public ICommand PushCommand => new Command((obj) =>
        {
            string opc = obj as string;
            Direccionar(opc);
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
                default:
                    break;
            }
        }

        public IndirectCostVM()
        {
            CargarIC();
        }

        private void CargarIC()
        {
            var iC = App.Database.GetAllOtherCost().Result;

            foreach(var i in iC)
            {
                OtherCosts.Add(i);
            }
        }
    }
}

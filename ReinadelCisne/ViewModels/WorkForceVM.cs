using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Windows.Input;
using ReinadelCisne.Models;
using Xamarin.Forms;
using System.Linq;

namespace ReinadelCisne.ViewModels
{
    public class WorkForceVM: BaseVM, IQueryAttributable
    {
        public ObservableCollection<WorkForceModel> WorkForces { get; set; } = new ObservableCollection<WorkForceModel>();

        public ICommand PushCommand => new Command((obj) =>
        {
            string opc = obj as string;
            Direccionar(opc);
        });
        public ICommand EliminarCommand => new Command((obj) =>
        {
            WorkForceModel worf = obj as WorkForceModel;
            App.Database.DeleteWorkcForce(worf);
            WorkForces.Remove(worf);
            Shell.Current.DisplayAlert("Elemento Eliminado", "Materia prima eliminada correctamente", "ok");

        });

        public ICommand ModificarCommand => new Command((obj) =>
        {
            WorkForceModel worf = obj as WorkForceModel;

            Shell.Current.GoToAsync($"//Rini/ManoObra/NewManoObra?modificarRM={worf.Id}");
        });
        public ICommand SelectdCommand => new Command((obj) =>
        {
            WorkForceModel worf = obj as WorkForceModel;
            CargarWF();
            Shell.Current.GoToAsync($"//Rini/ManoObra/DetailManoObra?IdWF={worf.Id}");
        });
        private void Direccionar(string opc)
        {
            switch (opc)
            {
                case "regresar":
                    Shell.Current.GoToAsync("//Rini");
                    break;
                case "nuevoM":
                    Shell.Current.GoToAsync($"//Rini/ManoObra/NewManoObra?modificarRM=0");
                    break;
                case "directo":
                    Filtrar("Directo");
                    break;
                case "indirecto":
                    Filtrar("Indirecto");
                    break;
                default:
                    break;
            }
        }
        
        private void Filtrar(string tipo)
        {
            var fil = (from a in App.Database.GetAllWorkForce().Result
                       where a.Type == tipo
                       select a).ToList();

            WorkForces.Clear();

            foreach (var n in fil)
            {
                WorkForces.Add(n);
            }
        }

        public WorkForceVM()
        {
        }

        private void CargarWF()
        {
            WorkForces.Clear();
            var wf = App.Database.GetAllWorkForce().Result;
            if (wf.Count > 0)
            {
                foreach (var w in wf)
                {
                    WorkForces.Add(w);
                }
            }
            
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string guardado = HttpUtility.UrlDecode(query["save"]);
            LoadList(guardado);            
        }

        private void LoadList(string guardado)
        {
            try
            {
                CargarWF();
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load animal.");
            }
        }
    }
}

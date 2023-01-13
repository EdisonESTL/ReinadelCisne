using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Services;
using ReinadelCisne.Models;
using System.Web;

namespace ReinadelCisne.ViewModels
{
    public class NewWorkForceVM : BaseVM, IQueryAttributable
    {
        private WorkForceVM _workForce;
        public WorkForceVM WorkForce
        {
            get => _workForce;
            set
            {
                _workForce = value;
                OnPropertyChanged();
            }
        }

        private int _id;
        public int ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        private string _jobTitle;
        public string JobTitle 
        { 
            get => _jobTitle;
            set
            {
                _jobTitle = value;
                OnPropertyChanged();
            }
        }

        private string _tasks;
        public string Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }

        private string _typewf;

        public string Typewf
        {
            get => _typewf;
            set
            {
                _typewf = value;
                OnPropertyChanged();
            }
        }

        public string SelecctTypeWF
        {
            get => _typewf;
            set
            {
                _typewf = value;
                Typewf = _typewf;
            }
        }
        private List<string> Valores { get; set; }
        public ICommand Push => new Command((obj) =>
        {
            var opc = obj as string;
            Direccionar(opc);
        });

        private void Direccionar(string opc)
        {
            switch (opc)
            {
                case "guardar":
                    SaveWorkForce();
                    CleanUp();
                    Shell.Current.GoToAsync($"//Rini/ManoObra?save=true");
                    break;
                case "cancelar":
                    CleanUp();
                    Shell.Current.GoToAsync("//Rini/ManoObra");
                    break;
                default:
                    break;
            }
        }

        private void CleanUp()
        {
            Valores.Clear();
            JobTitle = string.Empty;
            Tasks = string.Empty;
            Typewf = string.Empty;
        }

        private void SaveWorkForce()
        {
            Valores.Add(JobTitle);
            Valores.Add(Tasks);
            Valores.Add(Typewf);
            if (!CampsNullsEmpty(Valores))
            {
                WorkForceModel workForce = new WorkForceModel
                {
                    Id = ID,
                    Name = JobTitle,
                    Tasks = Tasks,
                    Type = Typewf
                };

                App.Database.SaveWorkForce(workForce);

                Shell.Current.DisplayAlert("Éxito", "El nuevo puesto de trabajo, " + workForce.Name + " ha sido guardado", "ok");
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Hay campos vacios", "ok");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            // The query parameter requires URL decoding.
            string idMod = HttpUtility.UrlDecode(query["modificarRM"]);
            Loadwf(idMod);
            
        }

        private void Loadwf(string idMod)
        {
            try
            {
                if(idMod != "0") 
                {
                    var wf = App.Database.GetWorkForce(int.Parse(idMod)).Result;

                    ID = wf.Id;
                    JobTitle = wf.Name;
                    Tasks = wf.Tasks;
                    SelecctTypeWF = wf.Type;
                }
                else
                {
                    ID = 0;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load animal.");
            }
        }

        public NewWorkForceVM()
        {
            WorkForce = new WorkForceVM();
            Valores = new List<string>();
        }

    }
}

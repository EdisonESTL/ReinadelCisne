using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Windows.Input;
using ReinadelCisne.Models;
using Xamarin.Forms;

namespace ReinadelCisne.ViewModels
{
    public class InWorkForceVM : BaseVM, IQueryAttributable
    {
        private int _id;
        public int Id { get { return _id; }
            set { _id = value;
                OnPropertyChanged();
            }
        }
        
        private string _profesional;
        public string Profesional
        {
            get { return _profesional; }
            set { _profesional = value;
                OnPropertyChanged();
            }
        }
        
        private int _numberProf;
        public int NumberProf { get { return _numberProf; }
            set { _numberProf = value;
                OnPropertyChanged();
            }
        }
        
        private string _salary;
        public string Salary { get { return _salary; }
            set { _salary = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private double _count = 0;
        public double Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private int _longList = 0;
        public int LongList
        {
            get { return _longList; }
            set
            {
                _longList = value;
                OnPropertyChanged();
            }
        }

        public int IdList;
        public string IdMOD;
        public ObservableCollection<WorkForceModel> Works { get; set; } = new ObservableCollection<WorkForceModel>();
        
        public ICommand goback
        {
            get
            {
                return new Command(() =>
                {
                    Shell.Current.GoToAsync($"..?IdlistWF=0");
                });
            }
        }
        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            //ListProductStock();

            IsRefreshing = false;
        });
        public ICommand AddWFCommand => new Command(() =>
        {
            if(NumberProf != 0 & !string.IsNullOrWhiteSpace(Profesional))
            {
                WorkForceModel workForce = new WorkForceModel()
                {
                    Id = Id,
                    Profesional = Profesional,
                    Amount = NumberProf,
                    UnitSalary = Convert.ToDouble(Salary)
                };

                if (IdMOD != null)
                {
                    Works.RemoveAt(int.Parse(IdMOD));
                    Works.Insert(int.Parse(IdMOD), workForce);
                    Shell.Current.DisplayAlert("Éxito", workForce.Profesional + " Ha sido modificado", "Ok");
                    Count = 0;
                    foreach (var a in Works)
                    {
                        Count += a.Amount * a.UnitSalary;
                    }

                    IdMOD = null;
                }
                else
                {
                    Works.Add(workForce);
                    Shell.Current.DisplayAlert("Éxito", workForce.Profesional + " Ha sido añadido", "Ok");
                    Count += workForce.UnitSalary * workForce.Amount;
                }

                LongList = Works.Count;

                Profesional = string.Empty;
                NumberProf = 0;
                Salary = string.Empty;
            }
            else
            {
                Shell.Current.DisplayAlert("Error", " Ingrese Cantidad de preofesionales necesarios y nombre de profesion", "Ok");
            }

        });
        public ICommand FinshCommand => new Command(() =>
        {
            if(Works.Count != 0)
            {
                ListWFModel listWF = new ListWFModel
                {
                    Id = IdList
                };
                App.Database.SaveListWF(listWF);

                listWF.WorkForces = new List<WorkForceModel>();

                foreach (var f in Works)
                {
                    App.Database.SaveWorkForce(f);
                    listWF.WorkForces.Add(f);

                    App.Database.UpdateListWF(listWF);
                }

                var gy = App.Database.GetListsWF();
                gy.Wait();
                var fgh = gy.Result;

                var fd = listWF.Id;

                Shell.Current.GoToAsync($"..?IdListOC=0&IdlistRM=0&IdlistWF={fd}&idProduct=0");
            }
            else
            {
                Shell.Current.GoToAsync($"..?IdListOC=0&IdlistRM=0&IdlistWF=0&idProduct=0");
            }
            
        });
        public ICommand CancelCommand => new Command(() =>
        {
            Id = 0;
            IdList = 0;
            Profesional = string.Empty;
            NumberProf = 0;
            Salary = string.Empty;
            Works.Clear();
            Count = 0;
            LongList = 0;
            
        });
        public Command<WorkForceModel> DeleteCommand { get; set; }
        public Command<WorkForceModel> ModifyCommand { get; set; }
        public InWorkForceVM()
        {
            DeleteCommand = new Command<WorkForceModel>(DeleteWF);
            ModifyCommand = new Command<WorkForceModel>(ModifyWF);
        }

        private void DeleteWF(WorkForceModel obj)
        {
            Works.Remove(obj);
            Count -= obj.Amount * obj.UnitSalary;
            LongList = Works.Count;
        }
        private void ModifyWF(WorkForceModel obj)
        {
            IdMOD = Convert.ToString(Works.IndexOf(obj));

            Id = obj.Id;
            Profesional = obj.Profesional;
            NumberProf = obj.Amount;
            Salary = obj.UnitSalary.ToString();            
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string idListWF = HttpUtility.UrlDecode(query["IdListWF"]);

                LoadLS(idListWF);

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }

        private void LoadLS(string idListWF)
        {
            Count = 0;
            ListWFModel t = App.Database.GetListWF(int.Parse(idListWF)).Result;

            foreach (WorkForceModel d in t.WorkForces)
            {
                Works.Add(d);
                Count += d.Amount * d.UnitSalary;
            }

            IdList = int.Parse(idListWF);
            LongList = t.WorkForces.Count;
        }
    }
}

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
    public class SelectWorkForceVM : BaseVM, IQueryAttributable
    {
        private WorkForceModel _perfilWF;

        public WorkForceModel PerfilWF
        {
            get => _perfilWF;
            set
            {
                _perfilWF = value;
                OnPropertyChanged();
            }
        }

        private int _iD;
        private string _name;
        private string _cI;
        private string _cellNumber;
        private string _pay;
        private string _state;

        public int ID
        {
            get => _iD;
            set
            {
                _iD = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string CI
        {
            get => _cI;
            set
            {
                _cI = value;
                OnPropertyChanged();
            }
        }

        public string Cellnumber
        {
            get => _cellNumber;
            set
            {
                _cellNumber = value;
                OnPropertyChanged();
            }
        }

        public string Pay
        {
            get => _pay;
            set
            {
                _pay = value;
                OnPropertyChanged();
            }
        }

        public string States
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }
        public string CantDiasHoras
        {
            get => _cantDiaHora;
            set
            {
                _cantDiaHora = value;
                OnPropertyChanged();
            }
        }
        public string DescripcionObra
        {
            get => _descripcionObra;
            set
            {
                _descripcionObra = value;
                OnPropertyChanged();
            }
        }

        private string _typeContrato;
        private bool _isMonth = false;
        private bool _isHoursDay = false;
        private bool _isObra = false;
        private DateTime _minDate = new DateTime(2018, 1, 1);
        private DateTime _maxDate = new DateTime(2050, 1, 1);
        private DateTime _selectedDate;
        private string _cantDiaHora;
        private string _descripcionObra;

        public string TypeContrato
        {
            get { return _typeContrato; }
            set
            {
                _typeContrato = value;
                OnPropertyChanged();
                switch (_typeContrato)
                {
                    case "Por horas":
                        IsMonth = false;
                        IsHoursDays = true;
                        IsObra = false;
                        break;
                    case "Mensual":
                        IsMonth = true;
                        IsHoursDays = false;
                        IsObra = false;
                        break;
                    case "Por obra":
                        IsMonth = false;
                        IsHoursDays = false;
                        IsObra = true;
                        break;
                    case "Por dias":
                        IsMonth = false;
                        IsHoursDays = true;
                        IsObra = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public bool IsMonth
        {
            get => _isMonth;
            set
            {
                _isMonth = value;
                OnPropertyChanged();
            }
        }
        public bool IsHoursDays
        {
            get => _isHoursDay;
            set
            {
                _isHoursDay = value;
                OnPropertyChanged();
            }
        }
        public bool IsObra
        {
            get => _isObra;
            set
            {
                _isObra = value;
                OnPropertyChanged();
            }
        }

        public DateTime MinDate
        {
            get => _minDate;
            set
            {
                _minDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime MaxDate
        {
            get => _maxDate;
            set
            {
                _maxDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<WorkForceModel> PerfilesLaborales { get; set; } = new ObservableCollection<WorkForceModel>();

        public ICommand BackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM=0&IdWF=0");
        });

        public ICommand PushCommand => new Command((obj) =>
        {
            var btn = obj as string;
            Direccionar(btn);
        });

        private void Direccionar(string btn)
        {
            switch (btn)
            {
                case "cancelar":
                    Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM=0&IdWF=0");
                    break;
                case "guardar":
                    SaveWF();
                    break;
                default:
                    break;
            }
        }

        private void SaveWF()
        {
            PersonalModel personal = new PersonalModel
            {
                Id = ID,
                Name = Name,
                CI = CI,
                Telephone = Cellnumber,
                Pay = float.Parse(Pay),
                Date = DateTime.Now,
                TypeContract = TypeContrato
            };

            App.Database.SavePersonal(personal);

            personal.WorkForce = PerfilWF;

            var rr = App.Database.UpdateRelationsPersonal(personal);
            rr.Wait();

            CrearRolPagos(personal);
            
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM=0&IdWF={personal.Id}");
        }

        private void CrearRolPagos(PersonalModel personal)
        {
            switch (TypeContrato)
            {
                case "Por horas":
                    personal.CantDiauHora = double.Parse(CantDiasHoras);
                    App.Database.SavePersonal(personal);

                    PaymentsModel payments = new PaymentsModel
                    {
                        Date = DateTime.Now,
                        ARecibir = personal.Pay
                    };

                    var fg = App.Database.SavePayments(payments);
                    fg.Wait();

                    payments.PersonalModel = personal;
                    App.Database.UpdateRelationsPayments(payments);
                    break;
                case "Mensual":
                    personal.DateInicio = MinDate;
                    personal.DateFinal = MaxDate;
                    App.Database.SavePersonal(personal);

                    var mess = MaxDate.Month - MinDate.Month;

                    PaymentsModel paymes = new PaymentsModel
                    {
                        Date = MinDate,
                        XIII = personal.Pay/12,
                        XIV = personal.Pay,
                        FondoReserva = personal.Pay * 0.0833,
                        TotalIngreso = personal.Pay + (personal.Pay * 0.0833) + (personal.Pay / 12) + personal.Pay,
                        AporteIESS = personal.Pay * 0.0945,
                        TotalEgreso = personal.Pay * 0.0945,
                        ARecibir = (personal.Pay + (personal.Pay * 0.0833) + (personal.Pay / 12) + personal.Pay) - (personal.Pay * 0.0945)
                    };
                    var fy = App.Database.SavePayments(paymes);
                    fy.Wait();

                    paymes.PersonalModel = personal;
                    App.Database.UpdateRelationsPayments(paymes);
                    break;
                case "Por obra":
                    personal.DescriptionObra = DescripcionObra;
                    App.Database.SavePersonal(personal);

                    PaymentsModel paymens = new PaymentsModel
                    {
                        Date = DateTime.Now,
                        ARecibir = personal.Pay
                    };

                    var fi = App.Database.SavePayments(paymens);
                    fi.Wait();

                    paymens.PersonalModel = personal;
                    App.Database.UpdateRelationsPayments(paymens);
                    break;
                case "Por dias":
                    personal.CantDiauHora = double.Parse(CantDiasHoras);
                    App.Database.SavePersonal(personal);

                    PaymentsModel paymen = new PaymentsModel
                    {
                        Date = DateTime.Now,
                        ARecibir = personal.Pay
                    };

                    var fgi = App.Database.SavePayments(paymen);
                    fgi.Wait();

                    paymen.PersonalModel = personal;
                    App.Database.UpdateRelationsPayments(paymen);
                    break;
                default:
                    break;
            }            
        }

        public SelectWorkForceVM()
        {
            LoadPerfiles();
        }

        private void LoadPerfiles()
        {
            List<WorkForceModel> cons = App.Database.GetAllWorkForce().Result;

            foreach(var a in cons)
            {
                PerfilesLaborales.Add(a);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string ModWfId = HttpUtility.UrlDecode(query["ModWfId"]);
            CargarWF(ModWfId);
        }

        private void CargarWF(string modWfId)
        {
            try
            {
                if(modWfId != "0")
                {
                    var wff = App.Database.GetPersonal(int.Parse(modWfId)).Result;

                    ID = wff.Id;
                    Name = wff.Name;
                    CI = wff.CI;
                    Cellnumber = wff.Telephone;
                    Pay = wff.Pay.ToString();
                    PerfilWF = wff.WorkForce;
                    TypeContrato = wff.TypeContract;
                }
                else
                {
                    ID = 0;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }
    }
}

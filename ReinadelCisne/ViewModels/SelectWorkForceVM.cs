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
        private double _pay;
        private string _state;
        private int _numElementos;
        private double _valorTotalElementos;
        double _totalMeses;

        public int NumElementos
        {
            get => _numElementos;
            set
            {
                if(value != _numElementos)
                {
                    _numElementos = value;
                    OnPropertyChanged();
                }
                
            }
        }
        public double ValorTotalElementos
        {
            get => _valorTotalElementos;
            set
            {
                if (value != _valorTotalElementos)
                {
                    _valorTotalElementos = value;
                    OnPropertyChanged();
                }

            }
        }
        public double TotalMeses
        {
            get => _totalMeses;
            set
            {
                if (value != _totalMeses)
                {
                    _totalMeses = value;
                    OnPropertyChanged();
                }

            }
        }
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
        public double Pay
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
                if(_cantDiaHora != value)
                {
                    _cantDiaHora = value;
                    OnPropertyChanged();
                    EstimarPago();
                }                
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
        private TimeSpan _selectedDate;
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
                EstimarPago();
            }
        }
        public DateTime MaxDate
        {
            get => _maxDate;
            set
            {
                _maxDate = value;
                OnPropertyChanged();
                EstimarPago();
            }
        }
        public TimeSpan SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<WorkForceModel> PerfilesLaborales { get; set; } = new ObservableCollection<WorkForceModel>();
        public ObservableCollection<PersonalModel> ListPersonal { get; set; } = new ObservableCollection<PersonalModel>();

        public ICommand BackCommand => new Command(() =>
        {
            Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?ListMaterialId=0&IdWF=0&IdListaMaquinaria=0&CantProd=&NameProd=");
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
                    Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM=0&IdWF=0&IdListaMaquinaria=0&CantProd=&NameProd=");
                    break;
                case "guardar":
                    SaveWF();
                    break;
                case "addmanoobra":
                    AddWorkForce();
                    ClearAll();
                    NumElementos = ListPersonal.Count();
                    ValorTotalElementos = ListPersonal.Sum(x => x.Pay);
                    break;
                default:
                    break;
            }
        }

        private void AddWorkForce()
        {
            if (!string.IsNullOrEmpty(Name) && Pay >= 0)
            {
                //EstimarPago();
                PersonalModel personal = new PersonalModel
                {
                    Id = ID,
                    Name = Name,
                    CI = CI,
                    Telephone = Cellnumber,
                    Pay = (float)Pay,
                    Date = DateTime.Now,
                    TypeContract = TypeContrato,
                    WorkForce = PerfilWF
                };

                ListPersonal.Add(personal);
                NumElementos = ListPersonal.Count();
                ValorTotalElementos = ListPersonal.Sum(x => x.Pay);
                
            }
            else
            {
                Shell.Current.DisplayAlert("Sin nombre", "Por favor, ingrese un nombre", "ok");
            }
            
        }

        private void ClearAll()
        {
            ID = 0;
            PerfilWF = null;
            Name = string.Empty;
            CI = string.Empty;
            Cellnumber = string.Empty;
            Pay = 0;
            CantDiasHoras = string.Empty;
            TotalMeses = 0;
        }

        private void EstimarPago()
        {
            if(PerfilWF!= null)
            {
                var PagoDiario = PerfilWF.PayMonth / 30;

                switch (TypeContrato)
                {
                    case "Por horas":
                        var pagohora = PagoDiario / 8;

                        var pagoPersonal = pagohora * int.Parse(CantDiasHoras);
                        Pay = pagoPersonal;

                        break;
                    case "Mensual":
                        SelectedDate = MaxDate.Subtract(MinDate);
                        var numeromeses = SelectedDate.Days / 30;
                        Pay = PerfilWF.PayMonth * numeromeses;
                        TotalMeses = numeromeses;
                        break;
                    case "Por obra":

                        break;
                    case "Por dias":
                        var pagodia = PagoDiario;
                        Pay = pagodia * int.Parse(CantDiasHoras);
                        break;
                    default:
                        break;
                }
            }
            /*else
            {
                if(PerfilWF.Id >= 0)
                {
                    Shell.Current.DisplayAlert("Seleccione puesto laboral", "El personal no tiene asignado un puesto laboral", "ok");
                }
            }*/
            
        }

        private void SaveWF()
        {
            if (ListPersonal.Count > 0)
            {
                ListWFModel listTrabajadores = new ListWFModel
                {
                    Total = ListPersonal.Sum(x => x.Pay)
                };

                var listaCreada = App.Database.SaveListWF(listTrabajadores);
                listaCreada.Wait();

                listTrabajadores.PersonalxProduct = new List<PersonalModel>();

                foreach (var personal in ListPersonal)
                {
                    var saveresp = App.Database.SavePersonal(personal);
                    saveresp.Wait();

                    //personal.WorkForce = PerfilWF;
                    //personal.ListWF = listTrabajadores;
                    var rr = App.Database.UpdateRelationsPersonal(personal);
                    rr.Wait();

                    listTrabajadores.PersonalxProduct.Add(personal);
                    App.Database.UpdateListWF(listTrabajadores);

                    //CrearRolPagos(personal);

                    //Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?IdRM=0&IdWF={personal.Id}");
                }


                Shell.Current.GoToAsync($"//Rini/Productos/NewStock/CostoProduccion?ListMaterialId=0&IdWF={listTrabajadores.Id}&IdListaMaquinaria=0&CantProd=&NameProd=");

            }

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
                    Pay = wff.Pay;
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

using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReinadelCisne.Models;
using ReinadelCisne.Services;
using System.Web;

namespace ReinadelCisne.ViewModels
{
    public class LoginVM : BaseVM, IQueryAttributable
    {
        /*public string Usuario { get; set; }
        public string Contrasenia { get; set; }*/
        private string _ciUser;
        public string CiUser
        {
            get { return _ciUser; }
            set
            {
                _ciUser = value;
                OnPropertyChanged();
            }
        }

        private string _passwordUser;
        public string PasswordUser
        {
            get
            {
                return _passwordUser;
            }
            set
            {
                _passwordUser = value;
                OnPropertyChanged();
            }
        }
        public List<UserModel> _lista;
        public List<UserModel> Lista
        {
            get { return _lista; }
            set { _lista = value; OnPropertyChanged(); }
        }

        private bool _isRegiter;
        public bool IsRegister
        {
            get => _isRegiter;
            set
            {
                _isRegiter = value;
                OnPropertyChanged();
            }
        }
        public ICommand EntryCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand UsuarioButonnCommand { get; set; }
        public ICommand TecladoButonnCommand { get; set; }

        private bool _frame1 = false;
        public bool Frame1 
        {
            get => _frame1;
            set
            {
                _frame1 = value;
                OnPropertyChanged();
            }
        }
        private bool _frame2 = false;
        public bool Frame2
        {
            get => _frame2;
            set
            {
                _frame2 = value;
                OnPropertyChanged();
            }
        }
        public LoginVM()
        {
            IsRegister = true;
            EntryCommand = new Command(() => Entry());
            RegisterCommand = new Command(() => Register());
            UsuarioButonnCommand = new Command(() => UserButton());
            TecladoButonnCommand = new Command(() => TecladoButton());
        }

        private void TecladoButton()
        {
            Frame1 = false;
            Frame2 = true;
        }

        private void UserButton()
        {
            Frame1 = true;
            Frame2 = false;
        }

        private async void Entry()
        {
            RulesValidation validar = new RulesValidation();
            int val = validar.ValidarCiPassword(CiUser, PasswordUser);

            if (val == 1)
            {
                var j = App.Database.ValidarUsuario(CiUser, PasswordUser);

                if (j == true)
                {
                    App.Current.Properties["IsLoggedIn"] = true;
                    App.Current.Properties["UserLogged"] = CiUser;
                    App.Current.Properties["PassLoged"] = PasswordUser;
                    await Application.Current.MainPage.DisplayAlert("Bienvenido",
                                                                    "Yintu le da la Bienvenida",
                                                                    "Aceptar");
                    await Shell.Current.GoToAsync($"//RGo");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Losiento",
                                                                    "Sus datos no se encuentran registrados en nuestro sistema",
                                                                    "Aceptar");
                }
            }
            if (val == 2)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Usuario o Contraseña incompletos",
                    "Ok");
            }
            if (val == 3)
            {
                await Shell.Current.DisplayAlert("Error",
                    "El numero de usuario es incorrecto",
                    "Aceptar");
            }
        }

        private async void Register()
        {
            await Shell.Current.GoToAsync("RegisterUser");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                string idListRM = HttpUtility.UrlDecode(query["IsRegister"]);

                if (idListRM != "0")
                {
                    IsRegister = false;
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load idproduct.");
            }
        }
    }
}

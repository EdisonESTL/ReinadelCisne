using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReinadelCisne.Services;
using ReinadelCisne.Models;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using System.Threading;
using SkiaSharp;
using System.Threading.Tasks;

namespace ReinadelCisne.ViewModels
{
    public class AjustesVM : BaseVM
    {
        private Image _imageU;
        public Image ImageU
        {
            get => _imageU;
            set
            {
                if(_imageU != value)
                {
                    _imageU = value;
                    OnPropertyChanged();
                    change = true;
                }
            }
        }
        bool change = false;
        bool changePassword = false;

        private UserModel _userRegister;
        public UserModel UserRegister
        {
            get => _userRegister;
            set
            {
                if(_userRegister != value)
                {
                    _userRegister = value;
                    OnPropertyChanged();
                    changePassword = true;
                }
            }
        }
        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if(_newPassword != value)
                {
                    _newPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _newPasswordClone;
        public string NewPasswordClone
        {
            get => _newPasswordClone;
            set
            {
                if (_newPasswordClone != value)
                {
                    _newPasswordClone = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _newPin;
        public string NewPin
        {
            get => _newPin;
            set
            {
                if (_newPin != value)
                {
                    _newPin = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SelectPhoto => new Command(async () =>
        {
           var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
           {
               Title = "selecciona una foto"
           });

           if(photo != null)
            {
                var getPhotoStream = await photo.OpenReadAsync();
                ImageU.Source = ImageSource.FromStream(() => getPhotoStream);

                var transformPhoto = await photo.OpenReadAsync();
                TranformImageByte(transformPhoto);
            }
        });
        private byte[] bytes;
        private async void TranformImageByte(Stream imageSource)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageSource.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }
        }

        public ICommand Update => new Command(() =>
        {
            UpdateUser();
        });
        
        public AjustesVM() 
        {
            ImageU = new Image();

            ChargeUser();
        }

        public void ChargeUser()
        {
            UserRegister = App.Database.GetUser().Result;
            var imageUser = App.Database.GetImageUser(UserRegister).Result;

            try
            {
                
                MemoryStream stream = new MemoryStream(imageUser.Image);
                ImageU.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(imageUser.Image);
                });
                Console.WriteLine("debo carga la imagen");
            }
            catch(Exception exe)
            {
                Console.WriteLine(exe.Message);
            }
        }
        
        private async void UpdateUser()
        {
            if (change)
            {
                
                var ss = await App.Database.SaveUser(UserRegister);

                var newImage = new ImagesAppModel();
                newImage.IdForeing = UserRegister.Id;
                newImage.NameForeing = "UserModel";
                newImage.Image = bytes;

                var aux = await App.Database.SaveImageApp(newImage);

                if(change != changePassword)
                {
                    await Shell.Current.DisplayAlert("Exito",
                                            "Usuario actualizado",
                                            "Ok");
                }
                

            }
            if (!string.IsNullOrEmpty(NewPassword) && changePassword)
            {
                if(NewPassword == NewPasswordClone)
                {
                    UserRegister.PasswordUser = NewPassword;
                    await App.Database.SaveUser(UserRegister);
                    await Shell.Current.DisplayAlert("Éxito",
                            "Usuario actualizado",
                            "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error",
                            "Las contraseñas no coinciden",
                            "Ok");
                }
            }

            if (!string.IsNullOrEmpty(NewPin))
            {
                UserRegister.PinUser = NewPin;
                await App.Database.SaveUser(UserRegister);
                await Shell.Current.DisplayAlert("Exito",
                        "Usuario actualizado",
                        "Ok");
            }
            ChargeUser();
        }


    }
}

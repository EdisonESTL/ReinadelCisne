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
                    change = true;
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

        private UserPhotosModel _imageUser;
        public UserPhotosModel ImageUser
        {
            get => _imageUser;
            set
            {
                if(_imageUser != value)
                {
                    _imageUser = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<UserPhotosModel> Fotos { get; set; } = new ObservableCollection<UserPhotosModel>();
        private int num = 1;
        ImagesAppModel imageApp = new ImagesAppModel();
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

            //ImageU = new Image();

            try
            {
                
                MemoryStream stream = new MemoryStream(imageUser.Image);
                //stream.Write(imageUser.Image, 0, imageUser.Image.Length);
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
                /*
                imageApp.IdForeing = UserRegister.Id;
                imageApp.NameForeing = "UserModel";
                imageApp.Image = bytes;
                */
                var aux = await App.Database.SaveImageApp(newImage);
                await Shell.Current.DisplayAlert("Exito",
                        "Usuario actualizado",
                        "Ok1");
                ChargeUser();

            }
            if (!string.IsNullOrEmpty(NewPassword))
            {
                UserRegister.PasswordUser = NewPassword;
                await App.Database.SaveUser(UserRegister);
                await Shell.Current.DisplayAlert("Exito",
                        "Usuario actualziado",
                        "Ok2");
                ChargeUser();

            }
            if (!string.IsNullOrEmpty(NewPin))
            {
                UserRegister.PinUser = NewPin;
                await App.Database.SaveUser(UserRegister);
                await Shell.Current.DisplayAlert("Exito",
                        "Usuario actualziado",
                        "Ok3");
                ChargeUser();

            }
        }


    }
}

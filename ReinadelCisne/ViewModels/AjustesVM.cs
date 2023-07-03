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

namespace ReinadelCisne.ViewModels
{
    public class AjustesVM : BaseVM
    {
        private Image _image;
        public Image Image
        {
            get => _image;
            set
            {
                if(_image != value)
                {
                    _image = value;
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
        public ICommand PhotoUpdate => new Command(async () =>
        {
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            var stremresp = stream;

            var imageUser = new UserPhotosModel();
            imageUser.Image.Source = ImageSource.FromStream(() => stream);
            imageUser.Name = "Image" + num;
            num++;
            Image = imageUser.Image;  
            TranformImageByte(stremresp);

        });
        public byte[] bytes;
        private async void TranformImageByte(Stream imageSource)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageSource.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();

                // Ahora tienes el arreglo de bytes (byte[]) de la imagen
                // Puedes utilizarlo según tus necesidades
            }
        }

        public ICommand Update => new Command(() =>
        {
            /*await Shell.Current.DisplayAlert("si aca",
                        "Usuario actualziado",
                        "Ok1");*/
            UpdateUser();
        });
        /*public ICommand PhotoUpdate2 => new Command(async () =>
        {
            var mediaPicker = DependencyService.Get<IMediaPicker>();
            var file = await mediaPicker.SelectPhotoAsync();
            Image = ImageSource.FromFile(file.Path);
        });*/
        //public Stream stream;
        public AjustesVM()
        {
            Image = new Image();

            ChargeUser();
        }

        private void ChargeUser()
        {
            var user = App.Database.GetUser();
            UserRegister = user.Result;
            try
            {
                var imageUser = App.Database.GetImageUser(user.Result);
                ImageUser = imageUser.Result;
            }
            catch(Exception exe)
            {
                Console.WriteLine(exe.Message);
            }
        }
        /*public async byte[] imageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var stream = new MemoryStream();
                var imageSource = (StreamImageSource)image.Source;

                using (Stream imageStream = await imageSource.Stream(System.Threading.CancellationToken.None))
                {
                    await imageStream.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }
        }*/
        private async void UpdateUser()
        {
            if (change)
            {
                //Guardo registro
                var ss = await App.Database.SaveUser(UserRegister);
                //Conseguir foto
                var streamImage  =DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
                //Guardo foto
                imageApp.IdForeing = UserRegister.Id;
                imageApp.NameForeing = "UserModel";

                /*
                 */
                
                /*using (MemoryStream ms = new MemoryStream())
                {
                    var stream = new MemoryStream();
                    var imageSource = (StreamImageSource)Image.Source;

                    using (Stream imageStream = await imageSource.Stream(System.Threading.CancellationToken.None))
                    {
                        await imageStream.CopyToAsync(ms);
                        imageApp.Image = ms.ToArray();
                    }
                }
                */
                //imageApp.Image = ImageSource.FromStream(() => new MemoryStream())
                await App.Database.SaveImageApp(imageApp);
                await Shell.Current.DisplayAlert("Exito",
                        "Usuario actualizado",
                        "Ok1");
                var use2r = App.Database.GetUser();
                var UserRegister2 = use2r.Result;
            }
            if (!string.IsNullOrEmpty(NewPassword))
            {
                UserRegister.PasswordUser = NewPassword;
                await App.Database.SaveUser(UserRegister);
                await Shell.Current.DisplayAlert("Exito",
                        "Usuario actualziado",
                        "Ok2");
            }
            if(!string.IsNullOrEmpty(NewPin))
            {
                UserRegister.PinUser = NewPin;
                await App.Database.SaveUser(UserRegister);
                await Shell.Current.DisplayAlert("Exito",
                        "Usuario actualziado",
                        "Ok3");
            }
        }
    
        
    }
}

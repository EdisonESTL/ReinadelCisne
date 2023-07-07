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
            var getPhotoStream = await photo.OpenReadAsync();
            ImageU.Source = ImageSource.FromStream(() => getPhotoStream);
            ImageU = ImageU;
            await LoadPhotoAsynx(photo);

        });

        byte[] BytesImageUSer = new byte[10];
        async Task LoadPhotoAsynx(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                ImageU.Source = null;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
            {
                await stream.CopyToAsync(newStream);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    //funciona
                    //Sacar de la funcion imageBytes para poder guardar
                }
            }
                
            //ImageU = newFile;
        }

        public ICommand PhotoUpdate => new Command(async () =>
        {
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();

            if(stream != null)
            {
                var imageUser = new UserPhotosModel();
                imageUser.Image.Source = ImageSource.FromStream(() => stream);
                imageUser.Name = "Image" + num;
                num++;
                ImageUser = imageUser;
                using (StreamReader writer = new StreamReader(stream))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        ImageUser.ByteImage = imageBytes;
                    }
                }
                
            }

            /*
            using (StreamReader writer = new StreamReader(stream))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    ImageUser.ByteImage = imageBytes;
                }
            }*/

            //Image = imageUser.Image;

            /*Image image = new Image();
            using (StreamWriter writer = new StreamWriter(stream))
{
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    imageApp.Image = imageBytes;
                    image.Source = ImageSource.FromStream(() => memoryStream);
                }

            }*/

        });
        public byte[] bytes;
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
            ImageU = new Image();

            ChargeUser();
        }

        private void ChargeUser()
        {
            var user = App.Database.GetUser();
            UserRegister = user.Result;
            try
            {
                Console.WriteLine("debo carga la imagen");
                /*var imageUser = App.Database.GetImageUser(user.Result);
                ImageUser = imageUser.Result;*/
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
                //var streamImage  = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
                //Guardo foto
                imageApp.IdForeing = UserRegister.Id;
                imageApp.NameForeing = "UserModel";

                

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
        }


    }
}

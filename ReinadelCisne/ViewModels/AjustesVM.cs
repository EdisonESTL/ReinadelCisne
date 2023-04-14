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
                }
            }
        }

        public ObservableCollection<UserPhotosModel> Fotos { get; set; } = new ObservableCollection<UserPhotosModel>();
        private int num = 1;
        public ICommand PhotoUpdate => new Command(async () =>
        {
            
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();

            if(stream != null)
            {
                UserPhotosModel image = new UserPhotosModel();

                image.Image.Source = ImageSource.FromStream(() => stream);

                image.Name = "Image" + num;
                num++;
                Image = image.Image;
                //Fotos.Add(image);
            }
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

            
        }
    }
}

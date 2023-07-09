using ReinadelCisne.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReinadelCisne.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReinadelCisne.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AjustesView : ContentPage
    {
        public AjustesView()
        {
            InitializeComponent();
        }
        async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                image.Source = ImageSource.FromStream(() => stream);
            }

            (sender as Button).IsEnabled = true;
        }

        /*protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AjustesVM viewModel)
            {
                viewModel.ChargeUser(); // Llamar al método Init() en el ViewModel
            }
        }*/

    }
}
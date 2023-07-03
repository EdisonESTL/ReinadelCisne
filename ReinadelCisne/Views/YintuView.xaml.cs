using ReinadelCisne.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReinadelCisne.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YintuView : ContentPage
    {
        public YintuView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is YintuVM viewModel)
            {
                viewModel.ChargeUser(); // Llamar al método Init() en el ViewModel
            }
        }
    }
}
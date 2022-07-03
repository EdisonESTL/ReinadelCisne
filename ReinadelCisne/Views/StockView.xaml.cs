using ReinadelCisne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReinadelCisne.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReinadelCisne.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockView : ContentPage
    {
        public StockView()
        {
            InitializeComponent();
            //BindingContext = new StockpsVM();
        }
    }
}
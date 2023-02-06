using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReinadelCisne.Views
{
    //[QueryProperty(nameof(ListMaterialId), nameof(ListMaterialId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CostosView : ContentPage
    {        
        //public string ListMaterialId { get; set; }
        public CostosView()
        {
            InitializeComponent();
        }
        /*protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(ListMaterialId, out var result);

            BindingContext = await App.Database.GetListRM(result);
        }*/
    }
}
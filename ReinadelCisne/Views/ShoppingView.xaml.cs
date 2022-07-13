using dotMorten.Xamarin.Forms;
using ReinadelCisne.ViewModels;
using ReinadelCisne.Models;
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
    public partial class ShoppingView : ContentPage
    {
        public string Entyd
        {
            get => descf.Text;
            set
            {
                descf.Text = value;
                OnPropertyChanged();
            }
        }

        public string AuxId
        {
            get => IdAux.Text;
            set
            {
                IdAux.Text = value;
                OnPropertyChanged();
            }
        }
        public ShoppingView()
        {
            InitializeComponent();
        }
         
        async void Handle_SelectionChanged(object sender, Syncfusion.SfAutoComplete.XForms.SelectionChangedEventArgs e)
        {
            if(autoComplete.SelectedValue != null && autoComplete.SelectedValue.ToString() != "")
            {
                var gg = e.AddedItems as RawMaterialModel;                
                bool answ = await DisplayAlert("Pregunta", "¿Usar " + gg.UnitMeasurementRM + " como unidad de medida?", "Si", "No");
                if (answ)
                {
                    AuxId = gg.Id.ToString();
                    Entyd = gg.UnitMeasurementRM;
                    autoComplete.Text = gg.NameRM;
                }
            }
            
        }
        /*
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset                
                //sender.ItemsSource = dataset;
                var d = App.Database.GetMR().Result;
                var g = (from n in d
                         where n.NameRM.Contains(sender.Text)
                         select n.NameRM).ToList();
                sender.ItemsSource = g;
            }
        }
        

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            //sender.Text = args.SelectedItem.ToString();
            
        }


        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                sender.Text = args.ChosenSuggestion.ToString();
                
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }*/
    }
}
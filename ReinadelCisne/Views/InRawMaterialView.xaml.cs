using System;
using System.Collections.Generic;
using ReinadelCisne.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReinadelCisne.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InRawMaterialView : ContentPage
    {
        public string Medida
        {
            get => MedidaLabel.Text;
            set
            {
                MedidaLabel.Text = value;
                OnPropertyChanged();
            }
        }
        public string CostoU
        {
            get => CostoUlabel.Text;
            set
            {
                CostoUlabel.Text = value;
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
        public InRawMaterialView()
        {
            InitializeComponent();
        }
        async void Handle_SelectionChanged(object sender, Syncfusion.SfAutoComplete.XForms.SelectionChangedEventArgs e)
        {
            if (autoComplete.SelectedValue != null && autoComplete.SelectedValue.ToString() != "")
            {
                var gg = e.AddedItems as RawMaterialModel;
                bool answ = await DisplayAlert("Pregunta", "¿Usar " + gg.UnitMeasurementRM + " como unidad de medida?", "Si", "No");
                if (answ)
                {
                    AuxId = gg.Id.ToString();
                    Medida = gg.UnitMeasurementRM;
                    CostoU = gg.CostoRM.ToString("N2");
                    autoComplete.Text = gg.NameRM;
                }
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReinadelCisne.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReinadelCisne
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        void RegisterRoutes()
        {
            Routes.Add("StockRegistration", typeof(StockRegistrationView));
            Routes.Add("GoRegistration", typeof(GoRegistrationView));
            Routes.Add("InRawMaterial", typeof(InRawMaterialView));
            Routes.Add("InListRawMaterial", typeof(ListRawMaterialView));
            Routes.Add("InWorkForce", typeof(InWorkForce));
            Routes.Add("InOtherCost", typeof(InOtherCost));
            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}
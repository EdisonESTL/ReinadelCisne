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
            Routes.Add("NewStock", typeof(StockView));
            Routes.Add("GoRegistration", typeof(GoRegistrationView));
            Routes.Add("GoRegistrationDetail", typeof(GoRegistrationDetail));
            Routes.Add("GoRegistrationXcobrar", typeof(GoRegistartionXCobrarView));
            Routes.Add("InRawMaterial", typeof(InRawMaterialView));
            Routes.Add("InListRawMaterial", typeof(ListRawMaterialView));
            Routes.Add("InWorkForce", typeof(InWorkForce));
            Routes.Add("InOtherCost", typeof(InOtherCost));
            Routes.Add("NewShopping", typeof(ShoppingView));
            Routes.Add("ShoppingDetail", typeof(ShoppingDetailView));
            Routes.Add("ActivityRegistration", typeof(ActivityRegistrationView));
            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}
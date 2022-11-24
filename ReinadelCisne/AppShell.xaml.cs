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
            //Routes.Add("NewShopping", typeof(ShoppingView));
            Routes.Add("ShoppingDetail", typeof(ShoppingDetailView));
            Routes.Add("ActivityRegistration", typeof(ActivityRegistrationView));
            Routes.Add("RegisterUser", typeof(RegisterView));

            /**************************************/
            Routes.Add("//Rini/Productos", typeof(StockRegistrationView));
            Routes.Add("//Rini/Productos/NewStock", typeof(StockView));
            Routes.Add("//Rini/Productos/Kardex", typeof(KardexView));

            Routes.Add("//Rini/RServicios", typeof(ServicesView));
            Routes.Add("//Rini/RServicios/NewService", typeof(ServicesNewView));

            Routes.Add("//Rini/RVentas", typeof(GoRegistrationView)); 
            
            Routes.Add("//Rini/RCompras", typeof(RegistrationShoppingView));
            Routes.Add("//Rini/RCompras/SelectionPS", typeof(ShoppingProdServView));
            Routes.Add("//Rini/RCompras/SelectionPS/NewShopping", typeof(ShoppingView));

            Routes.Add("//Rini/RMateriaPrima", typeof(RawMaterial));
            Routes.Add("//Rini/RMateriaPrima/RCompras", typeof(RegistrationShoppingView));
            Routes.Add("//Rini/RMateriaPrima/NewMP", typeof(InRawMaterialView));

            Routes.Add("//Rini/ManoObra", typeof(InWorkForce));


            Routes.Add("//Rini/OtherCost", typeof(InOtherCost));


            Routes.Add("//Rini/RIngresoEgreso", typeof(IngresoEgresoView));
            
            Routes.Add("//Rini/RReportes", typeof(ReportesView));
            Routes.Add("//Rini/RVentanilla", typeof(GoView));
            Routes.Add("//Rini/RAjustes", typeof(AjustesView));
            foreach (KeyValuePair<string, Type> item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}
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

            #region Rutas Borrador
            //Routes.Add("NewStock", typeof(StockView));
            Routes.Add("GoRegistration", typeof(GoRegistrationView));
            Routes.Add("GoRegistrationDetail", typeof(GoRegistrationDetail));
            Routes.Add("GoRegistrationXcobrar", typeof(GoRegistartionXCobrarView));
            Routes.Add("InRawMaterial", typeof(InRawMaterialView));
            Routes.Add("InListRawMaterial", typeof(ListRawMaterialView));
            Routes.Add("InWorkForce", typeof(InWorkForce));
            Routes.Add("InOtherCost", typeof(InOtherCost));
            //Routes.Add("NewShopping", typeof(ShoppingView));
            
            Routes.Add("ActivityRegistration", typeof(ActivityRegistrationView));
            Routes.Add("RegisterUser", typeof(RegisterView));
            Routes.Add("//Rini/RIngresoEgreso", typeof(IngresoEgresoView));
            Routes.Add("//Rini/RReportes", typeof(ReportesView));
            Routes.Add("//Rini/RAjustes", typeof(AjustesView));
            Routes.Add("//Rini/RServicios/NewService", typeof(ServicesNewView));
            Routes.Add("//Rini/RVentas", typeof(GoRegistrationView)); 
            #endregion
            /**************************************/

            //Rutas de Productos terminados
            Routes.Add("//Rini/Productos", typeof(StockRegistrationView));
            Routes.Add("//Rini/Productos/Ventas", typeof(GoRegistrationView));
            Routes.Add("//Rini/Productos/Kardex", typeof(KardexView));
            Routes.Add("//Rini/Productos/NewStock", typeof(StockView));

            //Rutas costos producción
            Routes.Add("//Rini/Productos/NewStock/CostoProduccion", typeof(CostosView));
            Routes.Add("SelectRM", typeof(SelectRawMaterialView));
            Routes.Add("SelectWF", typeof(SelectWorkForceView));
            Routes.Add("SelectCIFView", typeof(SelectCIFView));
            Routes.Add("SelectMaquinariaView", typeof(SelectMaquinariaView));

            //Rutas de productos en proceso
            Routes.Add("//Rini/RProductosEnProceso", typeof(ServicesView));
            Routes.Add("//Rini/RProductosEnProceso/DetailPP", typeof(SaleDetailView));
            
            //Rutas de compras
            Routes.Add("//Rini/RCompras", typeof(RegistrationShoppingView));
            Routes.Add("//Rini/RCompras/SelectionPS", typeof(ShoppingProdServView));
            Routes.Add("//Rini/RCompras/SelectionPS/NewShopping", typeof(ShoppingView));

            //Rutas de materia prima
            Routes.Add("//Rini/RMateriaPrima", typeof(RawMaterial));
            Routes.Add("//Rini/RMateriaPrima/RCompras", typeof(RegistrationShoppingView));
            Routes.Add("//Rini/RMateriaPrima/RCompras/ShoppingDetail", typeof(ShoppingDetailView));
            Routes.Add("//Rini/RMateriaPrima/RCompras/SelectionRM", typeof(ShoppingProdServView));
            Routes.Add("//Rini/RMateriaPrima/RCompras/SelectionRM/NewShopping", typeof(ShoppingView));
            Routes.Add("//Rini/RMateriaPrima/NewMP", typeof(InRawMaterialView));
            Routes.Add("//Rini/RMateriaPrima/KardeRM", typeof(KardexRMView));

            //Rutas de mano de obra
            Routes.Add("//Rini/ManoObra", typeof(WorkForceView));
            Routes.Add("//Rini/ManoObra/NewManoObra", typeof(NewWorkForceView));
            Routes.Add("//Rini/ManoObra/DetailManoObra", typeof(DetailWorkForceView));

            //Rutas de costos indirectos de producción
            Routes.Add("//Rini/OtherCost", typeof(IndirectCostView));

            //Rutas de orden de pedido
            Routes.Add("//Rini/RVentanilla", typeof(GoView));
            Routes.Add("//Rini/RVentanilla/NuevoProducto", typeof(StockView));

            //Rutas costos de constitución
            Routes.Add("//Rini/CostosConstitucion", typeof(CostConstitucionView));

            //Rutas Assets
            Routes.Add("//Rini/Assets", typeof(FixedAssetsView));
            Routes.Add("NewAset", typeof(FixedAssetsNew));


            foreach (KeyValuePair<string, Type> item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}
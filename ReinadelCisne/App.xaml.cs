using ReinadelCisne.Services;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReinadelCisne
{
    public partial class App : Application
    {
        static DataBase database;

        public static DataBase Database
        {
            get
            {
                if (database == null)
                {
                    database = new DataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ReinadelCisne.db3"));
                }
                return database;
            }
        }
        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Njc0NzkwQDMyMzAyZTMyMmUzMGlZV0FtZ1l2N0xaTUFsUktJUmJMUlZNcXFXeUEzTUdwVjNhY1AzZ1E2VGM9");

            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

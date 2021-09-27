using System;
using System.IO;
using WaiveApp.Data;
using WaiveApp.REST;
using Xamarin.Forms;

namespace WaiveApp
{
    public partial class App : Application
    {
        //static RenewableShareDataDatabase database;
        public static RenewableShareManager RenewableShareManager { get; private set; }

        // Create the database connection as a singleton.
        //public static RenewableShareDataDatabase Database
        //{
        //    get
        //    {
        //        if (database == null)
        //        {
        //            database = new RenewableShareDataDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RenewableShareDataData.db3"));
        //        }
        //        return database;
        //    }
        //}
        
        public App()
        {
            InitializeComponent();
            RenewableShareManager = new RenewableShareManager(new RestService());
            MainPage = new NavigationPage(new MainPage());
            //MainPage = new AppShell(); according to the Quickstart for SQL Lite NET database
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

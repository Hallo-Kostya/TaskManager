
using App1.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Data;
using System.IO;

namespace App1
{
    public partial class App : Application
    {
        static AssignmentsDB assignmentsDB;
        public static AssignmentsDB AssignmentsDB
        {
            get
            {
                if (assignmentsDB == null)
                    assignmentsDB = new AssignmentsDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AssignmentsDatabase.db3"));
                return assignmentsDB;
            }
            
        }

        public App()
        {
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

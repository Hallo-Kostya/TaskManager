
using App1.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Data;
using System.IO;
using Xamarin.Essentials;
using App1.Views.StartingPages;

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

        public static bool IsNoTasksVisible { get; set; } = true;

        public App()
        {
            InitializeComponent();
            SetupMainPage();
        }

        private void SetupMainPage()
        {
            if (Preferences.Get("IsFirstLaunch", true))
            {
                MainPage = new NavigationPage(new NickEntryPage());
            }
            else
            {
                MainPage = new AppShell();
            }
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

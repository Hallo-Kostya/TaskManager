
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
        static ArchiveDB archiveDB;
        public static AssignmentsDB AssignmentsDB
        {
            get
            {
                if (assignmentsDB == null)
                    assignmentsDB = new AssignmentsDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AssignmentsDatabase.db3"));
                return assignmentsDB;
            }
            
        }
        public static ArchiveDB ArchiveDB
        {
            get
            {
                if (archiveDB == null)
                    archiveDB = new ArchiveDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ArchiveDatabase.db3"));
                return archiveDB;
            }

        }

        public static bool IsNoTasksVisible { get; set; } = true;

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

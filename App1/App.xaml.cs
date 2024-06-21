
using App1.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Data;
using System.IO;
using Xamarin.Essentials;
using App1.Views.StartingPages;
using App1.ViewModels.Settings;
using System.Threading.Tasks;

namespace App1
{
    public partial class App : Application
    {
        static AssignmentsDB assignmentsDB;
        public static ProfileViewModel ProfileViewModel { get; private set; }
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
            ProfileViewModel = new ProfileViewModel(null);
            SetupMainPage();
            CheckDailyActivity();
            CheckWeeklyReset();
        }

        public void SetupMainPage()
        {
            if (Preferences.Get("IsFirstLaunch", true))
            {
                MainPage = new NavigationPage(new NickEntryPage());
            }
            else
            {
                MainPage = new AppShell();
            }
            Console.WriteLine("Основная страница  запустилась");
        }
        private async void CheckDailyActivity()
        {
            
            var userId = Preferences.Get("CurrentUserId", -1);
            
            if (userId != -1)
            {
                var user = await App.AssignmentsDB.GetUserAsync(userId);
                var lastLaunchDate = user.LastLaunchDate;
                var currentDate = DateTime.Now.Date;
                if (lastLaunchDate == currentDate.AddDays(-1))
                {
                    user.DayStreak += 1;
                    user.LastLaunchDate = currentDate;
                    user.Exp += 20;
                }
                if (lastLaunchDate != DateTime.MinValue && lastLaunchDate < currentDate.AddDays(-1))
                {
                    user.DayStreak = 1;
                    user.LastLaunchDate = currentDate;
                }
                await App.AssignmentsDB.AddUserAsync(user);
            }
            Console.WriteLine("Дневная проверка запустилась");
           
        }

        private async void CheckWeeklyReset()
        {
            var userId = Preferences.Get("CurrentUserId", -1);
            var lastResetDate = Preferences.Get("LastWeeklyResetDate", DateTime.MinValue);
            var currentDate = DateTime.Now.Date;

            if (lastResetDate == DateTime.MinValue || (currentDate - lastResetDate).TotalDays >= 7 && userId!=-1)
            {
                var user = await App.AssignmentsDB.GetUserAsync(userId); 
                user.OverDueForWeek = 0;
                user.DoneForWeek = 0;
                await App.AssignmentsDB.AddUserAsync(user);
                Preferences.Set("LastWeeklyResetDate", currentDate);
            }
            Console.WriteLine("Недельная проверка запустилась");
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

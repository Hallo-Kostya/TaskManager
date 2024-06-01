using Xamarin.Forms;
using Xamarin.Essentials;
using App1.Services.ArchiveCleanup;
using System;

namespace App1.ViewModels
{
    public class APPSettingsViewModel : BaseAssignmentViewModel
    {
        public Command CancelSettingsCommand { get; }
        public Command SaveSettingsCommand { get; }
        public Command EnableArchiveCleaningCommand { get; }
        public Command SetCleanUpIntervalCommand { get; }
        public INavigation Navigation { get; set; }
        private int _cleaningInterval; // Интервал очистки в часах
        private bool _isArchiveCleaningEnabled;

        public int CleaningInterval
        {
            get => _cleaningInterval;
            set => SetProperty(ref _cleaningInterval, value);
        }
        public bool IsArchiveCleaningEnabled
        {
            get => _isArchiveCleaningEnabled;
            set
            {
                SetProperty(ref _isArchiveCleaningEnabled, value);
                Console.WriteLine("IsArchiveCleaningEnabled changed: " + _isArchiveCleaningEnabled);
            }
        }

        public APPSettingsViewModel(INavigation _navigation)
        {
            EnableArchiveCleaningCommand = new Command(EnableCleaning);
            Navigation = _navigation;
            SetCleanUpIntervalCommand = new Command<string>(SetCleanUpInterval);
            CleaningInterval = Preferences.Get("CleaningInterval", 24);
            IsArchiveCleaningEnabled = Preferences.Get("IsArchiveCleaningEnabled", false);
            SaveSettingsCommand = new Command(SaveSettings);
            CancelSettingsCommand = new Command(CancelSettings);
            Console.WriteLine("Initial IsArchiveCleaningEnabled: " + IsArchiveCleaningEnabled);
        }
        
        private void EnableCleaning()
        {
            IsArchiveCleaningEnabled = !IsArchiveCleaningEnabled;
        }
        private void SetCleanUpInterval(string interval)
        {
            var tempInterval = int.Parse(interval);
            if (tempInterval>0 && tempInterval < 746)
            {
                CleaningInterval = tempInterval;
            }
            
        }
        private async void CancelSettings()
        {
            await Navigation.PopAsync();
        }
        private async void SaveSettings()
        {
            Console.WriteLine("SaveSettings called. IsArchiveCleaningEnabled: " + IsArchiveCleaningEnabled);
            var scheduler = DependencyService.Get<IArchiveCleanupScheduler>();
            if (IsArchiveCleaningEnabled==true)
            {
                
                Preferences.Set("CleaningInterval", CleaningInterval);
                Preferences.Set("IsArchiveCleaningEnabled", IsArchiveCleaningEnabled);
                scheduler.ScheduleArchiveCleanup(CleaningInterval);
                Console.WriteLine("APPSettingsViewModel", "Scheduled archive cleanup with interval: " + CleaningInterval + " hours");
                Console.WriteLine(Preferences.Get("IsArchiveCleaningEnabled", false));
            }
            else 
            {
                Preferences.Set("IsArchiveCleaningEnabled", IsArchiveCleaningEnabled);
                scheduler.CancelArchiveCleanup();
            }  
            await Navigation.PopAsync();
        }
            
    }
}

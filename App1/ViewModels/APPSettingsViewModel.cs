using Xamarin.Forms;
using Xamarin.Essentials;
using App1.Services.ArchiveCleanup;
using System;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace App1.ViewModels
{
    public class APPSettingsViewModel : BaseAssignmentViewModel
    {
        public Command CancelSettingsCommand { get; }
        public Command CustomTimeCleanCommand { get; }
        public Command SaveSettingsCommand { get; }
        public Command EnableArchiveCleaningCommand { get; }
        public Command SetCleanUpIntervalCommand { get; }
        public INavigation Navigation { get; set; }
        private int _cleaningInterval; 
        private bool _isArchiveCleaningEnabled;
        private string _customTime;

        public int CleaningInterval
        {
            get => _cleaningInterval;
            set => SetProperty(ref _cleaningInterval, value);
        }
        private DateTime _nextCleanDate;
        public DateTime NextCleanDate
        {
            get => _nextCleanDate;
            set
            {
                if (_nextCleanDate != value)
                {
                    _nextCleanDate = value;
                    OnPropertyChanged(nameof(NextCleanDate));
                    
                }
            }
        }
        public string CustomTime
        {
            get => _customTime;
            set
            {
                SetProperty(ref _customTime, value);
            }
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
            CustomTimeCleanCommand = new Command(CustomClean);
            Console.WriteLine("Initial IsArchiveCleaningEnabled: " + IsArchiveCleaningEnabled);
            NextCleanDate = DateTime.Now.AddHours(CleaningInterval);
        }

        private void CustomClean()
        {
            var tempInterval = int.Parse(CustomTime);
            if (tempInterval>0 && tempInterval < 367)
            {
                CleaningInterval = tempInterval * 24;
            }
            NextCleanDate = DateTime.Now.AddHours(CleaningInterval);
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
            NextCleanDate = DateTime.Now.AddHours(CleaningInterval);    
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

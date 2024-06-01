using Xamarin.Forms;
using Xamarin.Essentials;
using App1.Services.ArchiveCleanup;

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
            set => SetProperty(ref _isArchiveCleaningEnabled, value);
        }

        public APPSettingsViewModel(INavigation _navigation)
        {
            EnableArchiveCleaningCommand = new Command(EnableCleaning);
            Navigation = _navigation;
            SetCleanUpIntervalCommand = new Command<string>(SetCleanUpInterval);
            CleaningInterval = Preferences.Get("CleaningInterval", 24);
            IsArchiveCleaningEnabled = Preferences.Get("IsArchiveCleaningEnabled", true);
            SaveSettingsCommand = new Command(SaveSettings);
            CancelSettingsCommand = new Command(CancelSettings);
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
            if (IsArchiveCleaningEnabled)
            {
                Preferences.Set("CleaningInterval", CleaningInterval);
                Preferences.Set("IsArchiveCleaningEnabled", IsArchiveCleaningEnabled);

                // Планирование задачи очистки
                var scheduler = DependencyService.Get<IArchiveCleanupScheduler>();
                scheduler.ScheduleArchiveCleanup(CleaningInterval);
            }
            await Navigation.PopAsync();
        }
    }
}

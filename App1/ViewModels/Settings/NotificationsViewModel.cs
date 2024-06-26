using App1.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;



namespace App1.ViewModels.Settings
{
    public class NotificationsViewModel : BaseAssignmentViewModel
    {
        public Command SetNotificationsSound { get; }
        public Command SetDailyReminderCommand { get; }
        public Command IsNotificationsCommand { get; }
        public Command IsRemindersCommand { get; }
        public Command OnCancelCommand { get; }
        public INavigation Navigation { get; set; }
        private bool _isNotifications;
        public bool IsNotifications
        {
            get => _isNotifications;
            set
            {
                if (_isNotifications != value)
                {
                    _isNotifications = value;
                    OnPropertyChanged(nameof(IsNotifications));
                }
            }
        }
        private TimeSpan _dailyRemindersTime;
        public TimeSpan DailyRemindersTime
        {
            get => _dailyRemindersTime;
            set
            {
                if (_dailyRemindersTime != value)
                {
                    _dailyRemindersTime = value;
                    OnPropertyChanged(nameof(DailyRemindersTime));
                    
                }
            }
        }
        private bool _isReminders;
        public bool IsReminders
        {
            get => _isReminders;
            set
            {
                if (_isReminders != value)
                {
                    _isReminders = value;
                    OnPropertyChanged(nameof(IsReminders));
                }
            }
        }
        private string _choosenSound;
        public string ChoosenSound
        {
            get => _choosenSound;
            set
            {
                if (_choosenSound != value)
                {
                    _choosenSound = value;
                    OnPropertyChanged(nameof(ChoosenSound));
                }
            }
        }
        public NotificationsViewModel(INavigation navigation)
        {
            IsRemindersCommand = new Command(OnReminders);
            IsReminders= Preferences.Get("AreRemindersEnabled", false);
            OnCancelCommand = new Command(OnCancel);
            Navigation = navigation;
            IsNotifications = Preferences.Get("AreNotificationsEnabled", false);
            ChoosenSound = Preferences.Get("NotificationSound", "sound1.mp3");
            string savedTimeString = Preferences.Get("DailyRemindersTime", "12:00"); // 12:00 - значение по умолчанию
            DailyRemindersTime = TimeSpan.ParseExact(savedTimeString, "hh\\:mm", null);
            IsNotificationsCommand = new Command(HandleNotifications);
            SetNotificationsSound = new Command<string>(SetSound);
            SetDailyReminderCommand = new Command(SetDailyTime);
        }
        private void SetDailyTime()
        {
            string timeString = DailyRemindersTime.ToString("hh\\:mm"); // Форматируем время в формат HH:mm
            Preferences.Set("DailyRemindersTime", timeString);
        }
        private void OnReminders()
        {
            IsReminders = !IsReminders;
            OnPropertyChanged(nameof(IsReminders));
            Preferences.Set("AreRemindersEnabled", IsReminders);
        }
        private void SetSound(string sound)
        {
            switch (sound)
            {
                case "zagadka.mp3":
                case "bodrost.mp3":
                case "spokoistvie.mp3":
                case "rassvet.mp3":
                case "melody.mp3":
                case "rezkost.mp3":
                case "kolokol.mp3":
                case "christmas.wav":
                case "alexander.wav":
                case "konstantin.wav":
                case "sound1.mp3":
                    Preferences.Set("NotificationSound", sound);
                    ChoosenSound = sound;
                    OnPropertyChanged(nameof(ChoosenSound));

                    var audioPlayer = DependencyService.Get<IAudioPlayer>();
                    if (audioPlayer != null)
                    {
                        audioPlayer.PlaySound(sound);
                    }
                    else
                    {
                        Console.WriteLine("IAudioPlayer not found.");
                    }
                    break;
            }
        }

        public async void OnCancel()
        {
            await Navigation.PopAsync();
        }
        private void HandleNotifications()
        {
            IsNotifications = !IsNotifications;
            OnPropertyChanged(nameof(IsNotifications));
            Preferences.Set("AreNotificationsEnabled", IsNotifications);
        }
    }
}

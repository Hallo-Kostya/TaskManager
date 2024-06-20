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
        public Command IsNotificationsCommand { get; }
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
            OnCancelCommand = new Command(OnCancel);
            Navigation = navigation;
            _isNotifications = Preferences.Get("AreNotificationsEnabled", false);
            _choosenSound = Preferences.Get("NotificationSound", "sound1.mp3");
            IsNotificationsCommand = new Command(HandleNotifications);
            SetNotificationsSound = new Command<string>(SetSound);
        }
        private void SetSound(string sound)
        {
            switch (sound)
            {
                case "zagadka.mp3":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound= sound;
                    break;
                case "bodrost.mp3":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "spokoistvie.mp3":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "rassvet.mp3":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "melody.mp3":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "rezkost.mp3":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "kolokol.mp3":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "christmas.wav":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "alexander.wav":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "konstantin.wav":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
                case "sound1.mp3":
                    Preferences.Set("NotificationSound", sound);
                    _choosenSound = sound;
                    break;
            }
        }
        public async void OnCancel()
        {
            await Navigation.PopAsync();
        }
        private void HandleNotifications()
        {
            _isNotifications = !_isNotifications;
            Preferences.Set("AreNotificationsEnabled", _isNotifications);
        }
    }
}

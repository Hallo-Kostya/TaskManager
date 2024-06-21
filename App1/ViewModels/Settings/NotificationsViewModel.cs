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
            IsNotifications = Preferences.Get("AreNotificationsEnabled", false);
            ChoosenSound = Preferences.Get("NotificationSound", "sound1.mp3");
            IsNotificationsCommand = new Command(HandleNotifications);
            SetNotificationsSound = new Command<string>(SetSound);
        }
        private void SetSound(string sound)
        {
            switch (sound)
            {
                case "zagadka":
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

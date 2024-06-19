using App1.Models;
using App1.Views;
using App1.Views.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public  class SettingsViewModel: BaseAssignmentViewModel
    {
        public Command OpenAllAppSettingsCommand { get; }
        public Command СreateTagCommand { get; }
        public Command CreateFolderCommand { get; }
        public Command NotificationsCommand { get; }
        public Command ProfileCommand { get; }
        public Command HelpCommand { get; }
        public Command SociaMediaCommand { get; }
        public Command MoreCommand { get; }
        public int UserId {  get; }
        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        public INavigation Navigation { get; set; }
        public SettingsViewModel(INavigation _navigation)
        {
            UserId = Preferences.Get("CurrentUserID", -1);
            OpenAllAppSettingsCommand = new Command(OpenAllAppSettings);
            СreateTagCommand = new Command(СreateTag);
            CreateFolderCommand = new Command(CreateFolder);
            NotificationsCommand = new Command(Notifications);
            ProfileCommand = new Command(Profile);
            HelpCommand = new Command(Help);
            SociaMediaCommand = new Command(SociaMedia);
            MoreCommand = new Command(More);
            Navigation = _navigation;
            Task.Run(async () => await LoadUser());
        }
        async Task LoadUser()
        {
            if (UserId != -1)
            {
                var user = await App.AssignmentsDB.GetUserAsync(UserId);
                User = user;
                OnPropertyChanged(nameof(User));
            }
                
        }
        public async void OpenAllAppSettings()
        {
            await Navigation.PushAsync(new APPSettingsPage());
        }

        public async void CreateFolder()
        {
            await Navigation.PushAsync(new CreateFolderPage());
        }
        public async void СreateTag()
        {
            await Navigation.PushAsync(new СreateTagPage());
        }
        public async void Notifications()
        {
            await Navigation.PushAsync(new NotificationsPage());
        }
        public async void Profile()
        {
            await Navigation.PushAsync(new ProfilePage());
        }
        public async void Help()
        {
            await Navigation.PushAsync(new HelpPage());
        }
        public async void SociaMedia()
        {
            await Navigation.PushAsync(new SociaMediaPage());
        }
        public async void More()
        {
            await Navigation.PushAsync(new MorePage());
        }
    }
}

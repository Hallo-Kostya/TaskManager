using App1.Views;
using App1.Views.Settings;
using System;
using System.Collections.Generic;
using System.Text;
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


        public INavigation Navigation { get; set; }
        public SettingsViewModel(INavigation _navigation)
        {
            OpenAllAppSettingsCommand = new Command(OpenAllAppSettings);
            СreateTagCommand = new Command(СreateTag);
            CreateFolderCommand = new Command(CreateFolder);
            NotificationsCommand = new Command(Notifications);
            ProfileCommand = new Command(Profile);
            HelpCommand = new Command(Help);
            SociaMediaCommand = new Command(SociaMedia);
            MoreCommand = new Command(More);
            Navigation = _navigation;
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

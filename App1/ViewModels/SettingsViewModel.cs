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

        public INavigation Navigation { get; set; }
        public SettingsViewModel(INavigation _navigation)
        {
            OpenAllAppSettingsCommand = new Command(OpenAllAppSettings);
            СreateTagCommand = new Command(СreateTag);
            Navigation = _navigation;
        }
        public async void OpenAllAppSettings()
        {
            await Navigation.PushAsync(new APPSettingsPage());
        }


        public async void СreateTag()
        {
            await Navigation.PushAsync(new СreateTagPage());
        }
    }
}

using App1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public  class SettingsViewModel: BaseAssignmentViewModel
    {
        public Command OpenAllAppSettingsCommand { get; }
        public Command СreateFolderCommand { get; }
        public Command СreateTagCommand { get; }

        public INavigation Navigation { get; set; }
        public SettingsViewModel(INavigation _navigation)
        {
            OpenAllAppSettingsCommand = new Command(OpenAllAppSettings);
            СreateFolderCommand = new Command(СreateFolder);
            СreateTagCommand = new Command(СreateTag);
            Navigation = _navigation;
        }
        public async void OpenAllAppSettings()
        {
            await Navigation.PushAsync(new APPSettingsPage());
        }

        public async void СreateFolder()
        {
            await Navigation.PushAsync(new Views.Settings.СreateFolderPage());
        }

        public async void СreateTag()
        {
            await Navigation.PushAsync(new Views.Settings.СreateTagPage());
        }
    }
}

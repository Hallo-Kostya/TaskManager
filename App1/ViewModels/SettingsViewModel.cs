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
        public INavigation Navigation { get; set; }
        public SettingsViewModel(INavigation _navigation)
        {
            OpenAllAppSettingsCommand = new Command(OpenAllAppSettings);
            Navigation = _navigation;
        }
        public async void OpenAllAppSettings()
        {
            await Navigation.PushAsync(new APPSettingsPage());
        }
    }
}

using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class NotificationPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public Command Notification1PopupCommand { get; }
        public Command Notification2PopupCommand { get; }
        public Command Notification3PopupCommand { get; }

        public NotificationPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Notification1PopupCommand = new Command(ExecuteNotification1Popup);
            Notification2PopupCommand = new Command(ExecuteNotification2Popup);
            Notification3PopupCommand = new Command(ExecuteNotification3Popup);
        }

        private async void ExecuteNotification1Popup()
        {
            await Navigation.PushPopupAsync(new Notification1PopupPage());
        }

        private async void ExecuteNotification2Popup()
        {
            await Navigation.PushPopupAsync(new Notification2PopupPage());
        }

        private async void ExecuteNotification3Popup()
        {
            await Navigation.PushPopupAsync(new Notification3PopupPage());
        }
    }
}

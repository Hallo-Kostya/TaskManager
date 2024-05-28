using App1.Models;
using App1.Services.Notifications;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace App1.ViewModels
{
    public class Notification1PopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        public Command SetNotificationTimeCommand { get; }

        public Notification1PopupViewModel(INavigation navigation)
        {
            Assignment = new AssignmentModel();
            Navigation = navigation;
            SetNotificationTimeCommand = new Command<string>(SetNotificationTime);
        }

        private  async void SetNotificationTime(string parameter)
        {
            var assign = Assignment;
            if (parameter == "none")
            {
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(assign, "NotificationSetted");
            }
            if (int.TryParse(parameter, out int minutes))
            {
                var newTime = Assignment.ExecutionDate.AddMinutes(minutes);
                Console.WriteLine(newTime);
                if ( newTime< DateTime.Now)
                {
                    return;
                }
                Assignment.NotificationTime = newTime;
                Assignment.HasNotification = true;
                Assignment.NotificationTimeMultiplier = minutes;
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(assign, "NotificationSetted");
            }
        }
    }
}

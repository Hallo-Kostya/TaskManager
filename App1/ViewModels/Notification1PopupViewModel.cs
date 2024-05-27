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
        private AssignmentModel _notificationTempAssignment;
        public AssignmentModel NotificationTempAssignment
        {
            get { return _notificationTempAssignment; }
            set { _notificationTempAssignment = value; OnPropertyChanged(); }
        }
        public Command SetNotificationTimeCommand { get; }

        public Notification1PopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
            SetNotificationTimeCommand = new Command<string>(SetNotificationTime);
        }

        private  async void SetNotificationTime(string parameter)
        {
            if (parameter == "none")
            {
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(NotificationTempAssignment, "NotificationSetted");
            }
            if (int.TryParse(parameter, out int minutes))
            {
                var newTime = NotificationTempAssignment.ExecutionDate.AddMinutes(minutes);
                Console.WriteLine(newTime);
                if ( newTime< DateTime.Now)
                {
                    return;
                }
                NotificationTempAssignment.NotificationTime = newTime;
                NotificationTempAssignment.HasNotification = true;
                NotificationTempAssignment.NotificationTimeMultiplier = minutes;
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(NotificationTempAssignment, "NotificationSetted");
            }
        }
    }
}

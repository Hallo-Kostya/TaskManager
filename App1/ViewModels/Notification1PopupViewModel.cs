using App1.Services.Notifications;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class Notification1PopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        INotificationManager notificationManager;
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }
        public Command DateSelectedCommand { get; }

        public Notification1PopupViewModel(INavigation navigation)
        {
            notificationManager = DependencyService.Get<INotificationManager>();
            Navigation = navigation;
            DateSelectedCommand = new Command<string>(DateChanged);
        }
        
        private async void DateChanged(string time)
        {
            string title = $"Уведомление!";
            string message = $"Ваш дедлайн приближается!";
            switch (time)
            {
                case "Нет":
                    break;
                case "Во время":
                    notificationManager.SendNotification(title, message, SelectedDate);
                    break;
                case "За 5 минут":
                    notificationManager.SendNotification(title, message, SelectedDate.AddMinutes(-5));
                    break;
                case "За 30 минут":
                    notificationManager.SendNotification(title, message, SelectedDate.AddMinutes(-30));
                    break;
                case "За 1 час":
                    notificationManager.SendNotification(title, message, SelectedDate.AddHours(-1));
                    break;
                case "За 1 день":
                    notificationManager.SendNotification(title, message, SelectedDate.AddDays(-1));
                    break;
                case "Произвольно":
                    break;
            }
            await Navigation.PopPopupAsync();
        }
    }
}

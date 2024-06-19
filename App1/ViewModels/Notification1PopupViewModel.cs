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
        public Command ConfirmCommand { get; }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    SelectedTime = _selectedDate.TimeOfDay;
                    OnPropertyChanged(nameof(SelectedTime));
                }
            }
        }
        private TimeSpan _selectedTime;
        public TimeSpan SelectedTime
        {
            get => _selectedTime;
            set
            {
                if (_selectedTime != value)
                {
                    _selectedTime = value;
                    OnPropertyChanged(nameof(SelectedTime));
                    // Обновление даты при изменении времени
                    SelectedDate = SelectedDate.Date + _selectedTime;
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }
        public Notification1PopupViewModel(INavigation navigation)
        {
            Assignment = new AssignmentModel();
            Navigation = navigation;
            SetNotificationTimeCommand = new Command<string>(SetNotificationTime);
            ConfirmCommand = new Command(OnConfirm);
            SelectedDate = Assignment.ExecutionDate;
            SelectedTime = Assignment.ExecutionDate.TimeOfDay;
        }

        private  async void SetNotificationTime(string parameter)
        {
            if (parameter == "none")
            {
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(Assignment, "NotificationSetted");
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
                MessagingCenter.Send(Assignment, "NotificationSetted");
            }
        }
        private async void OnConfirm()
        {
            if (SelectedDate>=DateTime.Now && SelectedDate <= Assignment.ExecutionDate)
            {
                Assignment.NotificationTime = SelectedDate;
                Assignment.HasNotification= true;
                Assignment.NotificationTimeMultiplier = 2;
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(Assignment, "NotificationSetted");
            }
            else
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Ошибка", "Дата уведомления не может быть позже чем дедлайн задачи", "OK");
            }
        }
    }
}

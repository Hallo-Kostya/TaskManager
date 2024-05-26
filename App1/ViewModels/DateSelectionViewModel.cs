using App1.Models;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class DateSelectionViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        private AssignmentModel _tempAssignment;
        public AssignmentModel TempAssignment
        {
            get { return _tempAssignment; }
            set { _tempAssignment = value; OnPropertyChanged(); }
        }
        public Command Notification1PopupCommand { get; }
        public Command Notification2PopupCommand { get; }
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
                    OnPropertyChanged();
                    // Обновление даты при изменении времени
                    SelectedDate = SelectedDate.Date + _selectedTime;
                }
            }
        }

        public DateSelectionViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Notification1PopupCommand = new Command(ExecuteNotification1Popup);
            Notification2PopupCommand = new Command(ExecuteNotification2Popup);
            SelectedDate = DateTime.Now;
            TempAssignment = new AssignmentModel();
        }

        private async void ExecuteNotification1Popup()
        {
            TempAssignment.ExecutionDate = SelectedDate;
            MessagingCenter.Unsubscribe<AssignmentModel>(this, "NotificationSetted");
            MessagingCenter.Subscribe<AssignmentModel>(this, "NotificationSetted",
                (sender) =>
                {
                    TempAssignment.NotificationTime = sender.NotificationTime;
                    TempAssignment.HasNotification = sender.HasNotification;
                });
            await Navigation.PushPopupAsync(new Notification1PopupPage(TempAssignment));
        }

        private async void ExecuteNotification2Popup()
        {
            //var assign = TempAssignment; если это окно будет отвечать за повторяющуюся\неповторяющуюся задачу
            await Navigation.PushPopupAsync(new Notification2PopupPage());
        }

        public  async Task  AcceptAndClose()
        {
            TempAssignment.ExecutionDate = SelectedDate;
            await Navigation.PopAsync();
            MessagingCenter.Send(TempAssignment, "DateChanged");
        }

    }
}

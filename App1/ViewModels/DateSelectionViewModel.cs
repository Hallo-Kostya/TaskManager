using App1.Models;
using App1.Views;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class DateSelectionViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        public Command Notification1PopupCommand { get; }
        public Command Notification2PopupCommand { get; }
        public Command OnBackPressedCommand { get; }
        public Command ConfirmCommand { get; }
        public bool IsFromPopup { get; set; }
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
            Assignment = new AssignmentModel();
            OnBackPressedCommand = new Command(OnBackPressed);
            ConfirmCommand = new Command(async ()=> await AcceptAndClose());
            SelectedDate = Assignment.ExecutionDate;
            SelectedTime = Assignment.ExecutionDate.TimeOfDay;
        }

        private async void ExecuteNotification1Popup()
        {
            Assignment.ExecutionDate = SelectedDate;
            MessagingCenter.Unsubscribe<AssignmentModel>(this, "NotificationSetted");
            MessagingCenter.Subscribe<AssignmentModel>(this, "NotificationSetted",
                (sender) =>
                {
                    Assignment.NotificationTime = sender.NotificationTime;
                    Assignment.HasNotification = sender.HasNotification;
                    Assignment.NotificationTimeMultiplier = sender.NotificationTimeMultiplier;
                });
            await Navigation.PushPopupAsync(new Notification1PopupPage(Assignment));
        }

        public async void OnBackPressed()
        {
            await Navigation.PopAsync();
            if (IsFromPopup)
            {
                var assign=new AssignmentModel();
                assign.Name= Assignment.Name;
                assign.Description= Assignment.Description;
                assign.FolderName= Assignment.FolderName;
                assign.ID= Assignment.ID;
                assign.IsCompleted= Assignment.IsCompleted;
                assign.Priority= Assignment.Priority;
                assign.Tag= Assignment.Tag;
                assign.TagColor= Assignment.TagColor;
                await Navigation.PushPopupAsync(new AssignmentAddingPage(assign));
            }
                
        }
        private async void ExecuteNotification2Popup()
        {
            //var assign = TempAssignment; если это окно будет отвечать за повторяющуюся\неповторяющуюся задачу
            await Navigation.PushPopupAsync(new Notification2PopupPage());
        }

        public  async Task  AcceptAndClose()
        {
            Assignment.ExecutionDate = SelectedDate;
            var assign = Assignment;
            await Navigation.PopAsync();
            if(IsFromPopup)
                await Navigation.PushPopupAsync(new AssignmentAddingPage(assign), false);
            else
                MessagingCenter.Send(assign, "Date1Changed");

        }

    }
}

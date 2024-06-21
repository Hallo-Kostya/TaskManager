using App1.Models;
using App1.Views;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        public string NotificationTimeString => Assignment.HasNotification
           ? Assignment.NotificationTime.ToString("dd.MM.yy HH:mm")
           : "Нет";
        public DateSelectionViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Notification1PopupCommand = new Command(ExecuteNotification1Popup);
            Notification2PopupCommand = new Command(ExecuteNotification2Popup);
            OnBackPressedCommand = new Command(OnBackPressed);
            ConfirmCommand = new Command(async ()=> await AcceptAndClose());
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
                    OnPropertyChanged(nameof(Assignment));
                    OnPropertyChanged(nameof(NotificationTimeString));
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
                assign.Tags= Assignment.Tags;
                assign.Childs = Assignment.Childs;
                assign.IsChild = Assignment.IsChild;
                assign.HasChild = Assignment.HasChild;
                await Navigation.PushPopupAsync(new AssignmentAddingPage(assign));
            }
                
        }
        private async void ExecuteNotification2Popup()
        {
            MessagingCenter.Unsubscribe<AssignmentModel>(this, "RepeatitionSetted");
            MessagingCenter.Subscribe<AssignmentModel>(this, "RepeatitionSetted",
                (sender) =>
                {
                    Assignment.IsRepeatable = sender.IsRepeatable;
                    Assignment.RepeatitionAdditional = sender.RepeatitionAdditional;
                    OnPropertyChanged(nameof(Assignment));
                });
            await Navigation.PushPopupAsync(new Notification2PopupPage(Assignment));

        }

        public  async Task  AcceptAndClose()
        {
            Assignment.ExecutionDate = SelectedDate;
            OnPropertyChanged(nameof(Assignment.ExecutionDate));
            var assign = Assignment;
            await Navigation.PopAsync();
            if(IsFromPopup)
                await Navigation.PushPopupAsync(new AssignmentAddingPage(assign), false);
            else
                MessagingCenter.Send(assign, "Date1Changed");

        }

    }
}

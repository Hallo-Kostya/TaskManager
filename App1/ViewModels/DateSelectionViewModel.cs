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
        public AssignmentModel TempAssignment { get; }
        public Command Notification1PopupCommand { get; }
        public Command Notification2PopupCommand { get; }
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
            var datetime = SelectedDate;
            await Navigation.PushPopupAsync(new Notification1PopupPage(datetime));
        }

        private async void ExecuteNotification2Popup()
        {
            //var assign = TempAssignment; если это окно будет отвечать за повторяющуюся\неповторяющуюся задачу
            await Navigation.PushPopupAsync(new Notification2PopupPage());
        }

        public  async Task  AcceptAndClose()
        {
            TempAssignment.ExecutionDate = SelectedDate;
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(TempAssignment, "DateChanged");
        }

    }
}

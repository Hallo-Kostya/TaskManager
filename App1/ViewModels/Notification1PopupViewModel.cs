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
            Navigation = navigation;
            DateSelectedCommand = new Command(DateChanged);
        }
       
        private async void DateChanged()
        {
            await Navigation.PopPopupAsync();
        }
    }
}

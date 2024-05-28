using App1.Models;
using App1.Services.Notifications;
using App1.ViewModels;
using App1.Views.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        INotificationManager notificationManager;
        public EditPage()
        {
            InitializeComponent();
            BindingContext = new EditViewModel(Navigation);
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        public EditPage(AssignmentModel assignment)
        {
            InitializeComponent();
            BindingContext = new EditViewModel(Navigation);
            if (assignment != null )
            {
                ((EditViewModel)BindingContext).Assignment=assignment;
            }
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }

        private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            var assign = ((EditViewModel)BindingContext).Assignment;
            if (assign.HasNotification)
            {
                string title = $"Уведомление!";
                string message = $"Ваш дедлайн по задаче {assign.Name} приближается!";
                notificationManager.CancelNotification(assign.ID);
                notificationManager.SendNotification(title, message, assign.NotificationTime, assign.ID);
            }
        }
        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
                notificationTest.Children.Add(msg);
            });
        }
        private void DatePickerDate_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}
using App1.Models;
using App1.Services.Notifications;
using App1.ViewModels;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
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
        private EditViewModel viewModel;
        public EditPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new EditViewModel(Navigation);
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        public EditPage(AssignmentModel assignment,bool _isFromPopup)
        {
            InitializeComponent();
            BindingContext = viewModel = new EditViewModel(Navigation);
            if (assignment != null )
            {
                ((EditViewModel)BindingContext).Assignment=assignment;
                ((EditViewModel)BindingContext).isFromPopup = _isFromPopup;
            }
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }

        private void ButtonSave_Clicked(object sender, EventArgs e)
            {
            var assign = ((EditViewModel)BindingContext).Assignment;
            if (assign.HasNotification)
            {
                string tags = string.Join(", ", assign.Tags.Select(tag => $"#{tag}"));
                if (assign.HasChild)
                {
                    string title = $"Уведомление! {tags}";
                    string message = $"Ваш дедлайн по задаче:{assign.Name} приближается!\n{assign.Description}\nНе забудьте сделать её до:{assign.ExecutionDate}";
                    notificationManager.CancelNotification(assign.ID);
                    notificationManager.SendNotification(title, message, assign.NotificationTime, assign.ID);
                }
                else
                {
                    string title = $"Уведомление! {tags}";
                    string message = $"Ваш дедлайн по задаче:{assign.Name} приближается!\n{assign.Description}\nНе забудьте сделать её до:{assign.ExecutionDate}\nТакже не забудьте про подзадачи!";
                    notificationManager.CancelNotification(assign.ID);
                    notificationManager.SendNotification(title, message, assign.NotificationTime, assign.ID);
                }
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
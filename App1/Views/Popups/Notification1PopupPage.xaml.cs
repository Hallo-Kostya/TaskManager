using App1.Services.Notifications;
using App1.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace App1.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notification1PopupPage : PopupPage
    {
        INotificationManager notificationManager;
        public DateTime SelectedDate;
        public Notification1PopupPage()
        {
            InitializeComponent();
            BindingContext = new Notification1PopupViewModel(Navigation);
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        public Notification1PopupPage(DateTime dateTime)
        {
            InitializeComponent();
            BindingContext = new Notification1PopupViewModel(Navigation);
            if (dateTime != null)
            {
                SelectedDate = dateTime;
                (BindingContext as Notification1PopupViewModel).SelectedDate=dateTime;
            }
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        private void Button1_Clicked(object sender, EventArgs e)
        {
            
        }
        private void Button2_Clicked(object sender, EventArgs e)
        {
            
            string title = $"Уведомление!";
            string message = $"Ваш дедлайн приближается!";
            notificationManager.SendNotification(title, message, SelectedDate);
        }
        private void Button3_Clicked(object sender, EventArgs e)
        {
            string title = $"Уведомление!";
            string message = $"Ваш дедлайн приближается!";
            notificationManager.SendNotification(title, message, SelectedDate.AddMinutes(-5));
        }
        private void Button4_Clicked(object sender, EventArgs e)
        {
            string title = $"Уведомление!";
            string message = $"Ваш дедлайн приближается!";
            Console.WriteLine($"Notification Time: {SelectedDate.AddMinutes(-30)}");
            notificationManager.SendNotification(title, message, SelectedDate.AddMinutes(-30));
        }
        private void Button5_Clicked(object sender, EventArgs e)
        {
            string title = $"Уведомление!";
            string message = $"Ваш дедлайн приближается!";
            notificationManager.SendNotification(title, message, SelectedDate.AddHours(-1));
        }
        private void Button6_Clicked(object sender, EventArgs e)
        {
            string title = $"Уведомление!";
            string message = $"Ваш дедлайн приближается!";
            notificationManager.SendNotification(title, message, SelectedDate.AddDays(-1));
        }
        private void Button7_Clicked(object sender, EventArgs e)
        {
            string title = $"Уведомление!";
            string message = $"Ваш дедлайн приближается!";
            notificationManager.SendNotification(title, message, DateTime.Now.AddSeconds(15));
        }
        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
                Layouter.Children.Add(msg);
            });
        }
    }
}
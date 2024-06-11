﻿using App1.Models;
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

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssignmentAddingPage : PopupPage
    {
        INotificationManager notificationManager;
        public AssignmentModel Assignment { get; set; }
        public AssignmentAddingPage()
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        public AssignmentAddingPage(ListModel folder)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            if (folder != null)
            {
                ((AssignmentAddingViewModel)BindingContext).Assignment.FolderName = folder.Name;
            }
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        public AssignmentAddingPage(AssignmentModel assignment)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            if (assignment != null)
            {
                ((AssignmentAddingViewModel)BindingContext).Assignment = assignment;
            }
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        public AssignmentAddingPage(bool _isChildAssignment)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            ((AssignmentAddingViewModel)BindingContext).IsChildAssignment = _isChildAssignment;
            if (_isChildAssignment == true)
            {
                tags.IsVisible = false;
                folders.IsVisible = false;
                TagsList.IsVisible = false;
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
            var assign = ((AssignmentAddingViewModel)BindingContext).Assignment;
            if (assign.HasNotification)
            {
                string title = $"Уведомление!";
                string message = $"Ваш дедлайн по задаче {assign.Name} приближается!";
                notificationManager.CancelNotification(assign.ID);
                notificationManager.SendNotification(title, message, assign.NotificationTime,assign.ID);
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
                NotificationsStack.Children.Add(msg);
            });
        }
    }
}

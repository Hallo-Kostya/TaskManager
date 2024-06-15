using App1.Models;
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
            CustomEntry.IsVisible = false;
        }
        public Notification1PopupPage(AssignmentModel assign)
        {
            InitializeComponent();
            BindingContext = new Notification1PopupViewModel(Navigation);
            if (assign != null)
            {
                (BindingContext as Notification1PopupViewModel).Assignment= assign;
            }
            if (assign.HasNotification==true && assign.NotificationTimeMultiplier == 1)
            {
                CustomEntry.IsVisible = true;
            }
            notificationManager = DependencyService.Get<INotificationManager>();
        }
        private void CancelNotification_Clicked(object sender, EventArgs e)
        {
            var assignment = (BindingContext as Notification1PopupViewModel).Assignment;
            if (assignment != null && assignment.HasNotification)
            {
                notificationManager.CancelNotification(assignment.ID);  // Отмена уведомления по идентификатору задачи
                (BindingContext as Notification1PopupViewModel).Assignment.HasNotification = false;  // Обновление состояния задачи
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CustomEntry.IsVisible = true;
        }
    }
}
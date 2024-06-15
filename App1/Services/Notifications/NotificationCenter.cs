using App1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace App1.Services.Notifications
{
    public class NotificationCenter
    {
        INotificationManager notificationManager;
        public NotificationCenter()
        {
            notificationManager = DependencyService.Get<INotificationManager>();
        }
        public void SendNotification(AssignmentModel assign)
        {
            string tags = string.Join(", ", assign.Tags.Select(tag => $"#{tag.Name}"));
            string title = $"Уведомление! {tags}";
            string message = $"Ваш дедлайн по задаче:{assign.Name} приближается!\nОписание:{assign.Description}\nНе забудьте сделать её до:{assign.ExecutionDate}";
            notificationManager.CancelNotification(assign.ID);
            notificationManager.SendExtendedNotification(title, message, assign.NotificationTime, assign.ID);
        }

        public void SendExtendedNotification(AssignmentModel assign)
        {
            string tags = string.Join(", ", assign.Tags.Select(tag => $"#{tag.Name}"));
            if (assign.HasChild)
            {
                string title = $"Уведомление! {tags}";
                string message = $"Ваш дедлайн по задаче:{assign.Name} приближается!\nОписание:{assign.Description}\nНе забудьте сделать её до:{assign.ExecutionDate}";
                notificationManager.CancelNotification(assign.ID);
                notificationManager.SendExtendedNotification(title, message, assign.NotificationTime, assign.ID);
            }
            else
            {
                string title = $"Уведомление! {tags}";
                string message = $"Ваш дедлайн по задаче:{assign.Name} приближается!\nОписание:{assign.Description}\nНе забудьте сделать её до:{assign.ExecutionDate}\nТакже не забудьте про подзадачи!";
                notificationManager.CancelNotification(assign.ID);
                notificationManager.SendExtendedNotification(title, message, assign.NotificationTime, assign.ID);
            }
        }

        public void CancelNotification(AssignmentModel assign)
        {
            notificationManager.CancelNotification(assign.ID);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Services.Notifications
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, DateTime? notifyTime = null, int id = -1);
        void ReceiveNotification(string title, string message);
        void CancelNotification(int id);
        void SendExtendedNotification(string title, string message, DateTime? notifyTime = null, int id = -1);
    }
}

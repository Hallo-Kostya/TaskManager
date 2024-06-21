using System;
using System.Collections.Generic;
using App1.iOS;
using App1.Services.Notifications;
using Foundation;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSNotificationManager))]
namespace App1.iOS
{
    public class iOSNotificationManager  
    {
        int messageId = -1;
        bool hasNotificationsPermission;
        public event EventHandler NotificationReceived;

        public static iOSNotificationManager Instance { get; private set; }

        public iOSNotificationManager()
        {
            Instance = this;
            Initialize();
        }

        public void Initialize()
        {
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
            {
                hasNotificationsPermission = granted;
            });

            UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver(this);
        }

        public void SendExtendedNotification(string title, string message, DateTime? notifyTime = null, int id = -1)
        {
            if (!hasNotificationsPermission)
            {
                return;
            }

            messageId = id != -1 ? id : messageId++;

            var content = new UNMutableNotificationContent
            {
                Title = title,
                Body = message,
                Sound = UNNotificationSound.Default
            };

            // Create a trigger for the notification
            UNNotificationTrigger trigger;
            if (notifyTime != null)
            {
                var triggerDate = notifyTime.Value.ToLocalTime();
                var dateComponents = new NSDateComponents
                {
                    Year = triggerDate.Year,
                    Month = triggerDate.Month,
                    Day = triggerDate.Day,
                    Hour = triggerDate.Hour,
                    Minute = triggerDate.Minute,
                    Second = triggerDate.Second
                };
                trigger = UNCalendarNotificationTrigger.CreateTrigger(dateComponents, false);
            }
            else
            {
                trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            }

            var request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    Console.WriteLine($"Failed to schedule notification: {err}");
                }
            });
        }
        public void SendNotification(string title, string message, DateTime? notifyTime = null, int id = -1)
        {
            if (!hasNotificationsPermission)
            {
                return;
            }

            messageId = id != -1 ? id : messageId++;

            var content = new UNMutableNotificationContent
            {
                Title = title,
                Body = message,
                Sound = UNNotificationSound.Default
            };

            UNNotificationTrigger trigger;
            if (notifyTime != null)
            {
                var triggerDate = notifyTime.Value.ToLocalTime();
                var dateComponents = new NSDateComponents
                {
                    Year = triggerDate.Year,
                    Month = triggerDate.Month,
                    Day = triggerDate.Day,
                    Hour = triggerDate.Hour,
                    Minute = triggerDate.Minute,
                    Second = triggerDate.Second
                };
                trigger = UNCalendarNotificationTrigger.CreateTrigger(dateComponents, false);
            }
            else
            {
                trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            }

            var request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    Console.WriteLine($"Failed to schedule notification: {err}");
                }
            });
        }

        public void CancelNotification(int id)
        {
            UNUserNotificationCenter.Current.RemovePendingNotificationRequests(new string[] { id.ToString() });
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs
            {
                Title = title,
                Message = message
            };
            NotificationReceived?.Invoke(null, args);
        }
    }
}

using App1.iOS;
using System;
using UserNotifications;

public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
{
    iOSNotificationManager notificationManager;

    public iOSNotificationReceiver(iOSNotificationManager manager)
    {
        notificationManager = manager;
    }

    public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        var title = notification.Request.Content.Title;
        var message = notification.Request.Content.Body;
        notificationManager.ReceiveNotification(title, message);
        completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
    }
}
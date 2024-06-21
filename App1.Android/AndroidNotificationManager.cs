using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using App1.Droid;
using App1.Services.Notifications;
using Xamarin.Essentials;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(LocalNotifications.Droid.AndroidNotificationManager))]
namespace LocalNotifications.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static AndroidNotificationManager Instance { get; private set; }

        public AndroidNotificationManager() => Initialize();

        public void Initialize()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }
        
        //public void Show(string title, string message, int id = -1)
        //{
        //    Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
        //    intent.PutExtra(TitleKey, title);
        //    intent.PutExtra(MessageKey, message);
        //    int notificationId = id != -1 ? id : pendingIntentId++;

        //    PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, notificationId, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

        //    NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
        //        .SetContentIntent(pendingIntent)
        //        .SetContentTitle(title)
        //        .SetContentText(message)
        //        .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.alarm))
        //        .SetSmallIcon(Resource.Drawable.alarm)
        //        .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

        //    Notification notification = builder.Build();
        //    manager.Notify(notificationId, notification);
        //}
        
        public void SendExtendedNotification(string title, string message, DateTime? notifyTime = null, int id = -1)
        {
            if (!Preferences.Get("AreNotificationsEnabled", true))
            {
                return;
            }
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            if (notifyTime != null)
            {
                Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
                intent.PutExtra(TitleKey, title);
                intent.PutExtra(MessageKey, message);
                int notificationId = id != -1 ? id : pendingIntentId++;

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, notificationId, intent, PendingIntentFlags.CancelCurrent | PendingIntentFlags.Immutable);
                long triggerTime = GetNotifyTime(notifyTime.Value);
                AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                ShowExtendedNotification(title, message, id);
            }
        }

        public void ShowExtendedNotification(string title, string message, int id = -1)
        {
            if (!Preferences.Get("AreNotificationsEnabled", true))
            {
                return;
            }
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            int notificationId = id != -1 ? id : pendingIntentId++;

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, notificationId, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            string soundFileName = Preferences.Get("NotificationSound", "sound1.mp3");
            int soundResourceId = AndroidApp.Context.Resources.GetIdentifier(System.IO.Path.GetFileNameWithoutExtension(soundFileName), "raw", AndroidApp.Context.PackageName);
            Android.Net.Uri soundUri = new Android.Net.Uri.Builder()
                .Scheme(ContentResolver.SchemeAndroidResource)
                .Authority(AndroidApp.Context.PackageName)
                .AppendPath(soundResourceId.ToString())
                .Build();

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                 .SetContentIntent(pendingIntent)
                 .SetContentTitle(title)
                 .SetContentText(message)
                 .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, App1.Droid.Resource.Drawable.alarm))
                 .SetSmallIcon(App1.Droid.Resource.Drawable.alarm)
                 .SetSound(soundUri)
                 .SetDefaults((int)NotificationDefaults.Vibrate)
                 .SetStyle(new NotificationCompat.BigTextStyle().BigText(message));
            Notification notification = builder.Build();
            manager.Notify(notificationId, notification);
        }
        public void CancelNotification(int id)
        {
            if (manager == null)
            {
                manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);
            }

            if (manager != null)
            {
                manager.Cancel(id);
                Console.WriteLine($"Notification with ID {id} cancelled.");
            }
            else
            {
                Console.WriteLine("NotificationManager is null, cannot cancel notification.");
            }

            try
            {
                var alarmIntent = new Intent(Forms.Context, typeof(AlarmHandler));
                var pendingAlarmIntent = PendingIntent.GetBroadcast(Forms.Context, id, alarmIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
                var alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

                if (alarmManager != null)
                {
                    alarmManager.Cancel(pendingAlarmIntent);
                    Console.WriteLine($"Alarm for notification with ID {id} cancelled.");
                }
                else
                {
                    Console.WriteLine("AlarmManager is null, cannot cancel alarm.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while cancelling alarm: {ex.Message}");
            }
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        long GetNotifyTime(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime; // milliseconds
        }
    }
}
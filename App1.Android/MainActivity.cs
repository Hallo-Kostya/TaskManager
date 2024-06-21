using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using App1.Services.Notifications;
using App1.Droid;
using Xamarin.Forms;
using Android.Content;
using Android.App.Job;
using App1.Droid.Services;
using Xamarin.Essentials;
using Android;
using static AndroidX.Activity.Result.Contract.ActivityResultContracts;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System.Collections.Generic;
using System.Linq;
using App1.Services;

namespace App1.Droid
{
    [Activity(Label = "Teyeme", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestId = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
         
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#000000"));
            }
            LoadApplication(new App());
            CreateNotificationFromIntent(Intent);
            ScheduleJob();
            RequestPermissions();
            SetDailyAlarm();
            Console.WriteLine("Основная активность запустилась!!!");
            DependencyService.Register<IAudioPlayer, AudioPlayerService>();
            ;

        }
        void RequestPermissions()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var permissionsToRequest = new List<string>();

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.PostNotifications);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.Camera);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.WriteExternalStorage);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.ReadExternalStorage);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReceiveBootCompleted) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.ReceiveBootCompleted);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.StartForegroundServicesFromBackground) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.StartForegroundServicesFromBackground);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RequestCompanionStartForegroundServicesFromBackground) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.RequestCompanionStartForegroundServicesFromBackground);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.InstantAppForegroundService) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.InstantAppForegroundService);

                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessNetworkState) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.AccessNetworkState);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Vibrate) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.Vibrate);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WakeLock) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.WakeLock);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ForegroundService) != (int)Permission.Granted)
                {
                    permissionsToRequest.Add(Manifest.Permission.ForegroundService);
                }

                if (permissionsToRequest.Any())
                {
                    ActivityCompat.RequestPermissions(this, permissionsToRequest.ToArray(), RequestId);
                }
            }
        }
        private void SetDailyAlarm()
        {
            var alarmIntent = new Intent(this, typeof(CheckOverdueReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            var alarmManager = (AlarmManager)GetSystemService(AlarmService);

            var calendar = Java.Util.Calendar.Instance;
            calendar.Set(Java.Util.CalendarField.HourOfDay, 0);
            calendar.Set(Java.Util.CalendarField.Minute, 0);
            calendar.Set(Java.Util.CalendarField.Second, 0);

            alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, AlarmManager.IntervalDay, pendingIntent);
        }
        void ScheduleJob()
        {
            // Получаем интервал из настроек
            var interval = Preferences.Get("CleaningInterval", 24); // 24 - значение по умолчанию

            // Планирование задачи очистки
            ArchiveCleanupScheduler.ScheduleArchiveCleanup(this, interval);
        }
        public override void OnRequestPermissionsResult(int requestCode,
           string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }
        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
                string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }
        
    }
}

using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using App1.Services.Notifications;
using LocalNotifications.Droid;
using Xamarin.Forms;
using Android.Content;
using Android.App.Job;
using App1.Droid.Services;
using Xamarin.Essentials;
using Android;
using static AndroidX.Activity.Result.Contract.ActivityResultContracts;

namespace App1.Droid
{
    [Activity(Label = "App1", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestPermissionId = 1000;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            CreateNotificationFromIntent(Intent);
            ScheduleJob();
            RequestPermissions();
        }
        void RequestPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.ReceiveBootCompleted) != (int)Permission.Granted ||
                    CheckSelfPermission(Manifest.Permission.WakeLock) != (int)Permission.Granted ||
                    CheckSelfPermission(Manifest.Permission.Vibrate) != (int)Permission.Granted ||
                    CheckSelfPermission(Manifest.Permission.ForegroundService) != (int)Permission.Granted)
                {
                    RequestPermissions(new string[]
                    {
                        Manifest.Permission.ReceiveBootCompleted,
                        Manifest.Permission.WakeLock,
                        Manifest.Permission.Vibrate,
                        Manifest.Permission.ForegroundService
                    }, RequestPermissionId);
                }
            }
        }


        void ScheduleJob()
        {
            // Получаем интервал из настроек
            var interval = Preferences.Get("CleaningInterval", 24); // 24 - значение по умолчанию

            // Планирование задачи очистки
            ArchiveCleanupScheduler.ScheduleArchiveCleanup(this, interval);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

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
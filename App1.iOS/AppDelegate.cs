using System;
using System.Collections.Generic;
using System.Linq;
using App1.iOS;
using App1.iOS.Services;
using Foundation;
using UIKit;
using UserNotifications;

namespace App1.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge, (granted, error) => {
                if (granted)
                {
                    Console.WriteLine("Уведомления разрешены");
                }
                else
                {
                    Console.WriteLine("Уведомления запрещены");
                }
            });
            app.RegisterForRemoteNotifications();
            UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver(iOSNotificationManager.Instance);
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            CheckIfOverdue();
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        private void ScheduleDailyNotification()
        {
            var content = new UNMutableNotificationContent
            {
                Title = "Check Overdue Tasks",
                Body = "It's time to check your overdue tasks.",
                Sound = UNNotificationSound.Default
            };

            var triggerDaily = new NSDateComponents
            {
                Hour = 0,
                Minute = 0
            };

            var trigger = UNCalendarNotificationTrigger.CreateTrigger(triggerDaily, true);

            var requestID = "dailyCheckOverdue";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    Console.WriteLine($"Failed to schedule notification: {err}");
                }
            });
        }

        public async void CheckIfOverdue()
        {
            var assignments = (await App.AssignmentsDB.GetItemsAsync()).Where(x => !x.IsDeleted);
            foreach (var assignment in assignments)
            {
                await App.AssignmentsDB.AddItemAsync(assignment);
            }
        }
    }


    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }

        public async  override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            if (appDelegate != null)
            {
                // Perform overdue check task
                appDelegate.CheckIfOverdue();
                // Perform archive cleanup
                await ArchiveCleanupService.ClearArchive();
            }
            completionHandler();
        }
    }
}

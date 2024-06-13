using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }

        public async  override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            await ArchiveCleanupService.ClearArchive();
            completionHandler();
        }
    }
}

using Foundation;
using System;
using UserNotifications;
using Xamarin.Forms;
using App1.iOS.Services;
using App1.Services.ArchiveCleanup;

[assembly: Dependency(typeof(App1.iOS.Services.ArchiveCleanupScheduler))]
namespace App1.iOS.Services
{
    public class ArchiveCleanupScheduler : IArchiveCleanupScheduler
    {
        private const string ArchiveCleanupIdentifier = "ArchiveCleanupNotification";

        public void ScheduleArchiveCleanup(int intervalInHours)
        {
            var content = new UNMutableNotificationContent
            {
                Title = "Archive Cleanup",
                Body = "Cleaning up the archive...",
                Sound = UNNotificationSound.Default
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(intervalInHours * 3600, true);

            var request = UNNotificationRequest.FromIdentifier(ArchiveCleanupIdentifier, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (error) =>
            {
                if (error != null)
                {
                    Console.WriteLine($"Error scheduling archive cleanup: {error.LocalizedDescription}");
                }
                else
                {
                    Console.WriteLine("Archive cleanup scheduled successfully.");
                }
            });
        }

        public void CancelArchiveCleanup()
        {
            UNUserNotificationCenter.Current.RemovePendingNotificationRequests(new string[] { ArchiveCleanupIdentifier });
            Console.WriteLine("Archive cleanup canceled.");
        }
    }
}

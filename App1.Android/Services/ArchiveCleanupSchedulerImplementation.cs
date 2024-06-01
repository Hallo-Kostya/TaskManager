using Android.Content;
using App1.Droid.Services;
using App1.Services.ArchiveCleanup;
using Xamarin.Forms;

[assembly: Dependency(typeof(ArchiveCleanupSchedulerImplementation))]
namespace App1.Droid.Services
{
    public class ArchiveCleanupSchedulerImplementation : IArchiveCleanupScheduler
    {
        public void ScheduleArchiveCleanup(int intervalInHours)
        {
            var context = Android.App.Application.Context;
            ArchiveCleanupScheduler.ScheduleArchiveCleanup(context, intervalInHours);
        }
    }
}

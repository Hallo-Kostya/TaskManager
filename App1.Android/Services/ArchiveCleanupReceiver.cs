using Android.Content;
using Android.App;
using System.Threading.Tasks;
using App1.Data;
using System.Linq;
using Xamarin.Forms;
using System;
using Android.Util;

namespace App1.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Label = "Archive Cleanup Receiver")]
    public class ArchiveCleanupReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Log.Info("ArchiveCleanupReceiver", "Received intent: " + intent.Action);
            Task.Run(async () =>
            {
                await ClearArchive();
            });
        }

        private async Task ClearArchive()
        {
            try
            {
                Log.Info("ArchiveCleanupReceiver", "Starting archive cleanup");
                var deletedAssignments = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == true);
                foreach (var item in deletedAssignments)
                {
                    await App.AssignmentsDB.DeleteItemAsync(item.ID);
                    Log.Info("ArchiveCleanupReceiver", $"Deleted item with ID: {item.ID}");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("ArchiveCleanupReceiver", "Error during archive cleanup: " + ex.Message);
            }
        }
    }
}

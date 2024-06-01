using Android.Content;
using Android.App;
using System.Threading.Tasks;
using App1.Data;
using System.Linq;
using Xamarin.Forms;
using System;

namespace App1.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Label = "Archive Cleanup Receiver")]
    public class ArchiveCleanupReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Task.Run(async () =>
            {
                await ClearArchive();
            });
        }

        private async Task ClearArchive()
        {
            try
            {
                var deletedAssignments = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == true);
                foreach (var item in deletedAssignments)
                {
                    await App.AssignmentsDB.DeleteItemAsync(item.ID);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

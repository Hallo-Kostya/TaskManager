using Foundation;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserNotifications;
using Xamarin.Forms;
using App1.Data;

namespace App1.iOS.Services
{
    public class ArchiveCleanupService
    {
        public static async Task ClearArchive()
        {
            try
            {
                var deletedAssignments = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == true);
                foreach (var item in deletedAssignments)
                {
                    await App.AssignmentsDB.DeleteItemAsync(item.ID);
                    Console.WriteLine($"Deleted item with ID: {item.ID}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during archive cleanup: {ex.Message}");
            }
        }
    }
}

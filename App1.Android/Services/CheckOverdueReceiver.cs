using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using App1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace App1.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class CheckOverdueReceiver : BroadcastReceiver
    {
        public override async void OnReceive(Context context, Intent intent)
        {
            await CheckIfOverdue();
        }

        private async Task CheckIfOverdue()
        {
            var assignments = (await App.AssignmentsDB.GetItemsAsync()).Where(x => !x.IsDeleted);
            foreach (var assignment in assignments)
            {
                assignment.CheckIfOverdue();
                await App.AssignmentsDB.AddItemAsync(assignment);
            }
        }
    }
}

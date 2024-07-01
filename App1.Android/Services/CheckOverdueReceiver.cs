using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using App1.Data;
using App1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace App1.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class CheckOverdueReceiver : BroadcastReceiver
    {
        private AssignmentMethodsManager _assignManager;
        public override async void OnReceive(Context context, Intent intent)
        {
            await CheckIfOverdue();
            _assignManager = new AssignmentMethodsManager();
        }

        private async Task CheckIfOverdue()
        {
            var assignments = (await App.AssignmentsDB.GetItemsAsync()).Where(x => !x.IsDeleted);
            foreach (var assignment in assignments)
            {
                await _assignManager.CheckIfOverdue(assignment);
            }
        }
    }
}

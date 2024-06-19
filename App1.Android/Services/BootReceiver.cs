using Android.App;
using Android.Content;
using Android.OS;

namespace App1.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionBootCompleted)
            {
                var alarmIntent = new Intent(context, typeof(CheckOverdueReceiver));
                var pendingIntent = PendingIntent.GetBroadcast(context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

                var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

                var calendar = Java.Util.Calendar.Instance;
                calendar.Set(Java.Util.CalendarField.HourOfDay, 0);
                calendar.Set(Java.Util.CalendarField.Minute, 0);
                calendar.Set(Java.Util.CalendarField.Second, 0);

                alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, AlarmManager.IntervalDay, pendingIntent);
            }
        }
    }
}

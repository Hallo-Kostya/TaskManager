using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using Android.Util;
using System;

namespace App1.Droid.Services
{
    public class ArchiveCleanupScheduler
    {
        private static readonly int AlarmId = 1001; // Уникальный идентификатор будильника

        public static void ScheduleArchiveCleanup(Context context, int intervalInHours)
        {
            Log.Info("ArchiveCleanupScheduler", "Scheduling archive cleanup");
            Intent intent = new Intent(context, typeof(ArchiveCleanupReceiver));
            intent.SetAction("com.example.action.CLEANUP_ARCHIVE");
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, AlarmId, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

            // Установка таймера на каждый день в определенное время
            // Установка таймера начиная с текущего времени
            //long alarmStartTime = SystemClock.ElapsedRealtime() + intervalInHours * 60 * 60 * 1000;
            long alarmStartTime = SystemClock.ElapsedRealtime() + intervalInHours * 1000;
            long repeatInterval = intervalInHours * 1000; // Интервал в миллисекундах

            alarmManager.SetRepeating(AlarmType.RtcWakeup, alarmStartTime, repeatInterval, pendingIntent);
            Log.Info("ArchiveCleanupScheduler", "Archive cleanup scheduled with interval: " + intervalInHours + " hours");
        }

        public static void CancelArchiveCleanup(Context context)
        {
            Intent intent = new Intent(context, typeof(AlarmHandler));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, AlarmId, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            alarmManager.Cancel(pendingIntent);
        }
    }
}

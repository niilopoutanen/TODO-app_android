using Android.App;
using Android.Content;
using Android.Icu.Util;

namespace TODO_app
{
    [BroadcastReceiver(Name = "com.tiimi1.BootBroadcastReceiver", Enabled = true, Exported = true)]
    [IntentFilter(new string[] { "android.intent.action.BOOT_COMPLETED" })]
    internal class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(Intent.ActionBootCompleted))
            {
                ISharedPreferences notifTime = context.GetSharedPreferences("NotificationTime", 0);
                int selectedTime = notifTime.GetInt("notifTime", default);

                Calendar calendar = Calendar.Instance;

                calendar.Set(CalendarField.HourOfDay, selectedTime);
                calendar.Set(CalendarField.Minute, 0);
                calendar.Set(CalendarField.Second, 0);
                calendar.Set(CalendarField.Millisecond, 0);

                Intent intent2 = new Intent(packageContext: context, typeof(ReminderBroadcast));
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(context: context, requestCode: 0, intent2, flags: PendingIntentFlags.Immutable);

                AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
                alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, AlarmManager.IntervalDay, pendingIntent);
            }

        }


    }
}
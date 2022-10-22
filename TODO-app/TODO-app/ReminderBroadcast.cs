using Android.App;
using Android.Content;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using TODO_app.Resources.layout;

namespace TODO_app
{
    [BroadcastReceiver(Label = "TodoAppReceiver", Enabled = true, Exported = true)]
    internal class ReminderBroadcast : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            ISharedPreferences lastNotificationDay = context.GetSharedPreferences("lastNotification", 0);
            int dayWhenLast = lastNotificationDay.GetInt("dayWhenLast", default);
            FileClass fileClass = new FileClass();
            List<TaskItem> tasks = fileClass.ReadFile();
            int tasksToday = 0;
            foreach (TaskItem task in tasks)
            {
                if (task.DueDate == DateTime.Today)
                {
                    if (task.IsDone == false)
                    {
                        tasksToday++;
                    }
                }
            }
            if (tasksToday > 0)
            {
                if (dayWhenLast != DateTime.Now.Day)
                {
                    NotificationCompat.Builder builder = new NotificationCompat.Builder(context, channelId: context.GetString(Resource.String.taskReminder))
                    .SetSmallIcon(Resource.Drawable.iconFull)
                    .SetContentTitle(tasksToday + " " + context.GetString(Resource.String.tasksToday))
                    .SetPriority(NotificationCompat.PriorityDefault);
                    PendingIntent contentClick = PendingIntent.GetActivity(context, 0, new Intent(context, typeof(SplashActivity)), PendingIntentFlags.Immutable);
                    builder.SetContentIntent(contentClick);
                    NotificationManagerCompat notificationManager = NotificationManagerCompat.From(context);

                    notificationManager.Notify(id: 200, builder.Build());
                    lastNotificationDay.Edit().PutInt("dayWhenLast", DateTime.Now.Day).Commit();
                }

            }

        }
    }
}
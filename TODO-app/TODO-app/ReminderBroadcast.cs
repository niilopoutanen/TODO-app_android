using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TODO_app.Resources.layout;

namespace TODO_app
{
    [BroadcastReceiver(Label ="TodoAppReceiver", Enabled =true, Exported =true)]
    internal class ReminderBroadcast : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            FileClass fileClass = new FileClass();
            List<TaskItem> tasks = fileClass.ReadFile();
            int tasksToday = 0;
            foreach (TaskItem task in tasks)
            {
                if (task.DueDate == DateTime.Today)
                {
                    if(task.IsDone == false)
                    {
                        tasksToday++;
                    }
                }
            }
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, channelId: "TaskReminder")
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .SetContentTitle(tasksToday + " " + context.GetString(Resource.String.tasksToday))
                .SetPriority(NotificationCompat.PriorityDefault);
            PendingIntent contentClick = PendingIntent.GetActivity(context, 0, new Intent(context, typeof(SplashActivity)), PendingIntentFlags.Immutable);
            builder.SetContentIntent(contentClick);
            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(context);

            notificationManager.Notify(id: 200, builder.Build());
        }
    }
}
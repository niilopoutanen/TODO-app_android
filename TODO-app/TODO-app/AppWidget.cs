using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TODO_app
{
    [BroadcastReceiver(Label = "TODO-app widget", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]

    internal class AppWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));
        }
        private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
        {
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.widget);

            SetTextViewText(widgetView);
            RegisterClicks(context, appWidgetIds, widgetView);

            return widgetView;
        }
        private void SetTextViewText(RemoteViews widgetView)
        {
            FileClass files = new FileClass();
            int taskNotDoneCount = 0;
            List<TaskItem> taskList = new List<TaskItem>();
            taskList = files.ReadFile();
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i].DueDate == DateTime.Today)
                {
                    if (taskList[i].IsDone == false)
                    {
                        taskNotDoneCount++;
                    }

                }
            }
            widgetView.SetTextViewText(Resource.Id.widgetCount, taskNotDoneCount.ToString());
        }
        private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
        {
            var intent = new Intent(context, typeof(AppWidget));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            // Register click event for the Background
            var piBackground = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            widgetView.SetOnClickPendingIntent(Resource.Id.widgetBG, piBackground);
        }
    }
}
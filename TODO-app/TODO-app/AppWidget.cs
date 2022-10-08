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
    [BroadcastReceiver(Label = "TODO medium", Exported = true)]
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
    }


    [BroadcastReceiver(Label = "TODO large", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider_large")]
    internal class LargeWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(LargeWidget)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));
        }
        private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
        {
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.widgetLarge);

            UpdateWidgetList(widgetView, context);

            return widgetView;
        }
        private void UpdateWidgetList(RemoteViews widgetView, Context context)
        {
            FileClass files = new FileClass();
            List<TaskItem> localtaskList = new List<TaskItem>();
            localtaskList = files.ReadFile();
            localtaskList = TaskItem.SortListByDueDate(localtaskList);
            widgetView.SetViewVisibility(Resource.Id.widgetLargeElement3, ViewStates.Gone);
            widgetView.SetViewVisibility(Resource.Id.widgetLargeElement2, ViewStates.Gone);
            widgetView.SetViewVisibility(Resource.Id.widgetLargeElement1, ViewStates.Gone);
            foreach (TaskItem task in localtaskList)
            {
                if (task.IsDone == true)
                {
                    localtaskList.Remove(task);
                }
            }
            if (localtaskList.Count > 0)
            {
                widgetView.SetViewVisibility(Resource.Id.widgetLargeElement1, ViewStates.Visible);
                widgetView.SetTextViewText(Resource.Id.widgetLargeTask1, localtaskList[0].Text);
                widgetView.SetTextViewText(Resource.Id.widgetLargeTask1Due, localtaskList[0].DueDate.ToShortDateString());
            }
            if(localtaskList.Count > 1)
            {
                widgetView.SetViewVisibility(Resource.Id.widgetLargeElement2, ViewStates.Visible);
                widgetView.SetTextViewText(Resource.Id.widgetLargeTask2, localtaskList[1].Text);
                widgetView.SetTextViewText(Resource.Id.widgetLargeTask2Due, localtaskList[1].DueDate.ToShortDateString());
            }

            if(localtaskList.Count > 2)
            {
                widgetView.SetViewVisibility(Resource.Id.widgetLargeElement3, ViewStates.Visible);
            }
            widgetView.SetTextViewText(Resource.Id.widgetLargeHeader, context.GetString(Resource.String.taskAmount) + " (" + localtaskList.Count + ")");


        }
    }


    [BroadcastReceiver(Label = "TODO small", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider_small")]
    internal class SmallWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(SmallWidget)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));
        }
        private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
        {
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.widgetSmall);

            SetTextViewText(widgetView);

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
            widgetView.SetTextViewText(Resource.Id.widgetCountSmall, taskNotDoneCount.ToString());
        }
    }
}
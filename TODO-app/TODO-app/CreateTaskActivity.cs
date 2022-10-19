using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.Browser.Trusted;
using AndroidX.Fragment.App.StrictMode;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Java.Util.Jar.Attributes;

namespace TODO_app
{
    [Activity(Label = "CreateTaskActivity", NoHistory = true, Theme = "@style/DarkEdges")]
    public class CreateTaskActivity : Activity
    {
        Button doneBtn;
        EditText nameField;
        RelativeLayout openCalendar;
        TextView selectedDateText;
        CalendarView dateCalendar;
        ImageView calendarViewArrow;

        Button oneTimeBox;
        LinearLayout oneTimeContainer;
        Button multipleTimeBox;
        LinearLayout multipleTimeContainer;

        LinearLayout timesNeededPanel;
        EditText timesneededField;
        RelativeLayout timesLessBtn;
        RelativeLayout timesMoreBtn;

        FileClass file = new FileClass();
        List<TaskItem> taskList = new List<TaskItem>();

        DateTime selectedDate = DateTime.Today;
        string taskType = "single";
        int amountNeeded = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            GetStyle();
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_createtask);
            InitializeElements();
        }
        private void InitializeElements()
        {
            doneBtn = FindViewById<Button>(Resource.Id.finishedBtn);
            doneBtn.Click += CreateDone;

            nameField = FindViewById<EditText>(Resource.Id.taskNameF);
            nameField.Click += (s, e) =>
            {
                if(nameField.Text == GetString(Resource.String.task_name_header))
                {
                    nameField.Text = "";
                    nameField.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
                }
            };

            openCalendar = FindViewById<RelativeLayout>(Resource.Id.openCalendarBtn);
            openCalendar.Click += ToggleCalendar;

            dateCalendar = FindViewById<CalendarView>(Resource.Id.dateCalendar);
            dateCalendar.DateChange += DateChanged;
            dateCalendar.MinDate = JavaSystem.CurrentTimeMillis() - 1000;
            dateCalendar.Visibility = ViewStates.Gone;
            selectedDateText = FindViewById<TextView>(Resource.Id.selectedDateText);
            calendarViewArrow = FindViewById<ImageView>(Resource.Id.calendarViewArrow);

            oneTimeBox = FindViewById<Button>(Resource.Id.singleCheckBox);
            oneTimeContainer = FindViewById<LinearLayout>(Resource.Id.singleCheckContainer);
            oneTimeContainer.Click += ModeChange;

            multipleTimeBox = FindViewById<Button>(Resource.Id.multiCheckBox);
            multipleTimeContainer = FindViewById<LinearLayout>(Resource.Id.multiCheckContainer);
            multipleTimeContainer.Click += ModeChange;

            timesneededField = FindViewById<EditText>(Resource.Id.timesNeededField);
            timesNeededPanel = FindViewById<LinearLayout>(Resource.Id.timesNeededPanel);
            timesLessBtn = FindViewById<RelativeLayout>(Resource.Id.timesLessBtn);
            timesLessBtn.Click += ChangeTimesNeeded;
            timesMoreBtn = FindViewById<RelativeLayout>(Resource.Id.timesMoreBtn);
            timesMoreBtn.Click += ChangeTimesNeeded;

            taskList = file.ReadFile();
        }
        private void CreateDone(object sender, EventArgs e)
        {
            CreateTask(nameField.Text, selectedDate, taskType, amountNeeded);
            Intent toMain = new Intent(this, typeof(MainActivity));
            StartActivity(toMain);
            Finish();
        }
        private void ModeChange(object sender, EventArgs e)
        {
            LinearLayout senderBox = (LinearLayout)sender;
            Drawable toggled = GetDrawable(Resource.Drawable.task_radio_button_active);
            Drawable untoggled = GetDrawable(Resource.Drawable.task_radio_button);
            if (senderBox.Id == oneTimeContainer.Id)
            {
                oneTimeBox.Background = toggled;
                multipleTimeBox.Background = untoggled;
                timesNeededPanel.Visibility = ViewStates.Gone;
                taskType = "single";
                amountNeeded = 1;
            }
            else if (senderBox.Id == multipleTimeContainer.Id)
            {
                multipleTimeBox.Background = toggled;
                oneTimeBox.Background = untoggled;
                timesNeededPanel.Visibility = ViewStates.Visible;
                taskType = "multi";
            }
        }
        private void ChangeTimesNeeded(object sender, EventArgs e)
        {
            int timesInput = 0;
            RelativeLayout senderBtn = (RelativeLayout)sender;
            try
            {
                string input = timesneededField.Text;
                timesInput = int.Parse(input);
            }
            catch
            {
                timesneededField.Text = "1";
            }
            if(senderBtn.Id == timesLessBtn.Id)
            {
                if(timesInput > 1)
                {
                    timesInput--;
                    timesneededField.Text = timesInput.ToString();
                }
            }
            else if (senderBtn.Id == timesMoreBtn.Id)
            {
                timesInput++;
                timesneededField.Text = timesInput.ToString();
            }
            amountNeeded = timesInput;
        }
        private void ToggleCalendar(object sender, EventArgs e)
        {
            if (dateCalendar.Visibility == ViewStates.Gone)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Android.Content.Context.InputMethodService);
                imm.HideSoftInputFromWindow(nameField.WindowToken, 0);
                dateCalendar.Visibility = ViewStates.Visible;
                calendarViewArrow.Animate().Rotation(270).SetDuration(250).Start();
            }
            else if (dateCalendar.Visibility == ViewStates.Visible)
            {
                dateCalendar.Visibility = ViewStates.Gone;
                calendarViewArrow.Animate().Rotation(180).SetDuration(250).Start();
            }
        }
        private void DateChanged(object sender, CalendarView.DateChangeEventArgs e)
        {
            selectedDate = new DateTime(e.Year, e.Month + 1, e.DayOfMonth);
            selectedDateText.Text = GetString(Resource.String.DueDate)+ ": " + selectedDate.ToShortDateString();
        }
        private void CreateTask(string taskName, DateTime dueDate, string taskType, int amountNeeded)
        {
            TaskItem task = new TaskItem(DateTime.Now);
            task.Text = taskName;
            task.DueDate = dueDate;
            task.TaskType = taskType;
            task.AmountNeeded = amountNeeded;
            taskList.Add(task);
            file.WriteFile(taskList);
            UpdateWidget();
        }
        private void GetStyle()
        {
            ISharedPreferences colorTheme = GetSharedPreferences("ColorTheme", 0);
            string color = colorTheme.GetString("colorTheme", default);
            switch (color)
            {
                case "blue":
                    SetTheme(Resource.Style.MainBlueDark);
                    break;
                case "green":
                    SetTheme(Resource.Style.MainGreenDark);
                    break;
                case "orange":
                    SetTheme(Resource.Style.MainOrangeDark);
                    break;
                case "violet":
                    SetTheme(Resource.Style.MainVioletDark);
                    break;
                case "red":
                    SetTheme(Resource.Style.MainRedDark);
                    break;
                case null:
                    SetTheme(Resource.Style.MainBlueDark);
                    break;
            }
        }
        public void UpdateWidget()
        {
            List<TaskItem> localList = new List<TaskItem>();
            foreach (TaskItem task in taskList)
            {
                if (task.IsDone == false)
                {
                    localList.Add(task);
                }
            }
            int tasksNotDoneToday = 0;
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i].DueDate == DateTime.Today)
                {
                    if (taskList[i].IsDone == false)
                    {
                        tasksNotDoneToday++;
                    }
                }
            }
            localList = TaskItem.SortListByDueDate(localList);
            Context context = this;
            AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
            RemoteViews remoteViews = new RemoteViews(context.PackageName, Resource.Layout.widget);
            RemoteViews remoteViewsLarge = new RemoteViews(context.PackageName, Resource.Layout.widgetLarge);
            RemoteViews remoteViewsSmall = new RemoteViews(context.PackageName, Resource.Layout.widgetSmall);
            ComponentName widget = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
            ComponentName widgetLarge = new ComponentName(context, Java.Lang.Class.FromType(typeof(LargeWidget)).Name);
            ComponentName widgetSmall = new ComponentName(context, Java.Lang.Class.FromType(typeof(SmallWidget)).Name);
            remoteViews.SetTextViewText(Resource.Id.widgetCount, tasksNotDoneToday.ToString());
            remoteViewsSmall.SetTextViewText(Resource.Id.widgetCountSmall, tasksNotDoneToday.ToString());
            remoteViewsLarge.SetViewVisibility(Resource.Id.widgetLargeElement3, ViewStates.Gone);
            remoteViewsLarge.SetViewVisibility(Resource.Id.widgetLargeElement2, ViewStates.Gone);
            remoteViewsLarge.SetViewVisibility(Resource.Id.widgetLargeElement1, ViewStates.Gone);

            if (localList.Count > 0)
            {
                remoteViewsLarge.SetViewVisibility(Resource.Id.widgetLargeElement1, ViewStates.Visible);
                remoteViewsLarge.SetTextViewText(Resource.Id.widgetLargeTask1, ActivityMethods.TooLongStringParser(localList[0].Text, 16));
                remoteViewsLarge.SetTextViewText(Resource.Id.widgetLargeTask1Due, localList[0].DueDate.ToShortDateString());
            }
            if (localList.Count > 1)
            {
                remoteViewsLarge.SetViewVisibility(Resource.Id.widgetLargeElement2, ViewStates.Visible);
                remoteViewsLarge.SetTextViewText(Resource.Id.widgetLargeTask2, ActivityMethods.TooLongStringParser(localList[1].Text, 16));
                remoteViewsLarge.SetTextViewText(Resource.Id.widgetLargeTask2Due, localList[1].DueDate.ToShortDateString());
            }
            if (localList.Count > 2)
            {
                remoteViewsLarge.SetViewVisibility(Resource.Id.widgetLargeElement3, ViewStates.Visible);
            }
            remoteViewsLarge.SetTextViewText(Resource.Id.widgetLargeHeader, GetString(Resource.String.taskAmount) + " (" + localList.Count + ")");

            appWidgetManager.UpdateAppWidget(widget, remoteViews);
            appWidgetManager.UpdateAppWidget(widgetLarge, remoteViewsLarge);
            appWidgetManager.UpdateAppWidget(widgetSmall, remoteViewsSmall);
        }
    }
}
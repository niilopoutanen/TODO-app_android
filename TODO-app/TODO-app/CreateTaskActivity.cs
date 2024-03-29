﻿using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Icu.Util;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;

namespace TODO_app
{
    [Activity(Label = "CreateTaskActivity", NoHistory = true, Theme = "@style/DarkEdges")]
    public class CreateTaskActivity : Activity
    {
        TextView activityHeader;
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
        TextView timesneededField;
        RelativeLayout timesLessBtn;
        RelativeLayout timesMoreBtn;

        readonly FileClass file = new FileClass();
        List<TaskItem> taskList = new List<TaskItem>();
        readonly ActivityMethods methods = new ActivityMethods();
        DateTime selectedDate = DateTime.Today;
        string taskType = "single";
        int amountNeeded = 1;
        bool vibration = false;
        string oldTaskName = "";
        Vibrator vibrator;
        VibratorManager vibratorManager;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            ISharedPreferences vibrationPref = GetSharedPreferences("Vibration", 0);
            vibration = vibrationPref.GetBoolean("vibrationEnabled", default);
            GetStyle();
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_createtask);
            InitializeElements();

            Bundle b = Intent.Extras;
            if (b != null)
            {
                string mode = b.GetString("mode");
                if (mode == "create")
                {
                    activityHeader.Text = GetString(Resource.String.add_task_header);
                    doneBtn.Text = GetString(Resource.String.add);
                    doneBtn.Click += CreateDone;
                }
                else if (mode == "tileCreate")
                {
                    doneBtn.Click += TileCreateDone;
                }
                else if (mode == "edit")
                {
                    oldTaskName = b.GetString("taskName");
                    nameField.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
                    activityHeader.Text = GetString(Resource.String.edit_task_header);
                    doneBtn.Text = "OK";
                    doneBtn.Click += EditDone;
                    dateCalendar.DateChange += (s, e) =>
                    {
                        selectedDate = new DateTime(e.Year, e.Month, e.DayOfMonth);
                        selectedDateText.Text = GetString(Resource.String.DueDate) + ": " + selectedDate.ToShortDateString();
                    };
                    foreach (TaskItem task in taskList)
                    {
                        if (task.Text == oldTaskName)
                        {
                            Drawable toggled = GetDrawable(Resource.Drawable.task_radio_button_active);
                            Drawable untoggled = GetDrawable(Resource.Drawable.task_radio_button);
                            nameField.Text = task.Text;
                            Calendar oldDate = Calendar.Instance;
                            oldDate.Set(CalendarField.Year, task.DueDate.Year);
                            oldDate.Set(CalendarField.Month, task.DueDate.Month);
                            oldDate.Set(CalendarField.DayOfMonth, task.DueDate.Day);
                            if (task.TaskType == "single")
                            {
                                oneTimeBox.Background = toggled;
                                multipleTimeBox.Background = untoggled;
                                timesNeededPanel.Visibility = ViewStates.Gone;
                                taskType = "single";
                            }
                            else if (task.TaskType == "multi")
                            {
                                oneTimeBox.Background = untoggled;
                                multipleTimeBox.Background = toggled;
                                timesNeededPanel.Visibility = ViewStates.Visible;
                                taskType = "multi";
                            }
                            long milliTime = oldDate.TimeInMillis;
                            dateCalendar.Date = milliTime;
                            selectedDate = new DateTime(task.DueDate.Year, task.DueDate.Month, task.DueDate.Day);

                            timesneededField.Text = task.AmountNeeded.ToString();
                            amountNeeded = task.AmountNeeded;

                        }
                    }

                }
            }
        }
        private void InitializeElements()
        {
            vibrator = (Vibrator)GetSystemService(VibratorService);
            vibratorManager = (VibratorManager)GetSystemService(VibratorManagerService);
            activityHeader = FindViewById<TextView>(Resource.Id.createTaskActivityHeader);
            doneBtn = FindViewById<Button>(Resource.Id.finishedBtn);

            nameField = FindViewById<EditText>(Resource.Id.taskNameF);
            nameField.FocusChange += (s, e) =>
            {
                if (nameField.Text == GetString(Resource.String.task_name_header))
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

            timesneededField = FindViewById<TextView>(Resource.Id.timesNeededField);
            timesNeededPanel = FindViewById<LinearLayout>(Resource.Id.timesNeededPanel);
            timesLessBtn = FindViewById<RelativeLayout>(Resource.Id.timesLessBtn);
            timesLessBtn.Click += ChangeTimesNeeded;
            timesMoreBtn = FindViewById<RelativeLayout>(Resource.Id.timesMoreBtn);
            timesMoreBtn.Click += ChangeTimesNeeded;

            taskList = file.ReadFile();
        }
        private void CreateDone(object sender, EventArgs e)
        {
            bool failed = false;

            if (nameField.Text == GetString(Resource.String.task_name_header))
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                Toast.MakeText(this, GetString(Resource.String.invalidName), ToastLength.Long).Show();
                failed = true;
            }
            if (string.IsNullOrWhiteSpace(nameField.Text))
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                Toast.MakeText(this, GetString(Resource.String.invalidName), ToastLength.Long).Show();
                failed = true;
            }
            foreach (TaskItem task in taskList)
            {
                if (task.Text.ToLower() == nameField.Text.ToLower())
                {
                    methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                    Toast.MakeText(this, GetString(Resource.String.nameExists), ToastLength.Long).Show();
                    failed = true;
                }
            }


            if (failed == false)
            {
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, methods.intensitySmall);
                }
                CreateTask(nameField.Text, selectedDate, taskType, amountNeeded);
                Finish();
            }

        }
        private void TileCreateDone(object sender, EventArgs e)
        {
            bool failed = false;

            if (nameField.Text == GetString(Resource.String.task_name_header))
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                Toast.MakeText(this, GetString(Resource.String.invalidName), ToastLength.Long).Show();
                failed = true;
            }
            if (string.IsNullOrWhiteSpace(nameField.Text))
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                Toast.MakeText(this, GetString(Resource.String.invalidName), ToastLength.Long).Show();
                failed = true;
            }
            foreach (TaskItem task in taskList)
            {
                if (task.Text.ToLower() == nameField.Text.ToLower())
                {
                    methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                    Toast.MakeText(this, GetString(Resource.String.nameExists), ToastLength.Long).Show();
                    failed = true;
                }
            }


            if (failed == false)
            {
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, methods.intensitySmall);
                }
                CreateTask(nameField.Text, selectedDate, taskType, amountNeeded);
                Intent mainIntent = new Intent(this, typeof(MainActivity));
                StartActivity(mainIntent);
                Finish();
            }

        }
        private void EditDone(object sender, EventArgs e)
        {
            bool failed = false;

            if (nameField.Text == GetString(Resource.String.task_name_header))
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                Toast.MakeText(this, GetString(Resource.String.invalidName), ToastLength.Long).Show();
                failed = true;
            }
            if (string.IsNullOrWhiteSpace(nameField.Text))
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                Toast.MakeText(this, GetString(Resource.String.invalidName), ToastLength.Long).Show();
                failed = true;
            }
            foreach (TaskItem task in taskList)
            {
                if(task.Text.ToLower() != oldTaskName.ToLower())
                {
                    if (task.Text.ToLower() == nameField.Text.ToLower())
                    {
                        methods.Vibrate(vibrator, vibratorManager, methods.intensityHard);
                        Toast.MakeText(this, GetString(Resource.String.nameExists), ToastLength.Long).Show();
                        failed = true;
                    }
                }
            }


            if (failed == false)
            {
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, methods.intensitySmall);
                }
                DeleteTask(oldTaskName);
                CreateTask(nameField.Text, selectedDate, taskType, amountNeeded);
                Finish();
            }
        }
        private void ModeChange(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensitySmall);
            }
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
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensitySmall);
            }
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
            if (senderBtn.Id == timesLessBtn.Id)
            {
                if (timesInput > 1)
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
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensitySmall);
            }
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
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, methods.intensitySmall);
            }
            selectedDate = new DateTime(e.Year, e.Month + 1, e.DayOfMonth);
            selectedDateText.Text = GetString(Resource.String.DueDate) + ": " + selectedDate.ToShortDateString();
        }
        private void CreateTask(string taskName, DateTime dueDate, string taskType, int amountNeeded)
        {
            taskList = file.ReadFile();
            TaskItem task = new TaskItem(DateTime.Now);
            task.Text = taskName;
            task.DueDate = dueDate;
            task.TaskType = taskType;
            task.AmountNeeded = amountNeeded;
            taskList.Add(task);
            file.WriteFile(taskList);
            UpdateWidget();
        }
        private void DeleteTask(string oldTaskName)
        {
            foreach (TaskItem task in taskList)
            {
                if (task.Text == oldTaskName)
                {
                    taskList.Remove(task);
                    file.WriteFile(taskList);
                    break;
                }
            }
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
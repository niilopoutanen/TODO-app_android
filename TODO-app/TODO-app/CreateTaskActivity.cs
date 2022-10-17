using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Java.Util.Jar.Attributes;

namespace TODO_app
{
    [Activity(Label = "CreateTaskActivity")]
    public class CreateTaskActivity : Activity
    {
        Button doneBtn;
        EditText nameField;
        RelativeLayout openCalendar;
        CalendarView dateCalendar;
        ImageView calendarViewArrow;
        FileClass file = new FileClass();
        List<TaskItem> taskList = new List<TaskItem>();
        DateTime selectedDate = DateTime.Today; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
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

            calendarViewArrow = FindViewById<ImageView>(Resource.Id.calendarViewArrow);
            taskList = file.ReadFile();
        }
        private void CreateDone(object sender, EventArgs e)
        {
            CreateTask(nameField.Text, selectedDate);
            Finish();
        }

        private void ToggleCalendar(object sender, EventArgs e)
        {
            if (dateCalendar.Visibility == ViewStates.Gone)
            {
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
        }
        private void CreateTask(string taskName, DateTime dueDate)
        {
            TaskItem task = new TaskItem(DateTime.Now);
            task.Text = taskName;
            task.DueDate = dueDate;
            taskList.Add(task);
            file.WriteFile(taskList);
        }
    }
}
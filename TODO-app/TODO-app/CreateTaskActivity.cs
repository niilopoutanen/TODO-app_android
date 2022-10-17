using Android.App;
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
    [Activity(Label = "CreateTaskActivity")]
    public class CreateTaskActivity : Activity
    {
        Button doneBtn;
        EditText nameField;
        RelativeLayout openCalendar;
        CalendarView dateCalendar;
        ImageView calendarViewArrow;
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

            openCalendar = FindViewById<RelativeLayout>(Resource.Id.openCalendarBtn);
            openCalendar.Click += ToggleCalendar;

            dateCalendar = FindViewById<CalendarView>(Resource.Id.dateCalendar);
            dateCalendar.DateChange += DateChanged;
            dateCalendar.Visibility = ViewStates.Gone;

            calendarViewArrow = FindViewById<ImageView>(Resource.Id.calendarViewArrow);
        }
        private void CreateDone(object sender, EventArgs e)
        {
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

        }
    }
}
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_createtask);
        }
    }
}
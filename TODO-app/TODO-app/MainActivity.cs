using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ImageButton btnCreateTask;
        private Button btnTaskNameSubmit;
        private DatePicker datePicker;
        private EditText TaskNameInput;
        private List<TaskItem> taskList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            TaskNameInput = FindViewById<EditText>(Resource.Id.TaskNameSubmit);
            btnCreateTask = FindViewById<ImageButton>(Resource.Id.CreateTask);
            btnCreateTask.Click += btnCreateTask_Click;
            btnTaskNameSubmit = FindViewById<Button>(Resource.Id.TaskNameSubmit);
            btnTaskNameSubmit.Click += btnSave_Click;
            datePicker = FindViewById<DatePicker>(Resource.Id.datePicker1);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            Dialog popup = new Dialog(this);
            popup.SetContentView(Resource.Layout.create_task_popup);
            popup.Window.SetSoftInputMode(SoftInput.AdjustResize);
            popup.SetTitle("New Task");
            popup.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TaskItem task = new TaskItem();
            task.Text = TaskNameInput.Text;
            task.DueDate = datePicker.DateTime;
            taskList.Add(task);
        }

        internal List<TaskItem> ReturnTasks()
        {
            return taskList;
        }
    }
}
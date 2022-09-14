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
        private Button btnCreateTask;
        private Button btnAddTask;
        private EditText TaskNameInput;
        private List<TaskItem> taskList;
        private FileClass fileSaver = new FileClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            fileSaver.ReadFile();

            btnCreateTask = FindViewById<Button>(Resource.Id.CreateTask);
            btnCreateTask.Click += btnCreateTask_Click;

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.create_task_popup);
            TaskNameInput = FindViewById<EditText>(Resource.Id.NameInputForm);
            btnAddTask = FindViewById<Button>(Resource.Id.AddButton);
            btnAddTask.Click += btnAddTask_Click;
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            TaskItem task = new TaskItem();
            task.Text = TaskNameInput.Text;
            taskList.Add(task);
            fileSaver.WriteFile(taskList);
        }

        internal List<TaskItem> ReturnTasks()
        {
            return taskList;
        }
    }
}
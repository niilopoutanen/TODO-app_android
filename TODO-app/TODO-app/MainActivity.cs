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
        private List<TaskItem> taskList = new List<TaskItem>();
        private EditText yearInput;
        private EditText monthInput;
        private EditText dayInput;
        private FileClass fileSaver = new FileClass();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            taskList = fileSaver.ReadFile();

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
            yearInput = FindViewById<EditText>(Resource.Id.YearSelectInput);
            monthInput = FindViewById<EditText>(Resource.Id.MonthSelectInput);
            dayInput = FindViewById<EditText>(Resource.Id.DaySelectInput);
            btnAddTask.Click += btnAddTask_Click;
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            TaskItem task = new TaskItem();


            if (!IsNull(TaskNameInput.Text))
            {
                task.Text = TaskNameInput.Text;
            }

            else
            {
                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();
                alert.SetTitle("Huomio");
                alert.SetMessage("Tehtävän nimi ei voi olla tyhjä");
                alert.SetButton("OK", (c, ev) => { alert.Dismiss(); });
                alert.Show();
            }

            int day = Convert.ToInt32(dayInput.Text);
            int month = Convert.ToInt32(monthInput.Text);
            int year = Convert.ToInt32(yearInput.Text);
            DateTime dueDate = new DateTime(year, month, day);
            task.DueDate = dueDate;

            taskList.Add(task);
            fileSaver.WriteFile(taskList);
        }

        internal List<TaskItem> ReturnTasks()
        {
            return taskList;
        }

        private bool IsNull(string s)
        {
            if (s == "" && s == null)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private void RightDay(int month, int year)
        {

        }
    }
}
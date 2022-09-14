using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Java.Time.Format;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TableLayout tableLayout;
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
            CreateTaskItem("testi");
            //SetContentView(Resource.Layout.create_task_popup);
            //TaskNameInput = FindViewById<EditText>(Resource.Id.NameInputForm);
            //btnAddTask = FindViewById<Button>(Resource.Id.AddButton);
            //yearInput = FindViewById<EditText>(Resource.Id.YearSelectInput);
            //monthInput = FindViewById<EditText>(Resource.Id.MonthSelectInput);
            //dayInput = FindViewById<EditText>(Resource.Id.DaySelectInput);
            //btnAddTask.Click += btnAddTask_Click;
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            TaskItem task = new TaskItem();
            int day = 0;
            int month = 0;
            int year = 0;

            day = Convert.ToInt32(dayInput.Text);
            month = Convert.ToInt32(monthInput.Text);
            year = Convert.ToInt32(yearInput.Text);
            DateTime dueDate = new DateTime(year, month, day);

            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Huomio");
            alert.SetButton("OK", (c, ev) => { alert.Dismiss(); });

            if (IsNull(TaskNameInput.Text))
            {
                alert.SetMessage("Tehtävän nimi ei voi olla tyhjä");
                alert.Show();
            }

            else if (IsNull(dayInput.Text) && IsNull(monthInput.Text) && IsNull(yearInput.Text))
            {
                alert.SetMessage("Päivämäärä ei voi olla tyhjä");
                alert.Show();
            }

            else if (month > 12)
            {
                alert.SetMessage("Vuodessa ei ole noin montaa kuukautta");
                alert.Show();
            }

            else if (!IsDayInMonth(day, month, year))
            {
                alert.SetMessage("Antamassasi kuukaudessa ei ole noin montaa päivää");
                alert.Show();
            }

            else if (dueDate < DateTime.Now)
            {
                alert.SetMessage("Eräpäivä ei voi olla menneisyydessä");
                alert.Show();
            }

            else
            {
                task.DueDate = dueDate;
                task.Text = TaskNameInput.Text;
                taskList.Add(task);
                fileSaver.WriteFile(taskList);

            }
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

        private bool IsDayInMonth(int day, int month, int year)
        {
            int amountOfDaysInMonth = DateTime.DaysInMonth(year, month);
            if (day > amountOfDaysInMonth)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        private void CreateTaskItem(string taskName)
        {
            tableLayout = FindViewById<TableLayout>(Resource.Id.TasksTable);
            Button btn = new Button(this);
            btn.SetWidth(400);
            btn.SetHeight(200);
            Drawable rounded60 = GetDrawable(Resource.Drawable.rounded60px);
            btn.Background = rounded60;
            TableRow row = new TableRow(this);
            

            tableLayout.AddView(row);
            row.AddView(btn);
        }
    }
}
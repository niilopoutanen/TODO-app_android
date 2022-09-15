using Android.App;
using Android.Graphics.Drawables;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
        private Button taskToggle;
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

            taskToggle = FindViewById<Button>(Resource.Id.radiobutton);
            taskToggle.Click += TaskToggle_Click;

            foreach (TaskItem t in taskList)
            {
                CreateTaskItem(t.Text);
            }

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
        private void TaskToggle_Click(object sender, EventArgs e)
        {
            Drawable active = GetDrawable(Resource.Drawable.task_radio_button_active);
            taskToggle.Background = active;
        }
        private void btnAddTask_Click(object sender, EventArgs e)
        {
            TaskItem task = new TaskItem();
            int day = 0;
            int month = 0;
            int year = 0;
            DateTime dueDate = DateTime.Today;

            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Huomio");
            alert.SetButton("OK", (c, ev) => { alert.Dismiss(); });

            if (IsNull(TaskNameInput.Text))
            {
                alert.SetMessage("Tehtävän nimi ei voi olla tyhjä");
                alert.Show();
            }

            else if (!IsNull(dayInput.Text) && !IsNull(monthInput.Text) && !IsNull(yearInput.Text))
            {
                day = Convert.ToInt32(dayInput.Text);
                month = Convert.ToInt32(monthInput.Text);
                year = Convert.ToInt32(yearInput.Text);

                if (month > 12)
                {
                    alert.SetMessage("Vuodessa ei ole noin montaa kuukautta");
                    alert.Show();
                }
                
                else if (!IsDayInMonth(day, month, year))
                {
                    alert.SetMessage("Antamassasi kuukaudessa ei ole noin montaa päivää");
                    alert.Show();
                }

                else
                {
                    dueDate = new DateTime(year, month, day);
                }

                if (dueDate < DateTime.Today)
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

                    SetContentView(Resource.Layout.activity_main);
                    btnCreateTask = FindViewById<Button>(Resource.Id.CreateTask);
                    btnCreateTask.Click += btnCreateTask_Click;

                    foreach (TaskItem t in taskList)
                    {
                        CreateTaskItem(t.Text);
                    }
                }
            }

            else
            {
                alert.SetMessage("Päivämäärä ei voi olla tyhjä");
                alert.Show();
            }
        }

        private bool IsNull(string s)
        {
            if (s == "" || s == null)
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

        /// <summary>
        /// Creates task card with given task name
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns>returns id index of created element</returns>
        private int CreateTaskItem(string taskName)
        {
            tableLayout = FindViewById<TableLayout>(Resource.Id.TasksTable);

            Drawable rounded60 = GetDrawable(Resource.Drawable.rounded60px);
            Drawable toggleBG = GetDrawable(Resource.Drawable.task_radio_button);

            RelativeLayout layout = new RelativeLayout(this);
            layout.SetMinimumHeight(250);
            layout.Background = rounded60;
            layout.SetGravity(GravityFlags.CenterVertical);
            layout.Id = View.GenerateViewId();
            TableLayout.LayoutParams parameters = new TableLayout.LayoutParams(TableLayout.LayoutParams.MatchParent, TableLayout.LayoutParams.WrapContent);
            parameters.SetMargins(0, 0, 0, 60);
            layout.SetPadding(65, 0,0,0);
            layout.LayoutParameters = parameters;

            Button toggle = new Button(this);
            RelativeLayout.LayoutParams buttonSize = new RelativeLayout.LayoutParams(120,120);
            buttonSize.SetMargins(0, 0, 40, 0);
            toggle.Background = toggleBG;
            toggle.LayoutParameters = buttonSize;
            toggle.Id = View.GenerateViewId();
            toggle.Click += TaskToggle_Click;
            

            RelativeLayout.LayoutParams headerParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            headerParams.AddRule(LayoutRules.RightOf, toggle.Id);
            TextView header = new TextView(this);
            header.Text = taskName;
            header.LayoutParameters = headerParams;
            header.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.white));
            header.SetTextSize(new Android.Util.ComplexUnitType(),60);
            


            tableLayout.AddView(layout);
            layout.AddView(header);
            layout.AddView(toggle);


            return layout.Id;
        }
    }
}
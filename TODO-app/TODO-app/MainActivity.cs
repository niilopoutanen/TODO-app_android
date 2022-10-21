using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views.InputMethods;
using TODO_app.Resources.layout;
using Firebase.Analytics;
using Android.Animation;
using Android.Appwidget;
using AndroidX.Core.Content;
using Java.Security;
using Android;
using Android.Icu.Util;
using Android.Views.Autofill;
using AndroidX.Core.OS;
using AndroidX.Core.View;
using Android.Views.Animations;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private int activeDate = 1;
        private string currentTheme;
        private bool guideDone;
        private bool vibration;
        private bool notifications;
        RelativeLayout btnCreateTask;

        LinearLayout mainHeader;
        HorizontalScrollView calendarView;
        Button showAll;
        Button searchBar;
        EditText searchField;
        RelativeLayout settingsOpen;
        LinearLayout taskCountLayout;
        TextView taskCount;

        RelativeLayout dayUpEdit;
        RelativeLayout monthUpEdit;
        RelativeLayout yearUpEdit;
        RelativeLayout dayDownEdit;
        RelativeLayout monthDownEdit;
        RelativeLayout yearDownEdit;

        EditText dayInputEdit;
        EditText monthInputEdit;
        EditText yearInputEdit;

        int thisDay = DateTime.Today.Day;
        int thisMonth = DateTime.Today.Month;
        int thisYear = DateTime.Today.Year;


        RelativeLayout date1Btn;
        RelativeLayout date2Btn;
        RelativeLayout date3Btn;
        RelativeLayout date4Btn;
        RelativeLayout date5Btn;
        RelativeLayout date6Btn;
        RelativeLayout date7Btn;

        TextView date1number;
        TextView date1str;
        TextView date2number;
        TextView date2str;
        TextView date3number;
        TextView date3str;
        TextView date4number;
        TextView date4str;
        TextView date5number;
        TextView date5str;
        TextView date6number;
        TextView date6str;
        TextView date7number;
        TextView date7str;

        LinearLayout scrollLayout;
        Button sortByCreationDate;
        Button sortByDueDate;

        RelativeLayout missedTasksBtn;
        TextView missedTasksCount;
        Space missedTaskSpace;

        ImageView alarmIcon;

        EditText editTaskField;

        RelativeLayout mainInfo;
        TextView invalidEditTaskName;
        TextView invalidEditDate;



        Vibrator vibrator;
        VibratorManager vibratorManager;
        Dictionary<string, int> elementIds = new Dictionary<string, int>();

        ActivityMethods methods = new ActivityMethods();

        private static FileClass file = new FileClass();
        private List<TaskItem> taskList = new List<TaskItem>();
        private bool taskCreated = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            FirebaseAnalytics.GetInstance(this);
            LoadSettings();
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            taskList = file.ReadFile();
            taskList = TaskItem.SortListByIsDone(taskList);

            SetContentView(Resource.Layout.activity_main);
            UpdateWidget();
            InitializeElements();
            CalendarDater();

            DoneAndGone();
            CountAndShowMissed();
            ShowDatestasks(DateTime.Today);
            UpdateTaskCount();
            GetStyle();

            CreateNotificationChannel();
            if (notifications == true)
            {
                CreateNotificationRepeater();
            }

            //Start onboarding
            if (guideDone == false)
            {
                Intent onBoarderStarter = new Intent(this, typeof(OnBoardingActivity));
                StartActivity(onBoarderStarter);
            }
        }
        protected override void OnRestart()
        {
            base.OnRestart();
            taskList = file.ReadFile();
            taskList = TaskItem.SortListByIsDone(taskList);
            CountAndShowMissed();
            for (int i = 1; i < 8; i++)
            {
                //1 or 0 = Today
                if (activeDate == 1 || activeDate == 0)
                {
                    scrollLayout.RemoveAllViews();
                    ShowDatestasks(DateTime.Today);
                    UpdateTaskCount();
                    taskCreated = true;
                    break;
                }

                //-1 = Missed tasks
                else if (activeDate == -1)
                {
                    UpdateTaskCount();
                    taskCreated = true;
                    break;
                }

                //2-7 = Rest of the days
                else if (activeDate == i)
                {
                    scrollLayout.RemoveAllViews();
                    ShowDatestasks(DateTime.Today.AddDays(i - 1));
                    UpdateTaskCount();
                    taskCreated = true;
                    break;
                }
            }
            UpdateTaskCount();

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void CheckIfMissedAnymore()
        {
            int amountOfMissed = 0;
            foreach (TaskItem t in taskList)
            {
                if (t.DueDate < DateTime.Today)
                {
                    amountOfMissed++;
                }
            }
            missedTasksCount.Text = amountOfMissed.ToString();
            if (amountOfMissed <= 0)
            {
                missedTasksBtn.Visibility = ViewStates.Gone;
                missedTaskSpace.Visibility = ViewStates.Gone;
                missedTasksCount.Text = "0";
            }
        }
        private void ShowMissedTasksElement(int amountOfMissed)
        {
            missedTasksBtn.Visibility = ViewStates.Visible;
            missedTaskSpace.Visibility = ViewStates.Visible;
            missedTasksCount.Text = amountOfMissed.ToString();
        }
        private void ShowMissedTasks(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 60);
            }
            activeDate = -1;
            scrollLayout.RemoveAllViews();
            alarmIcon.BackgroundTintList = GetColorStateList(Resource.Color.white);
            missedTasksCount.SetTextColor(GetColorStateList(Resource.Color.white));
            missedTasksBtn.BackgroundTintList = GetColorStateList(GetStyle());
            date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date1number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date1str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date2number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date2str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date3number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date3str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date4number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date4str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date5number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date5str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date6number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date6str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date7number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date7str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            foreach (TaskItem t in taskList)
            {
                if (t.DueDate < DateTime.Today)
                {
                    CreateTaskElement(t);
                }
            }
            UpdateTaskCount();
        }
        private int GetStyle()
        {
            if (currentTheme == "mainBlue")
            {
                return Resource.Color.mainBlue;
            }
            else if (currentTheme == "mainOrange")
            {
                return Resource.Color.mainOrange;
            }
            else if (currentTheme == "mainGreen")
            {
                return Resource.Color.mainGreen;
            }
            else if (currentTheme == "mainViolet")
            {
                return Resource.Color.mainViolet;
            }
            else if (currentTheme == "mainRed")
            {
                return Resource.Color.mainRed;
            }
            else
            {
                return Resource.Color.mainBlue;
            }
        }
        private void LoadSettings()
        {
            ISharedPreferences themePref = GetSharedPreferences("Theme", 0);
            string themeSelected = themePref.GetString("themeSelected", default);
            ISharedPreferences hasWatchedGuide = GetSharedPreferences("hasWatchedGuide", 0);
            guideDone = hasWatchedGuide.GetBoolean("hasWatchedGuide", default);
            ISharedPreferences colorTheme = GetSharedPreferences("ColorTheme", 0);
            ISharedPreferences vibrationPref = GetSharedPreferences("Vibration", 0);
            ISharedPreferences notificationsPref = GetSharedPreferences("Notifications", 0);

            vibration = vibrationPref.GetBoolean("vibrationEnabled", default);
            notifications = notificationsPref.GetBoolean("notificationsEnabled", default);
            string color = colorTheme.GetString("colorTheme", default);
            switch (color)
            {
                case "blue":
                    SetTheme(Resource.Style.MainBlue);
                    currentTheme = "mainBlue";
                    break;
                case "green":
                    SetTheme(Resource.Style.MainGreen);
                    currentTheme = "mainGreen";
                    break;
                case "orange":
                    SetTheme(Resource.Style.MainOrange);
                    currentTheme = "mainOrange";
                    break;
                case "violet":
                    SetTheme(Resource.Style.MainViolet);
                    currentTheme = "mainViolet";
                    break;
                case "red":
                    SetTheme(Resource.Style.MainRed);
                    currentTheme = "mainRed";
                    break;
                case null:
                    SetTheme(Resource.Style.MainBlue);
                    currentTheme = "mainBlue";
                    break;
            }

            switch (themeSelected)
            {
                case "dark":
                    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightYes;
                    break;
                case "light":
                    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
                    break;
                case "system":
                    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightFollowSystem;
                    break;
                case null:
                    themePref.Edit().PutString("themeSelected", "system").Commit();
                    break;
            }
        }
        /// <summary>
        /// Put all element connections here for cleaner code
        /// </summary>
        private void InitializeElements()
        {
            alarmIcon = FindViewById<ImageView>(Resource.Id.alarmIcon);
            vibrator = (Vibrator)GetSystemService(VibratorService);
            vibratorManager = (VibratorManager)GetSystemService(VibratorManagerService);
            mainInfo = FindViewById<RelativeLayout>(Resource.Id.MainInfo);
            missedTasksBtn = FindViewById<RelativeLayout>(Resource.Id.missedTasksBtn);
            missedTasksBtn.Click += ShowMissedTasks;
            missedTasksCount = FindViewById<TextView>(Resource.Id.missedTasksCount);
            missedTaskSpace = FindViewById<Space>(Resource.Id.missedTasksSpace);
            btnCreateTask = FindViewById<RelativeLayout>(Resource.Id.CreateTask);
            btnCreateTask.Click += (s, e) =>
            {
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, 45);
                }
                Intent intent = new Intent(this, typeof(CreateTaskActivity));
                intent.PutExtra("mode", "create");
                StartActivity(intent);
            };
            mainHeader = FindViewById<LinearLayout>(Resource.Id.mainHeader);
            calendarView = FindViewById<HorizontalScrollView>(Resource.Id.calendarView);
            showAll = FindViewById<Button>(Resource.Id.ShowAll);
            showAll.Click += ShowAll;
            taskCountLayout = FindViewById<LinearLayout>(Resource.Id.taskCountLayout);
            taskCount = FindViewById<TextView>(Resource.Id.taskCountText);

            settingsOpen = FindViewById<RelativeLayout>(Resource.Id.SettingsButton);
            settingsOpen.Click += OpenSettings;
            searchField = FindViewById<EditText>(Resource.Id.SearchField);
            searchField.Click += ToggleSearchMode;
            searchField.TextChanged += SearchChanged;
            searchBar = FindViewById<Button>(Resource.Id.SearchBar);
            searchBar.Click += ToggleSearchMode;

            date1number = FindViewById<TextView>(Resource.Id.date1number);
            date1str = FindViewById<TextView>(Resource.Id.date1str);
            date2number = FindViewById<TextView>(Resource.Id.date2number);
            date2str = FindViewById<TextView>(Resource.Id.date2str);
            date3number = FindViewById<TextView>(Resource.Id.date3number);
            date3str = FindViewById<TextView>(Resource.Id.date3str);
            date4number = FindViewById<TextView>(Resource.Id.date4number);
            date4str = FindViewById<TextView>(Resource.Id.date4str);
            date5number = FindViewById<TextView>(Resource.Id.date5number);
            date5str = FindViewById<TextView>(Resource.Id.date5str);
            date6number = FindViewById<TextView>(Resource.Id.date6number);
            date6str = FindViewById<TextView>(Resource.Id.date6str);
            date7number = FindViewById<TextView>(Resource.Id.date7number);
            date7str = FindViewById<TextView>(Resource.Id.date7str);


            date1Btn = FindViewById<RelativeLayout>(Resource.Id.date1btn);
            date2Btn = FindViewById<RelativeLayout>(Resource.Id.date2btn);
            date3Btn = FindViewById<RelativeLayout>(Resource.Id.date3btn);
            date4Btn = FindViewById<RelativeLayout>(Resource.Id.date4btn);
            date5Btn = FindViewById<RelativeLayout>(Resource.Id.date5btn);
            date6Btn = FindViewById<RelativeLayout>(Resource.Id.date6btn);
            date7Btn = FindViewById<RelativeLayout>(Resource.Id.date7btn);

            date1Btn.Click += CalendarSelector;
            date2Btn.Click += CalendarSelector;
            date3Btn.Click += CalendarSelector;
            date4Btn.Click += CalendarSelector;
            date5Btn.Click += CalendarSelector;
            date6Btn.Click += CalendarSelector;
            date7Btn.Click += CalendarSelector;

            scrollLayout = FindViewById<LinearLayout>(Resource.Id.ScrollLayout);

            sortByCreationDate = FindViewById<Button>(Resource.Id.SortByCreationDate);
            sortByCreationDate.Click += SortBy;

            sortByDueDate = FindViewById<Button>(Resource.Id.SortByDueDate);
            sortByDueDate.Click += SortBy;
        }

        /// <summary>
        /// Updates task count element
        /// </summary>
        private void UpdateTaskCount()
        {
            int elementCount = scrollLayout.ChildCount;
            if (elementCount == 1)
            {
                taskCount.Text = elementCount.ToString() + " " + GetString(Resource.String.task);
            }
            else
            {
                taskCount.Text = elementCount.ToString() + " " + GetString(Resource.String.task);
            }
        }

        /// <summary>
        /// Triggers every time search field text changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchChanged(object sender, EventArgs e)
        {
            TextView field = (TextView)sender;


            string fieldText = searchField.Text;
            scrollLayout.RemoveAllViews();
            foreach (TaskItem task in taskList)
            {
                if (task.Text.Contains(fieldText))
                {
                    CreateTaskElement(task);
                }
            }

            UpdateTaskCount();
        }

        /// <summary>
        /// Triggers show all menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAll(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 45);
            }
            if (calendarView.Visibility == ViewStates.Visible)
            {
                scrollLayout.RemoveAllViews();
                mainInfo.Visibility = ViewStates.Gone;
                calendarView.Visibility = ViewStates.Gone;
                sortByDueDate.Visibility = ViewStates.Visible;
                sortByCreationDate.Visibility = ViewStates.Visible;
                showAll.BackgroundTintList = GetColorStateList(GetStyle());
                showAll.SetTextColor(GetColorStateList(Resource.Color.white));
                taskList = TaskItem.SortListByIsDone(taskList);
                foreach (TaskItem t in taskList)
                {
                    CreateTaskElement(t);
                }
                UpdateTaskCount();

            }
            else if (calendarView.Visibility == ViewStates.Gone)
            {
                for (int i = 1; i < 8; i++)
                {
                    if (activeDate == 1 || activeDate == 0)
                    {
                        scrollLayout.RemoveAllViews();
                        ShowDatestasks(DateTime.Today);
                        UpdateTaskCount();
                        break;
                    }

                    else if (activeDate == i)
                    {
                        scrollLayout.RemoveAllViews();
                        ShowDatestasks(DateTime.Today.AddDays(i - 1));
                        UpdateTaskCount();
                        break;
                    }
                }
                calendarView.Visibility = ViewStates.Visible;
                sortByDueDate.Visibility = ViewStates.Gone;
                sortByCreationDate.Visibility = ViewStates.Gone;
                mainInfo.Visibility = ViewStates.Visible;
                showAll.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                showAll.SetTextColor(GetColorStateList(Resource.Color.textSecondary));
                UpdateTaskCount();

            }
        }

        /// <summary>
        /// Call this when you want to toggle between search mode and normal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleSearchMode(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 45);
            }
            if (searchBar.Visibility == ViewStates.Visible)
            {
                searchField.Text = "";
                scrollLayout.RemoveAllViews();
                calendarView.Visibility = ViewStates.Gone;
                mainInfo.Visibility = ViewStates.Gone;
                searchBar.Visibility = ViewStates.Gone;
                searchField.Visibility = ViewStates.Visible;
                searchField.FocusableInTouchMode = true;
                searchField.RequestFocus();
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.ShowSoftInput(searchField, ShowFlags.Forced);
                UpdateTaskCount();
            }
            else if (searchField.Visibility == ViewStates.Visible)
            {
                searchBar.Visibility = ViewStates.Visible;
                searchField.Visibility = ViewStates.Gone;
                mainInfo.Visibility = ViewStates.Visible;
                calendarView.Visibility = ViewStates.Visible;
                sortByDueDate.Visibility = ViewStates.Gone;
                sortByCreationDate.Visibility = ViewStates.Gone;
                mainInfo.Visibility = ViewStates.Visible;
                showAll.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                showAll.SetTextColor(GetColorStateList(Resource.Color.textSecondary));
                InputMethodManager imm = (InputMethodManager)GetSystemService(Android.Content.Context.InputMethodService);
                imm.HideSoftInputFromWindow(searchField.WindowToken, 0);
                scrollLayout.RemoveAllViews();
                for (int i = 1; i < 8; i++)
                {
                    if (activeDate == 1 || activeDate == 0)
                    {
                        scrollLayout.RemoveAllViews();
                        ShowDatestasks(DateTime.Today);
                        UpdateTaskCount();
                        break;
                    }

                    else if (activeDate == i)
                    {
                        scrollLayout.RemoveAllViews();
                        ShowDatestasks(DateTime.Today.AddDays(i - 1));
                        UpdateTaskCount();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// this opens settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSettings(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 45);
            }

            Intent settingsStarter = new Intent(this, typeof(SettingsActivity));
            StartActivity(settingsStarter);

        }

        /// <summary>
        /// This modifies date values with arrows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArrowModify(object sender, EventArgs e)
        {
            var button = (RelativeLayout)sender;
            try
            {
                string DayInputTextEdit = dayInputEdit.Text;
                int daySelectedEdit = int.Parse(DayInputTextEdit);
                string MonthInputTextEdit = monthInputEdit.Text;
                int MonthSelectedEdit = int.Parse(MonthInputTextEdit);
                string YearInputTextEdit = yearInputEdit.Text;
                int YearSelectedEdit = int.Parse(YearInputTextEdit);
                switch (button.Id)
                {
                    case Resource.Id.EditDayArrowUp:
                        daySelectedEdit++;
                        if (daySelectedEdit > DateTime.DaysInMonth(YearSelectedEdit, MonthSelectedEdit))
                        {
                            daySelectedEdit--;
                        }
                        dayInputEdit.Text = daySelectedEdit.ToString();
                        break;

                    case Resource.Id.EditDayArrowDown:
                        daySelectedEdit--;
                        if (daySelectedEdit < 1)
                        {
                            daySelectedEdit++;
                        }
                        dayInputEdit.Text = daySelectedEdit.ToString();
                        break;

                    case Resource.Id.EditMonthArrowUp:
                        MonthSelectedEdit++;
                        if (MonthSelectedEdit > 12)
                        {
                            MonthSelectedEdit = 12;
                        }
                        monthInputEdit.Text = MonthSelectedEdit.ToString();
                        break;

                    case Resource.Id.EditYearArrowUp:
                        YearSelectedEdit++;
                        yearInputEdit.Text = YearSelectedEdit.ToString();
                        break;

                    case Resource.Id.EditYearArrowDown:
                        YearSelectedEdit--;
                        if (YearSelectedEdit < DateTime.Today.Year)
                        {
                            YearSelectedEdit = DateTime.Today.Year;
                        }
                        yearInputEdit.Text = YearSelectedEdit.ToString();
                        break;

                    case Resource.Id.EditMonthArrowDown:
                        MonthSelectedEdit--;
                        if (MonthSelectedEdit < 1)
                        {
                            MonthSelectedEdit = 1;
                        }
                        monthInputEdit.Text = MonthSelectedEdit.ToString();
                        break;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Use this to show a popup on screen. Provide a text for the header, description, and the OK-button.
        /// </summary>
        /// <param name="Header">Header of the popup.</param>
        /// <param name="Desc">Description of the popup.</param>
        /// <param name="YesText">Text of the OK-button.</param>
        /// <summary>
        /// Opens task edit popup
        /// </summary>
        /// <param name="oldTaskName"></param>
        private void EditTaskPopup(string oldTaskName)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 45);
            }
            Intent intent = new Intent(this, typeof(CreateTaskActivity));
            Bundle editBundle = new Bundle();
            editBundle.PutString("mode", "edit");
            editBundle.PutString("taskName", oldTaskName);
            intent.PutExtras(editBundle);
            
            StartActivity(intent);

        }

        /// <summary>
        /// This happens when task element is pressed & held down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HoldTaskElement(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 100);
            }


            LinearLayout button = (LinearLayout)sender;
            TextView taskName = (TextView)button.FindViewById<TextView>(Resource.Id.taskName);
            CheckIfMissedAnymore();

            Android.App.AlertDialog.Builder dialog1 = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert1 = dialog1.Create();

            LayoutInflater inflater1 = (LayoutInflater)this.GetSystemService(Android.Content.Context.LayoutInflaterService);
            View view1 = inflater1.Inflate(Resource.Layout.dialog_popup, null);
            view1.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
            alert1.SetView(view1);
            alert1.Show();
            alert1.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
            alert1.Window.SetLayout(DpToPx(300), RelativeLayout.LayoutParams.WrapContent);
            Button confirm1 = view1.FindViewById<Button>(Resource.Id.PopupConfirm);
            confirm1.Text = GetString(Resource.String.delete);
            TextView header1 = view1.FindViewById<TextView>(Resource.Id.PopupHeader);
            header1.Text = taskName.Text;
            TextView desc1 = view1.FindViewById<TextView>(Resource.Id.PopupDescription);
            desc1.Text = GetString(Resource.String.editDesc);

            confirm1.Click += (s, e) =>
            {
                alert1.Dismiss();
                Android.App.AlertDialog.Builder dialog2 = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert2 = dialog2.Create();

                LayoutInflater inflater2 = (LayoutInflater)this.GetSystemService(Android.Content.Context.LayoutInflaterService);
                View view2 = inflater2.Inflate(Resource.Layout.dialog_popup, null);
                view2.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
                alert2.SetView(view2);
                alert2.Show();
                alert2.Window.SetLayout(DpToPx(300), DpToPx(150));
                alert2.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
                Button confirm2 = view2.FindViewById<Button>(Resource.Id.PopupConfirm);
                TextView header2 = view2.FindViewById<TextView>(Resource.Id.PopupHeader);
                header2.Text = GetString(Resource.String.deleteTaskHeader);
                TextView desc2 = view2.FindViewById<TextView>(Resource.Id.PopupDescription);
                desc2.Text = GetString(Resource.String.deleteTaskDescription);

                confirm2.Click += (s, e) =>
                {
                    DeleteTaskItem(taskName.Text);
                    button.RemoveAllViews();
                    scrollLayout.RemoveView(button);
                    alert2.Dismiss();
                    UpdateTaskCount();
                    CheckIfMissedAnymore();
                };

                Button cancel2 = view2.FindViewById<Button>(Resource.Id.PopupCancel);
                cancel2.Click += (s, e) =>
                {
                    button.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
                    alert2.Dismiss();
                };
            };

            Button edit = view1.FindViewById<Button>(Resource.Id.PopupCancel);
            edit.Text = GetString(Resource.String.edit);

            edit.Click += (s, e) =>
            {
                alert1.Dismiss();
                EditTaskPopup(taskName.Text);
            };










        }
        /// <summary>
        /// Initializes calendar dates on creation
        /// </summary>
        private void CalendarDater()
        {
            DateTime today = DateTime.Today;
            TextView todayHeader = FindViewById<TextView>(Resource.Id.todayHeader);
            Dictionary<int, string> dayNames = new Dictionary<int, string>()
            {
                {0, GetString(Resource.String.Sunday)},
                {1, GetString(Resource.String.Monday)},
                {2, GetString(Resource.String.Tuesday)},
                {3, GetString(Resource.String.Wednesday)},
                {4, GetString(Resource.String.Thursday)},
                {5, GetString(Resource.String.Friday)},
                {6, GetString(Resource.String.Saturday)}
            };
            Dictionary<int, string> dayNamesShort = new Dictionary<int, string>()
            {
                {0, GetString(Resource.String.SundayShort)},
                {1, GetString(Resource.String.MondayShort)},
                {2, GetString(Resource.String.TuesdayShort)},
                {3, GetString(Resource.String.WednesdayShort)},
                {4, GetString(Resource.String.ThursdayShort)},
                {5, GetString(Resource.String.FridayShort)},
                {6, GetString(Resource.String.SaturdayShort)}
            };
            string todaystr = dayNames[(int)today.DayOfWeek] + "\n" + today.Day.ToString() + "." + today.Month.ToString() + "." + today.Year.ToString();
            todayHeader.Text = todaystr;

            date1number.Text = today.Day.ToString();
            date1str.Text = dayNamesShort[(int)today.DayOfWeek];

            date2number.Text = today.AddDays(1).Day.ToString();
            date2str.Text = dayNamesShort[(int)today.AddDays(1).DayOfWeek];

            date3number.Text = today.AddDays(2).Day.ToString();
            date3str.Text = dayNamesShort[(int)today.AddDays(2).DayOfWeek];

            date4number.Text = today.AddDays(3).Day.ToString();
            date4str.Text = dayNamesShort[(int)today.AddDays(3).DayOfWeek];

            date5number.Text = today.AddDays(4).Day.ToString();
            date5str.Text = dayNamesShort[(int)today.AddDays(4).DayOfWeek];

            date6number.Text = today.AddDays(5).Day.ToString();
            date6str.Text = dayNamesShort[(int)today.AddDays(5).DayOfWeek];

            date7number.Text = today.AddDays(6).Day.ToString();
            date7str.Text = dayNamesShort[(int)today.AddDays(6).DayOfWeek];
        }

        /// <summary>
        /// Triggers the colors with the calendar items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarSelector(object sender, EventArgs e)
        {

            alarmIcon.BackgroundTintList = GetColorStateList(Resource.Color.textPrimary);
            missedTasksCount.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);

            date1number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date1str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date2number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date2str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date3number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date3str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date4number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date4str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date5number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date5str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date6number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date6str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date7number.SetTextColor(GetColorStateList(Resource.Color.textPrimary));
            date7str.SetTextColor(GetColorStateList(Resource.Color.textPrimary));

            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 45);
            }
            scrollLayout.RemoveAllViews();
            var button = (RelativeLayout)sender;
            missedTasksBtn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            switch (button.Id)
            {


                case Resource.Id.date1btn:
                    activeDate = 1;
                    date1Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    date1number.SetTextColor(GetColorStateList(Resource.Color.white));
                    date1str.SetTextColor(GetColorStateList(Resource.Color.white));
                    ShowDatestasks(DateTime.Today);

                    break;

                case Resource.Id.date2btn:
                    activeDate = 2;
                    date2Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    date2number.SetTextColor(GetColorStateList(Resource.Color.white));
                    date2str.SetTextColor(GetColorStateList(Resource.Color.white));
                    ShowDatestasks(DateTime.Today.AddDays(1));
                    break;

                case Resource.Id.date3btn:
                    activeDate = 3;
                    date3Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    date3number.SetTextColor(GetColorStateList(Resource.Color.white));
                    date3str.SetTextColor(GetColorStateList(Resource.Color.white));
                    ShowDatestasks(DateTime.Today.AddDays(2));
                    break;
                case Resource.Id.date4btn:
                    activeDate = 4;
                    date4Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    date4number.SetTextColor(GetColorStateList(Resource.Color.white));
                    date4str.SetTextColor(GetColorStateList(Resource.Color.white));
                    ShowDatestasks(DateTime.Today.AddDays(3));
                    break;

                case Resource.Id.date5btn:
                    activeDate = 5;
                    date5Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    date5number.SetTextColor(GetColorStateList(Resource.Color.white));
                    date5str.SetTextColor(GetColorStateList(Resource.Color.white));
                    ShowDatestasks(DateTime.Today.AddDays(4));
                    break;

                case Resource.Id.date6btn:
                    activeDate = 6;
                    date6Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    date6number.SetTextColor(GetColorStateList(Resource.Color.white));
                    date6str.SetTextColor(GetColorStateList(Resource.Color.white));
                    ShowDatestasks(DateTime.Today.AddDays(5));
                    break;

                case Resource.Id.date7btn:
                    activeDate = 7;
                    date7Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    date7number.SetTextColor(GetColorStateList(Resource.Color.white));
                    date7str.SetTextColor(GetColorStateList(Resource.Color.white));
                    ShowDatestasks(DateTime.Today.AddDays(6));
                    break;


            }
            UpdateTaskCount();
        }

        /// <summary>
        /// Dynamically creates task element
        /// </summary>
        /// <param name="taskName"></param>
        private void CreateTaskElement(TaskItem task)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            if (task.TaskType == "single")
            {
                View cardSingle = inflater.Inflate(Resource.Layout.card_single, scrollLayout, false);
                TextView taskName = cardSingle.FindViewById<TextView>(Resource.Id.taskName);
                Button taskToggle = cardSingle.FindViewById<Button>(Resource.Id.taskToggle);
                TextView taskDate = cardSingle.FindViewById<TextView>(Resource.Id.taskDate);
                cardSingle.LongClick += HoldTaskElement;
                cardSingle.Click += (s, e) =>
                {
                    if (vibration == true)
                    {
                        methods.Vibrate(vibrator, vibratorManager, 45);
                    }
                    switch (taskDate.Visibility)
                    {
                        case ViewStates.Gone:
                            taskDate.Visibility = ViewStates.Visible;
                            break;
                        case ViewStates.Visible:
                            taskDate.Visibility = ViewStates.Gone;
                            break;
                    }
                };
                LinearLayout.LayoutParams marginParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                marginParams.SetMargins(0, 0, 0, DpToPx(20));
                cardSingle.LayoutParameters = marginParams;
                cardSingle.Id = View.GenerateViewId();
                taskToggle.Click += TaskToggle;
                Drawable toggleClicked = GetDrawable(Resource.Drawable.task_radio_button_active);
                if(task.IsDone == true)
                {
                    taskToggle.Background = toggleClicked;
                }
                taskName.Text = task.Text;
                taskDate.Text = task.DueDate.ToShortDateString();

                try
                {
                    elementIds.Add(task.Text, cardSingle.Id);
                }
                catch
                {
                    elementIds[task.Text] = cardSingle.Id;
                }
                scrollLayout.AddView(cardSingle);
            }
            else if (task.TaskType == "multi")
            {
                View cardMulti = inflater.Inflate(Resource.Layout.card_multi, scrollLayout, false);
                TextView taskName = cardMulti.FindViewById<TextView>(Resource.Id.taskName);
                TextView taskProgress = cardMulti.FindViewById<TextView>(Resource.Id.taskProgress);
                LinearLayout taskAmountAdjust = cardMulti.FindViewById<LinearLayout>(Resource.Id.taskAdjustDone);
                RelativeLayout taskTimesLess = cardMulti.FindViewById<RelativeLayout>(Resource.Id.timesDoneLess);
                RelativeLayout taskTimesMore = cardMulti.FindViewById<RelativeLayout>(Resource.Id.timesDoneMore);
                TextView taskAmountDone = cardMulti.FindViewById<TextView>(Resource.Id.taskAmountDone);
                RelativeLayout taskProgressBase = cardMulti.FindViewById<RelativeLayout>(Resource.Id.taskProgressBase);
                ImageView taskProgressBar = cardMulti.FindViewById<ImageView>(Resource.Id.taskProgressBar);
                int estimatedWidth = scrollLayout.Width - DpToPx(80);
                RelativeLayout.LayoutParams widthParam = new RelativeLayout.LayoutParams(methods.ProgressBarCalculator(estimatedWidth, task.AmountDone, task.AmountNeeded), RelativeLayout.LayoutParams.MatchParent);
                taskProgressBar.LayoutParameters = widthParam;

                LinearLayout.LayoutParams marginParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                marginParams.SetMargins(0, 0, 0, DpToPx(20));
                cardMulti.LayoutParameters = marginParams;
                taskName.Text = task.Text;
                taskProgress.Text = methods.ProgressVisualizer(task.AmountDone, task.AmountNeeded);
                cardMulti.Id = View.GenerateViewId();
                taskAmountDone.Text = task.AmountDone.ToString();
                taskAmountAdjust.Visibility = ViewStates.Gone;
                cardMulti.Click += (s, e) =>
                {
                    switch (taskAmountAdjust.Visibility)
                    {
                        case ViewStates.Visible:
                            taskAmountAdjust.Visibility = ViewStates.Gone;
                            break;
                        case ViewStates.Gone:
                            taskAmountAdjust.Visibility = ViewStates.Visible;
                            break;
                    }
                };
                cardMulti.LongClick += HoldTaskElement;
                taskTimesLess.Click += (s, e) =>
                {
                    if (vibration == true)
                    {
                        methods.Vibrate(vibrator, vibratorManager, 45);
                    }
                    int timesDone;
                    bool success = int.TryParse(taskAmountDone.Text, out timesDone);
                    if (success)
                    {
                        if(timesDone > 0)
                        {
                            timesDone--;
                            taskAmountDone.Text = timesDone.ToString();
                            foreach (TaskItem t in taskList)
                            {
                                if (t == task)
                                {
                                    if (vibration == true)
                                    {
                                        methods.Vibrate(vibrator, vibratorManager, 45);
                                    }
                                    t.AmountDone = timesDone;
                                    taskProgress.Text = methods.ProgressVisualizer(t.AmountDone, t.AmountNeeded);
                                    taskProgressBase.LayoutTransition.EnableTransitionType(LayoutTransitionType.Changing);
                                    RelativeLayout.LayoutParams widthParam = new RelativeLayout.LayoutParams(methods.ProgressBarCalculator(taskProgressBase.Width, task.AmountDone, task.AmountNeeded), RelativeLayout.LayoutParams.MatchParent);
                                    taskProgressBar.LayoutParameters = widthParam;
                                }
                            }
                            file.WriteFile(taskList);
                        }
                    }
                };
                taskTimesMore.Click += (s, e) =>
                {
                    int timesDone;
                    bool success = int.TryParse(taskAmountDone.Text, out timesDone);
                    if (success)
                    {
                        if(timesDone < task.AmountNeeded)
                        {
                            timesDone++;
                            taskAmountDone.Text = timesDone.ToString();
                            foreach (TaskItem t in taskList)
                            {
                                if (t == task)
                                {
                                    if (vibration == true)
                                    {
                                        methods.Vibrate(vibrator, vibratorManager, 45);
                                    }
                                    t.AmountDone = timesDone;
                                    taskProgress.Text = methods.ProgressVisualizer(t.AmountDone, t.AmountNeeded);
                                    taskProgressBase.LayoutTransition.EnableTransitionType(LayoutTransitionType.Changing);
                                    RelativeLayout.LayoutParams widthParam = new RelativeLayout.LayoutParams(methods.ProgressBarCalculator(taskProgressBase.Width, task.AmountDone, task.AmountNeeded), RelativeLayout.LayoutParams.MatchParent);
                                    taskProgressBar.LayoutParameters = widthParam;
                                }
                            }
                            file.WriteFile(taskList);
                        }

                    }
                };
                try
                {
                    elementIds.Add(task.Text, cardMulti.Id);
                }
                catch
                {
                    elementIds[task.Text] = cardMulti.Id;
                }
                scrollLayout.AddView(cardMulti);
            }
            CheckIfMissedAnymore();

        }


        /// <summary>
        /// Toggle between done and not done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskToggle(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 45);
            }
            Button button = (Button)sender;
            RelativeLayout buttonParent = (RelativeLayout)button.Parent;
            Drawable active = GetDrawable(Resource.Drawable.task_radio_button_active);
            Drawable inactive = GetDrawable(Resource.Drawable.task_radio_button);
            TextView header = (TextView)buttonParent.FindViewById<TextView>(Resource.Id.taskName);
            foreach (TaskItem t in taskList)
            {
                if (t.Text == header.Text)
                {
                    t.IsDone = !t.IsDone;
                    if (t.IsDone == true)
                    {
                        button.Background = active;
                    }
                    else if (t.IsDone == false)
                    {
                        button.Background = inactive;
                    }
                }
            }
            file.WriteFile(taskList);
            UpdateWidget();

            //buttonParent.RemoveAllViews();
            //scrollLayout.RemoveView(buttonParent);
            UpdateTaskCount();
        }

        /// <summary>
        /// Converts pixels to dots per inch
        /// </summary>
        /// <param name="dpValue"></param>
        /// <returns></returns>
        private int DpToPx(int dpValue)
        {
            int pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dpValue, Resources.DisplayMetrics);
            return pixel;
        }

        private void CreateTaskItem(string name, DateTime dueDate, string type, int amountNeeded, int amountDone)
        {
            TaskItem task = new TaskItem(DateTime.Now);
            task.Text = name;
            task.DueDate = dueDate;
            task.TaskType = type;
            task.AmountNeeded = amountNeeded;
            task.AmountDone = amountDone;
            taskList.Add(task);
            file.WriteFile(taskList);
            UpdateWidget();
        }

        private void DeleteTaskItem(string name)
        {
            foreach (TaskItem t in taskList)
            {
                if (t.Text == name)
                {
                    taskList.Remove(t);
                    file.WriteFile(taskList);
                    break;
                }
            }
            UpdateWidget();

        }

        /// <summary>
        /// Checks if the given date is in the given month
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>
        /// True if the day is in the month
        /// </returns>
        private bool IsDayInMonth(int day, int month, int year)
        {
            if (day < 1)
            {
                return false;
            }

            if (month > 12 || month < 1)
            {
                return false;
            }

            if (year > DateTime.MaxValue.Year || year < DateTime.MinValue.Year)
            {
                return false;
            }

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
        /// Sorts show all view tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortBy(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            sortByCreationDate.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
            sortByDueDate.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);

            switch (clicked.Id)
            {
                case Resource.Id.SortByDueDate:
                    sortByDueDate.BackgroundTintList = GetColorStateList(GetStyle());
                    scrollLayout.RemoveAllViews();
                    if (vibration == true)
                    {
                        methods.Vibrate(vibrator, vibratorManager, 45);
                    }
                    foreach (TaskItem task in TaskItem.SortListByDueDate(taskList))
                    {
                        CreateTaskElement(task);
                    }

                    break;

                case Resource.Id.SortByCreationDate:
                    sortByCreationDate.BackgroundTintList = GetColorStateList(GetStyle());

                    scrollLayout.RemoveAllViews();
                    if (vibration == true)
                    {
                        methods.Vibrate(vibrator, vibratorManager, 45);
                    }
                    foreach (TaskItem task in TaskItem.SortListByCreationDate(taskList))
                    {
                        CreateTaskElement(task);
                    }

                    break;
            }
        }

        private void ShowDatestasks(DateTime date)
        {
            foreach (TaskItem t in taskList)
            {
                if (t.DueDate == date)
                {
                    CreateTaskElement(t);
                }
            }
        }

        private void CountAndShowMissed()
        {
            int amountOfMissed = 0;
            foreach (TaskItem t in taskList)
            {
                if (t.DueDate < DateTime.Today)
                {
                    amountOfMissed++;
                }
            }

            if (amountOfMissed > 0)
            {
                ShowMissedTasksElement(amountOfMissed);
            }
        }

        /// <summary>
        /// Deletes all missed tasks that are marked as done
        /// </summary>
        private void DoneAndGone()
        {
            foreach (TaskItem t in taskList)
            {
                if (t.IsDone == true && t.DueDate < DateTime.Today)
                {
                    DeleteTaskItem(t.Text);
                }
            }
            file.WriteFile(taskList);
        }

        /// <summary>
        /// Creates a new TaskItem and adds it to the taskList
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="oldTaskName"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="isNew"></param>
        private void CreateATask(string taskName, string oldTaskName, string day, string month, string year, bool isNew)
        {
            taskCreated = false;
            bool didFail = false;

            if (string.IsNullOrWhiteSpace(taskName))
            {
                InvalidInput(editTaskField, invalidEditTaskName, "Tehtävän nimi ei voi olla tyhjä");
                didFail = true;
            }
            if (isNew == true)
            {
                foreach (TaskItem t in taskList)
                {
                    if (t.Text.ToLower() == taskName.ToLower())
                    {
                        InvalidInput(editTaskField, invalidEditTaskName, "Tämän niminen tehtävä on jo olemassa");
                        didFail = true;
                    }
                }
            }
            if (!int.TryParse(day, out int intDay))
            {
                InvalidInput(dayInputEdit, invalidEditDate, "Virheellinen päivä");
                didFail = true;
            }
            if (!int.TryParse(month, out int intMonth))
            {
                InvalidInput(monthInputEdit, invalidEditDate, "Virheellinen kuukausi");
                didFail = true;
            }
            if (!int.TryParse(year, out int intYear))
            {
                InvalidInput(yearInputEdit, invalidEditDate, "Virheellinen vuosi");
                didFail = true;
            }
            if (intDay < 1)
            {
                InvalidInput(dayInputEdit, invalidEditDate, "Päivä ei voi olla pienempi kuin 1");
                didFail = true;
            }
            if (intMonth < 1)
            {
                InvalidInput(monthInputEdit, invalidEditDate, "Kuukausi ei voi olla pienempi kuin 1");
                didFail = true;
            }
            if (intYear < 1)
            {
                InvalidInput(yearInputEdit, invalidEditDate, "Vuosi ei voi olla pienempi kuin 1");
                didFail = true;
            }
            if (intMonth > 12)
            {
                InvalidInput(monthInputEdit, invalidEditDate, "Kuukausi ei voi olla suurempi kuin 12");
                didFail = true;
            }
            if (intDay < DateTime.Today.Day)
            {
                InvalidInput(dayInputEdit, invalidEditDate, "Päivä ei voi olla menneisyydessä");
                didFail = true;
            }
            if (intMonth < DateTime.Today.Month)
            {
                InvalidInput(monthInputEdit, invalidEditDate, "Kuukausi ei voi olla menneisyydessä");
                didFail = true;
            }
            if (intYear < DateTime.Today.Year)
            {
                InvalidInput(yearInputEdit, invalidEditDate, "Vuosi ei voi olla menneisyydessä");
                didFail = true;
            }
            if (intDay > DateTime.MaxValue.Day)
            {
                InvalidInput(dayInputEdit, invalidEditDate, "Liian suuri päivä");
                didFail = true;
            }
            if (intMonth > DateTime.MaxValue.Month)
            {
                InvalidInput(monthInputEdit, invalidEditDate, "Liian suuri kuukausi");
                didFail = true;
            }
            if (intYear > DateTime.MaxValue.Year)
            {
                InvalidInput(yearInputEdit, invalidEditDate, "Liian suuri vuosi");
                didFail = true;
            }
            if (!IsDayInMonth(intDay, intMonth, intYear))
            {
                InvalidInput(dayInputEdit, invalidEditDate, "Päivä ei kuulu annettuun kuukauteen");
                didFail = true;
            }
            if (didFail == false)
            {
                DateTime dueDate = new DateTime(intYear, intMonth, intDay);
                DeleteTaskItem(oldTaskName);
                CreateTaskItem(taskName, dueDate, "single", 1, 0);
                //Checks which date the user is currently focused on and then shows the tasks for that date
                for (int i = 1; i < 8; i++)
                {
                    //1 or 0 = Today
                    if (activeDate == 1 || activeDate == 0)
                    {
                        scrollLayout.RemoveAllViews();
                        ShowDatestasks(DateTime.Today);
                        UpdateTaskCount();
                        taskCreated = true;
                        break;
                    }
                    //-1 = Missed tasks
                    else if (activeDate == -1)
                    {
                        UpdateTaskCount();
                        taskCreated = true;
                        break;
                    }

                    //2-7 = Rest of the days
                    else if (activeDate == i)
                    {
                        scrollLayout.RemoveAllViews();
                        ShowDatestasks(DateTime.Today.AddDays(i - 1));
                        UpdateTaskCount();
                        taskCreated = true;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// When input is invalid, this method is called to change the color of the input field and vibrate the phone (if toggled on)
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="errorDesc"></param>
        /// <param name="errorName"></param>
        public void InvalidInput(EditText visual, TextView errorDesc, string errorName)
        {
            if (visual != null)
            {
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, 200);
                }
                Drawable invalid = GetDrawable(Resource.Drawable.rounded10pxinvalid);
                if (errorDesc != null)
                {
                    errorDesc.Text = errorName;
                }
                visual.BackgroundTintList = null;
                //visual.BackgroundTintList = GetColorStateList(Resource.Color.errorColor);

                //await System.Threading.Tasks.Task.Delay(1000);
                //visual.Background = active;
                //visual.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);  
            }

        }


        [Obsolete]
        public override void OnBackPressed()
        {
            this.FinishAffinity();
        }
        private void CreateNotificationChannel()
        {
            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel channel = new NotificationChannel(id: GetString(Resource.String.taskReminder), GetString(Resource.String.taskReminder), Android.App.NotificationImportance.Default);
                channel.Description = GetString(Resource.String.notificationDesc);

                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
        private void CreateNotificationRepeater()
        {
            ISharedPreferences notifTime = GetSharedPreferences("NotificationTime", 0);
            int selectedTime = notifTime.GetInt("notifTime", default);

            Calendar calendar = Calendar.Instance;

            calendar.Set(CalendarField.HourOfDay, selectedTime);
            calendar.Set(CalendarField.Minute, 0);
            calendar.Set(CalendarField.Second, 0);
            calendar.Set(CalendarField.Millisecond, 0);

            Intent intent = new Intent(packageContext: this, typeof(ReminderBroadcast));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context: this, requestCode: 0, intent, flags: PendingIntentFlags.Immutable);

            AlarmManager alarmManager = (AlarmManager)GetSystemService(AlarmService);
            alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, AlarmManager.IntervalDay, pendingIntent);
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
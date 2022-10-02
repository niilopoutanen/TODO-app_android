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
using Java.Lang;
using Firebase.Analytics;
using Android.Gms.Tasks;
using System.Threading.Tasks;
using AndroidX.RecyclerView.Widget;
using static Java.Util.Jar.Attributes;
using Android.Animation;
using System.ComponentModel;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private int activeDate = 1;
        private string currentTheme;
        private bool guideDone;
        private bool vibration;
        RelativeLayout btnCreateTask;
        Button btnAddTask;

        LinearLayout mainHeader;
        LinearLayout createTaskHeader;
        HorizontalScrollView calendarView;
        Button showAll;
        Button searchBar;
        EditText searchField;
        RelativeLayout settingsOpen;
        EditText taskNameField;
        LinearLayout taskCountLayout;
        TextView taskCount;

        RelativeLayout dayUp;
        RelativeLayout monthUp;
        RelativeLayout yearUp;
        RelativeLayout dayDown;
        RelativeLayout monthDown;
        RelativeLayout yearDown;

        EditText dayInput;
        EditText monthInput;
        EditText yearInput;

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


        LinearLayout scrollLayout;

        RelativeLayout backToMain;

        Button sortByCreationDate;
        Button sortByDueDate;
        ScrollView scrollBase;


        RelativeLayout missedTasksBtn;
        TextView missedTasksCount;
        Space missedTaskSpace;

        EditText editTaskField;

        RelativeLayout mainInfo;
        TextView invalidEditTaskName;
        TextView invalidEditDate;

        TextView invalidTaskName;
        TextView invalidDate;


        Dictionary<string, int> elementIds = new Dictionary<string, int>();


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
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


            InitializeElements();
            CalendarDater();

            DoneAndGone();
            CountAndShowMissed();

            ShowDatestasks(DateTime.Today);
            UpdateTaskCount();
            GetStyle();


            //Start onboarding
            if (guideDone == false)
            {
                Intent onBoarderStarter = new Intent(this, typeof(OnBoardingActivity));
                StartActivity(onBoarderStarter);
            }
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
            activeDate = -1;
            scrollLayout.RemoveAllViews();
            missedTasksBtn.BackgroundTintList = GetColorStateList(GetStyle());
            date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            foreach (TaskItem t in taskList)
            {
                if (t.DueDate < DateTime.Today)
                {
                    CreateTaskElement(t.Text, t.IsDone, t.DueDate);
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
            ISharedPreferences hasWatchedGuide = GetSharedPreferences("hasWatchedGuide", 0);
            guideDone = hasWatchedGuide.GetBoolean("hasWatchedGuide", default);
            ISharedPreferences colorTheme = GetSharedPreferences("ColorTheme", 0);
            ISharedPreferences vibrationPref = GetSharedPreferences("Vibration", 0);
            vibration = vibrationPref.GetBoolean("vibrationEnabled", default);
            string color = colorTheme.GetString("colorTheme", default);
            if (color == "blue")
            {
                SetTheme(Resource.Style.MainBlue);
                currentTheme = "mainBlue";
            }
            else if (color == "green")
            {
                SetTheme(Resource.Style.MainGreen);
                currentTheme = "mainGreen";
            }
            else if (color == "orange")
            {
                SetTheme(Resource.Style.MainOrange);
                currentTheme = "mainOrange";
            }
            else if (color == "violet")
            {
                SetTheme(Resource.Style.MainViolet);
                currentTheme = "mainViolet";
            }
            else if (color == "red")
            {
                SetTheme(Resource.Style.MainRed);
                currentTheme = "mainRed";
            }
            else if (color == null)
            {
                SetTheme(Resource.Style.MainBlue);
                currentTheme = "mainBlue";
            }
        }
        /// <summary>
        /// Put all element connections here for cleaner code
        /// </summary>
        private void InitializeElements()
        {
            mainInfo = FindViewById<RelativeLayout>(Resource.Id.MainInfo);
            missedTasksBtn = FindViewById<RelativeLayout>(Resource.Id.missedTasksBtn);
            missedTasksBtn.Click += ShowMissedTasks;
            missedTasksCount = FindViewById<TextView>(Resource.Id.missedTasksCount);
            missedTaskSpace = FindViewById<Space>(Resource.Id.missedTasksSpace);
            scrollBase = FindViewById<ScrollView>(Resource.Id.scrollBase);
            backToMain = FindViewById<RelativeLayout>(Resource.Id.BackToMain);
            backToMain.Click += BackToMain;
            btnCreateTask = FindViewById<RelativeLayout>(Resource.Id.CreateTask);
            btnCreateTask.Click += OpenCreateView;
            btnAddTask = FindViewById<Button>(Resource.Id.AddTask);
            btnAddTask.Click += CloseCreateView;
            createTaskHeader = FindViewById<LinearLayout>(Resource.Id.CreateTaskHeader);
            mainHeader = FindViewById<LinearLayout>(Resource.Id.mainHeader);
            calendarView = FindViewById<HorizontalScrollView>(Resource.Id.calendarView);
            showAll = FindViewById<Button>(Resource.Id.ShowAll);
            showAll.Click += ShowAll;
            taskNameField = FindViewById<EditText>(Resource.Id.TaskNameField);
            taskCountLayout = FindViewById<LinearLayout>(Resource.Id.taskCountLayout);
            taskCount = FindViewById<TextView>(Resource.Id.taskCountText);

            settingsOpen = FindViewById<RelativeLayout>(Resource.Id.SettingsButton);
            settingsOpen.Click += ButtonAction;
            searchField = FindViewById<EditText>(Resource.Id.SearchField);
            searchField.Click += ToggleSearchMode;
            searchField.TextChanged += SearchChanged;
            searchBar = FindViewById<Button>(Resource.Id.SearchBar);
            searchBar.Click += ToggleSearchMode;

            dayInput = FindViewById<EditText>(Resource.Id.DayInput);
            monthInput = FindViewById<EditText>(Resource.Id.MonthInput);
            yearInput = FindViewById<EditText>(Resource.Id.YearInput);

            dayUp = FindViewById<RelativeLayout>(Resource.Id.DayArrowUp);
            monthUp = FindViewById<RelativeLayout>(Resource.Id.MonthArrowUp);
            yearUp = FindViewById<RelativeLayout>(Resource.Id.YearArrowUp);

            dayDown = FindViewById<RelativeLayout>(Resource.Id.DayArrowDown);
            monthDown = FindViewById<RelativeLayout>(Resource.Id.MonthArrowDown);
            yearDown = FindViewById<RelativeLayout>(Resource.Id.YearArrowDown);

            dayUp.Click += ArrowModify;
            monthUp.Click += ArrowModify;
            yearUp.Click += ArrowModify;

            dayDown.Click += ArrowModify;
            monthDown.Click += ArrowModify;
            yearDown.Click += ArrowModify;





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

            invalidTaskName = FindViewById<TextView>(Resource.Id.invalidTaskName);
            invalidDate = FindViewById<TextView>(Resource.Id.invalidDate);
        }


        /// <summary>
        /// Back to main view, used below task creation view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToMain(object sender, EventArgs e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Android.Content.Context.InputMethodService);
            imm.HideSoftInputFromWindow(taskNameField.WindowToken, 0);
            mainHeader.Visibility = ViewStates.Visible;
            createTaskHeader.Visibility = ViewStates.Gone;
            scrollBase.Visibility = ViewStates.Visible;
            scrollLayout.Visibility = ViewStates.Visible;
            taskCountLayout.Visibility = ViewStates.Visible;
            taskNameField.Text = "";
            dayInput.Text = "";
            monthInput.Text = "";
            yearInput.Text = "";
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
                    CreateTaskElement(task.Text, task.IsDone, task.DueDate);
                }
            }

            UpdateTaskCount();
        }
        
        /// <summary>
        /// Closes task creation menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseCreateView(object sender, EventArgs e)
        {
            string taskname = taskNameField.Text;
            if (mainHeader.Visibility == ViewStates.Gone)
            {
                taskNameField.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                dayInput.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                monthInput.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                yearInput.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                invalidDate.Text = "";
                invalidTaskName.Text = "";
                CreateATask(taskname, null, dayInput.Text, monthInput.Text, yearInput.Text, true);
                if (taskCreated == true)
                {
                    scrollBase.Visibility = ViewStates.Visible;
                    scrollLayout.Visibility = ViewStates.Visible;
                    mainHeader.Visibility = ViewStates.Visible;
                    createTaskHeader.Visibility = ViewStates.Gone;
                    taskCountLayout.Visibility = ViewStates.Visible;


                    taskNameField.Text = "";
                    dayInput.Text = "";
                    monthInput.Text = "";
                    yearInput.Text = "";

                    InputMethodManager imm = (InputMethodManager)GetSystemService(Android.Content.Context.InputMethodService);
                    imm.HideSoftInputFromWindow(taskNameField.WindowToken, 0);
                }
            }
        }
        
        /// <summary>
        /// Opens task creation menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCreateView(object sender, EventArgs e)
        {

            taskNameField.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            dayInput.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            monthInput.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            yearInput.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            invalidDate.Text = "";
            invalidTaskName.Text = "";


            if (createTaskHeader.Visibility == ViewStates.Gone)
            {
                mainHeader.Visibility = ViewStates.Gone;
                createTaskHeader.Visibility = ViewStates.Visible;
                scrollBase.Visibility = ViewStates.Gone;
                scrollLayout.Visibility = ViewStates.Gone;
                taskCountLayout.Visibility = ViewStates.Gone;
                backToMain.Visibility = ViewStates.Visible;

                dayInput.Text = thisDay.ToString();
                monthInput.Text = thisMonth.ToString();
                yearInput.Text = thisYear.ToString();

            }

        }

        /// <summary>
        /// Triggers show all menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAll(object sender, EventArgs e)
        {
            if (calendarView.Visibility == ViewStates.Visible)
            {
                scrollLayout.RemoveAllViews();
                mainInfo.Visibility = ViewStates.Gone;
                calendarView.Visibility = ViewStates.Gone;
                sortByDueDate.Visibility = ViewStates.Visible;
                sortByCreationDate.Visibility = ViewStates.Visible;
                taskList = TaskItem.SortListByIsDone(taskList);
                foreach (TaskItem t in taskList)
                {
                    CreateTaskElement(t.Text, t.IsDone, t.DueDate);
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
            if (searchBar.Visibility == ViewStates.Visible)
            {
                searchField.Text = "";
                scrollLayout.RemoveAllViews();
                calendarView.Visibility = ViewStates.Gone;
                searchBar.Visibility = ViewStates.Gone;
                searchField.Visibility = ViewStates.Visible;
                searchField.FocusableInTouchMode = true;
                searchField.RequestFocus();
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.ToggleSoftInput(ShowFlags.Forced, 0);
                UpdateTaskCount();
            }
            else if (searchField.Visibility == ViewStates.Visible)
            {
                searchBar.Visibility = ViewStates.Visible;
                searchField.Visibility = ViewStates.Gone;
                calendarView.Visibility = ViewStates.Visible;
                InputMethodManager imm = (InputMethodManager)GetSystemService(Android.Content.Context.InputMethodService);
                imm.HideSoftInputFromWindow(taskNameField.WindowToken, 0);
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
        private void ButtonAction(object sender, EventArgs e)
        {
            var button = (RelativeLayout)sender;
            switch (button.Id)
            {
                case Resource.Id.SettingsButton:
                    Intent settingsStarter = new Intent(this, typeof(SettingsActivity));
                    StartActivity(settingsStarter);
                    break;
            }

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
                string DayInputText = dayInput.Text;
                int daySelected = int.Parse(DayInputText);
                string MonthInputText = monthInput.Text;
                int MonthSelected = int.Parse(MonthInputText);
                string YearInputText = yearInput.Text;
                int YearSelected = int.Parse(YearInputText);
                switch (button.Id)
                {
                    case Resource.Id.DayArrowUp:
                        daySelected++;
                        if (daySelected > DateTime.DaysInMonth(YearSelected, MonthSelected))
                        {
                            daySelected--;
                        }
                        dayInput.Text = daySelected.ToString();
                        break;

                    case Resource.Id.DayArrowDown:
                        daySelected--;
                        if (daySelected < 1)
                        {
                            daySelected++;
                        }
                        dayInput.Text = daySelected.ToString();
                        break;

                    case Resource.Id.MonthArrowUp:
                        MonthSelected++;
                        if (MonthSelected > 12)
                        {
                            MonthSelected = 12;
                        }
                        monthInput.Text = MonthSelected.ToString();
                        break;

                    case Resource.Id.YearArrowUp:
                        YearSelected++;
                        yearInput.Text = YearSelected.ToString();
                        break;

                    case Resource.Id.YearArrowDown:
                        YearSelected--;
                        if (YearSelected < DateTime.Today.Year)
                        {
                            YearSelected = DateTime.Today.Year;
                        }
                        yearInput.Text = YearSelected.ToString();
                        break;

                    case Resource.Id.MonthArrowDown:
                        MonthSelected--;
                        if (MonthSelected < 1)
                        {
                            MonthSelected = 1;
                        }
                        monthInput.Text = MonthSelected.ToString();
                        break;



                }
            }
            catch
            {

            }
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
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Android.Content.Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.edit_popup, null);
            dayInputEdit = view.FindViewById<EditText>(Resource.Id.EditDayInput);
            monthInputEdit = view.FindViewById<EditText>(Resource.Id.EditMonthInput);
            yearInputEdit = view.FindViewById<EditText>(Resource.Id.EditYearInput);
            dayInputEdit.Text = DateTime.Today.Day.ToString();
            monthInputEdit.Text = DateTime.Today.Month.ToString();
            yearInputEdit.Text = DateTime.Today.Year.ToString();
            dayUpEdit = view.FindViewById<RelativeLayout>(Resource.Id.EditDayArrowUp);
            monthUpEdit = view.FindViewById<RelativeLayout>(Resource.Id.EditMonthArrowUp);
            yearUpEdit = view.FindViewById<RelativeLayout>(Resource.Id.EditYearArrowUp);

            dayDownEdit = view.FindViewById<RelativeLayout>(Resource.Id.EditDayArrowDown);
            monthDownEdit = view.FindViewById<RelativeLayout>(Resource.Id.EditMonthArrowDown);
            yearDownEdit = view.FindViewById<RelativeLayout>(Resource.Id.EditYearArrowDown);
            invalidEditTaskName = view.FindViewById<TextView>(Resource.Id.invalidEditName);
            invalidEditDate = view.FindViewById<TextView>(Resource.Id.invalidEditDate);

            dayUpEdit.Click += ArrowModify;
            monthUpEdit.Click += ArrowModify;
            yearUpEdit.Click += ArrowModify;

            dayDownEdit.Click += ArrowModify;
            monthDownEdit.Click += ArrowModify;
            yearDownEdit.Click += ArrowModify;

            foreach (TaskItem task in taskList)
            {
                if (task.Text == oldTaskName)
                {
                    dayInputEdit.Text = task.DueDate.Day.ToString();
                    monthInputEdit.Text = task.DueDate.Month.ToString();
                    yearInputEdit.Text = task.DueDate.Year.ToString();
                    break;
                }
            }


            view.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
            alert.SetView(view);
            alert.Show();
            alert.Window.SetLayout(DpToPx(300), DpToPx(320));
            alert.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
            editTaskField = view.FindViewById<EditText>(Resource.Id.EditTaskInput);
            editTaskField.Text = oldTaskName;
            Button editConfirm = view.FindViewById<Button>(Resource.Id.editPopupConfirm);
            editConfirm.Text = "OK";
            editConfirm.Click += (s, e) =>
            {
                editTaskField.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                dayInputEdit.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                monthInputEdit.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                yearInputEdit.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                CreateATask(editTaskField.Text, oldTaskName, dayInputEdit.Text, monthInputEdit.Text, yearInputEdit.Text, false);

                if (taskCreated == true)
                {
                    alert.Dismiss();
                }

            };

            Button editCancel = view.FindViewById<Button>(Resource.Id.editPopupCancel);
            editCancel.Click += (s, e) =>
            {
                alert.Dismiss();
            };
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
                VibrationEffect invalidHaptic = VibrationEffect.CreateOneShot(100, VibrationEffect.DefaultAmplitude);
                Vibrator hapticSystem = (Vibrator)GetSystemService(VibratorService);
                hapticSystem.Cancel();
                hapticSystem.Vibrate(invalidHaptic);
            }


            RelativeLayout button = (RelativeLayout)sender;
            TextView taskName = (TextView)button.GetChildAt(1);
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

            FindViewById<TextView>(Resource.Id.date1number).Text = today.Day.ToString();
            FindViewById<TextView>(Resource.Id.date1str).Text = dayNamesShort[(int)today.DayOfWeek];

            FindViewById<TextView>(Resource.Id.date2number).Text = today.AddDays(1).Day.ToString();
            FindViewById<TextView>(Resource.Id.date2str).Text = dayNamesShort[(int)today.AddDays(1).DayOfWeek];

            FindViewById<TextView>(Resource.Id.date3number).Text = today.AddDays(2).Day.ToString();
            FindViewById<TextView>(Resource.Id.date3str).Text = dayNamesShort[(int)today.AddDays(2).DayOfWeek];

            FindViewById<TextView>(Resource.Id.date4number).Text = today.AddDays(3).Day.ToString();
            FindViewById<TextView>(Resource.Id.date4str).Text = dayNamesShort[(int)today.AddDays(3).DayOfWeek];

            FindViewById<TextView>(Resource.Id.date5number).Text = today.AddDays(4).Day.ToString();
            FindViewById<TextView>(Resource.Id.date5str).Text = dayNamesShort[(int)today.AddDays(4).DayOfWeek];

            FindViewById<TextView>(Resource.Id.date6number).Text = today.AddDays(5).Day.ToString();
            FindViewById<TextView>(Resource.Id.date6str).Text = dayNamesShort[(int)today.AddDays(5).DayOfWeek];

            FindViewById<TextView>(Resource.Id.date7number).Text = today.AddDays(6).Day.ToString();
            FindViewById<TextView>(Resource.Id.date7str).Text = dayNamesShort[(int)today.AddDays(6).DayOfWeek];
        }

        /// <summary>
        /// Triggers the colors with the calendar items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarSelector(object sender, EventArgs e)
        {
            scrollLayout.RemoveAllViews();
            var button = (RelativeLayout)sender;
            missedTasksBtn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
            switch (button.Id)
            {


                case Resource.Id.date1btn:
                    activeDate = 1;
                    date1Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    ShowDatestasks(DateTime.Today);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date2btn:
                    activeDate = 2;
                    date2Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    ShowDatestasks(DateTime.Today.AddDays(1));
                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date3btn:
                    activeDate = 3;
                    date3Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    ShowDatestasks(DateTime.Today.AddDays(2));
                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;
                case Resource.Id.date4btn:
                    activeDate = 4;
                    date4Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    ShowDatestasks(DateTime.Today.AddDays(3));
                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date5btn:
                    activeDate = 5;
                    date5Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    ShowDatestasks(DateTime.Today.AddDays(4));
                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date6btn:
                    activeDate = 6;
                    date6Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    ShowDatestasks(DateTime.Today.AddDays(5));
                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date7btn:
                    activeDate = 7;
                    date7Btn.BackgroundTintList = GetColorStateList(GetStyle());
                    ShowDatestasks(DateTime.Today.AddDays(6));
                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;


            }
            UpdateTaskCount();
        }

        /// <summary>
        /// Dynamically creates task element
        /// </summary>
        /// <param name="taskName"></param>
        private void CreateTaskElement(string taskName, bool isTrue, DateTime dueDate)
        {

            RelativeLayout cardBG = new RelativeLayout(this);
            Drawable rounded50 = GetDrawable(Resource.Drawable.rounded50px);
            cardBG.Background = rounded50;
            cardBG.SetPadding(DpToPx(20), 0, 0, 0);
            cardBG.Id = View.GenerateViewId();
            cardBG.LayoutTransition = new LayoutTransition();
            RelativeLayout.LayoutParams cardparams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
            cardparams.SetMargins(DpToPx(20), 0, DpToPx(20), DpToPx(20));

            cardBG.LayoutParameters = cardparams;
            cardBG.LongClick += HoldTaskElement;
            cardBG.Click += ExpandCard;


            Button toggleBtn = new Button(this);
            Drawable toggleDefault = GetDrawable(Resource.Drawable.task_radio_button);
            Drawable toggleActive = GetDrawable(Resource.Drawable.task_radio_button_active);
            toggleBtn.Background = toggleDefault;
            RelativeLayout.LayoutParams buttonparams = new RelativeLayout.LayoutParams(DpToPx(45), DpToPx(45));
            buttonparams.SetMargins(0, DpToPx(17), DpToPx(10), DpToPx(17));
            //buttonparams.AddRule(LayoutRules.CenterVertical);
            toggleBtn.LayoutParameters = buttonparams;
            toggleBtn.Id = View.GenerateViewId();
            toggleBtn.Click += TaskToggle;
            toggleBtn.Tag = "Inactive";


            TextView header = new TextView(this);
            header.Text = taskName;
            header.TextSize = DpToPx(6);
            header.SetTypeface(Resources.GetFont(Resource.Font.inter_bold), Android.Graphics.TypefaceStyle.Normal);
            RelativeLayout.LayoutParams headerparams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            headerparams.SetMargins(0, DpToPx(28), 0, 0);
            //headerparams.AddRule(LayoutRules.CenterVertical);
            headerparams.AddRule(LayoutRules.RightOf, toggleBtn.Id);
            header.LayoutParameters = headerparams;


            TextView date = new TextView(this);
            date.Text = dueDate.Day.ToString() + "." + dueDate.Month.ToString() + "." + dueDate.Year.ToString();
            RelativeLayout.LayoutParams dateparams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            dateparams.AddRule(LayoutRules.CenterHorizontal);
            dateparams.AddRule(LayoutRules.AlignParentBottom);
            dateparams.SetMargins(0, 0, 0, DpToPx(5));
            date.LayoutParameters = dateparams;
            date.TextSize = DpToPx(6);
            date.SetTypeface(Resources.GetFont(Resource.Font.inter_semibold), Android.Graphics.TypefaceStyle.Normal);
            if (isTrue == true)
            {
                toggleBtn.Background = toggleActive;
            }
            scrollLayout.AddView(cardBG);
            cardBG.AddView(toggleBtn);
            cardBG.AddView(header);
            cardBG.AddView(date);
            date.Visibility = ViewStates.Gone;
            try
            {
                elementIds.Add(taskName, cardBG.Id);
            }
            catch
            {
                elementIds[taskName] = cardBG.Id;
            }
            CheckIfMissedAnymore();
        }

        private void ExpandCard(object sender, EventArgs e)
        {
            RelativeLayout card = (RelativeLayout)sender;
            if (card.LayoutParameters.Height == RelativeLayout.LayoutParams.WrapContent)
            {
                LinearLayout.LayoutParams heightParam = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, card.Height + DpToPx(30));
                heightParam.SetMargins(DpToPx(20), 0, DpToPx(20), DpToPx(20));
                card.LayoutParameters = heightParam;
                card.GetChildAt(2).Visibility = ViewStates.Visible;
            }
            else if (card.LayoutParameters.Height != RelativeLayout.LayoutParams.WrapContent)
            {
                LinearLayout.LayoutParams heightParam = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                heightParam.SetMargins(DpToPx(20), 0, DpToPx(20), DpToPx(20));
                card.LayoutParameters = heightParam;
                card.GetChildAt(2).Visibility = ViewStates.Gone;
            }

        }
        
        /// <summary>
        /// Toggle between done and not done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskToggle(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            RelativeLayout buttonParent = (RelativeLayout)button.Parent;
            Drawable active = GetDrawable(Resource.Drawable.task_radio_button_active);
            Drawable inactive = GetDrawable(Resource.Drawable.task_radio_button);
            TextView header = (TextView)buttonParent.GetChildAt(1);
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

        private void CreateTaskItem(string name, DateTime dueDate)
        {
            TaskItem task = new TaskItem(DateTime.Now);
            task.Text = name;
            task.DueDate = dueDate;
            taskList.Add(task);
            file.WriteFile(taskList);
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
                    foreach (TaskItem task in TaskItem.SortListByDueDate(taskList))
                    {
                        CreateTaskElement(task.Text, task.IsDone, task.DueDate);
                    }

                    break;

                case Resource.Id.SortByCreationDate:
                    sortByCreationDate.BackgroundTintList = GetColorStateList(GetStyle());

                    scrollLayout.RemoveAllViews();
                    foreach (TaskItem task in TaskItem.SortListByCreationDate(taskList))
                    {
                        CreateTaskElement(task.Text, task.IsDone, task.DueDate);
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
                    CreateTaskElement(t.Text, t.IsDone, t.DueDate);
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
                InvalidInput(taskNameField, invalidTaskName, "Tehtävän nimi ei voi olla tyhjä");
                InvalidInput(editTaskField, invalidEditTaskName, "Tehtävän nimi ei voi olla tyhjä");

                didFail = true;
            }

            if (isNew == true)
            {
                foreach (TaskItem t in taskList)
                {
                    if (t.Text.ToLower() == taskName.ToLower())
                    {
                        InvalidInput(taskNameField, invalidTaskName, "Tämän niminen tehtävä on jo olemassa");
                        InvalidInput(editTaskField, invalidEditTaskName, "Tämän niminen tehtävä on jo olemassa");

                        didFail = true;
                    }
                }
            }

            if (!int.TryParse(day, out int intDay))
            {
                InvalidInput(dayInput, invalidDate, "Virheellinen päivä");
                InvalidInput(dayInputEdit, invalidEditDate, "Virheellinen päivä");

                didFail = true;

            }

            if (!int.TryParse(month, out int intMonth))
            {
                InvalidInput(monthInput, invalidDate, "Virheellinen kuukausi");
                InvalidInput(monthInputEdit, invalidEditDate, "Virheellinen kuukausi");

                didFail = true;
            }

            if (!int.TryParse(year, out int intYear))
            {
                InvalidInput(yearInput, invalidDate, "Virheellinen vuosi");
                InvalidInput(yearInputEdit, invalidEditDate, "Virheellinen vuosi");

                didFail = true;
            }

            if (intDay < 1)
            {
                InvalidInput(dayInput, invalidDate, "Päivä ei voi olla pienempi kuin 1");
                InvalidInput(dayInputEdit, invalidEditDate, "Päivä ei voi olla pienempi kuin 1");

                didFail = true;
            }

            if (intMonth < 1)
            {
                InvalidInput(monthInput, invalidDate, "Kuukausi ei voi olla pienempi kuin 1");
                InvalidInput(monthInputEdit, invalidEditDate, "Kuukausi ei voi olla pienempi kuin 1");

                didFail = true;
            }

            if (intYear < 1)
            {
                InvalidInput(yearInput, invalidDate, "Vuosi ei voi olla pienempi kuin 1");
                InvalidInput(yearInputEdit, invalidEditDate, "Vuosi ei voi olla pienempi kuin 1");

                didFail = true;
            }

            if (intMonth > 12)
            {
                InvalidInput(monthInput, invalidDate, "Kuukausi ei voi olla suurempi kuin 12");
                InvalidInput(monthInputEdit, invalidEditDate, "Kuukausi ei voi olla suurempi kuin 12");

                didFail = true;
            }

            if (intDay < DateTime.Today.Day)
            {
                InvalidInput(dayInput, invalidDate, "Päivä ei voi olla menneisyydessä");
                InvalidInput(dayInputEdit, invalidEditDate, "Päivä ei voi olla menneisyydessä");

                didFail = true;
            }

            if (intMonth < DateTime.Today.Month)
            {
                InvalidInput(monthInput, invalidDate, "Kuukausi ei voi olla menneisyydessä");
                InvalidInput(monthInputEdit, invalidEditDate, "Kuukausi ei voi olla menneisyydessä");

                didFail = true;
            }

            if (intYear < DateTime.Today.Year)
            {
                InvalidInput(yearInput, invalidDate, "Vuosi ei voi olla menneisyydessä");
                InvalidInput(yearInputEdit, invalidEditDate, "Vuosi ei voi olla menneisyydessä");

                didFail = true;
            }

            if (intDay > DateTime.MaxValue.Day)
            {
                InvalidInput(dayInput, invalidDate, "Liian suuri päivä");
                InvalidInput(dayInputEdit, invalidEditDate, "Liian suuri päivä");

                didFail = true;
            }

            if (intMonth > DateTime.MaxValue.Month)
            {
                InvalidInput(monthInput, invalidDate, "Liian suuri kuukausi");
                InvalidInput(monthInputEdit, invalidEditDate, "Liian suuri kuukausi");

                didFail = true;
            }

            if (intYear > DateTime.MaxValue.Year)
            {
                InvalidInput(yearInput, invalidDate, "Liian suuri vuosi");
                InvalidInput(yearInputEdit, invalidEditDate, "Liian suuri vuosi");

                didFail = true;
            }

            if (!IsDayInMonth(intDay, intMonth, intYear))
            {
                InvalidInput(dayInput, invalidDate, "Päivä ei kuulu annettuun kuukauteen");
                InvalidInput(dayInputEdit, invalidEditDate, "Päivä ei kuulu annettuun kuukauteen");

                didFail = true;
            }

            if (didFail == false)
            {
                DateTime dueDate = new DateTime(intYear, intMonth, intDay);
                DeleteTaskItem(oldTaskName);
                CreateTaskItem(taskName, dueDate);

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
                    VibrationEffect invalidHaptic = VibrationEffect.CreateOneShot(200, VibrationEffect.DefaultAmplitude);
                    Vibrator hapticSystem = (Vibrator)GetSystemService(VibratorService);
                    hapticSystem.Cancel();
                    hapticSystem.Vibrate(invalidHaptic);
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
        public override void OnBackPressed()
        {
            this.FinishAffinity();
        }
    }
}
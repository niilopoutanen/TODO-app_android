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

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private int activeDate = 1;
        private string currentTheme;
        private bool guideDone;
        Button btnCreateTask;
        Button btnAddTask;

        LinearLayout header;
        LinearLayout mainHeader;
        LinearLayout createTaskHeader;
        HorizontalScrollView calendarView;
        Button showAll;
        Button searchBar;
        LinearLayout navBar;
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

        RelativeLayout mainInfo;
        
        Dictionary<string, int> elementIds = new Dictionary<string, int>();


        private static FileClass file = new FileClass();
        private List<TaskItem> taskList = new List<TaskItem>();
        private int amountOfMissed;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            FirebaseAnalytics.GetInstance(this);
            LoadSettings();
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            try
            {
                taskList = file.ReadFile();
                taskList = TaskItem.SortListByIsDone(taskList);
            }
            catch (System.NullReferenceException)
            {
                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();

                LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Android.Content.Context.LayoutInflaterService);
                View view = inflater.Inflate(Resource.Layout.dialog_popup, null);
                view.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
                alert.SetView(view);
                alert.Show();
                alert.Window.SetLayout(DpToPx(300), DpToPx(150));
                alert.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
                Button confirm = view.FindViewById<Button>(Resource.Id.PopupConfirm);
                confirm.Text = "Close application";
                TextView header = view.FindViewById<TextView>(Resource.Id.PopupHeader);
                header.Text = "Error";
                TextView desc = view.FindViewById<TextView>(Resource.Id.PopupDescription);
                desc.Text = "Sorry. You have to restart";
                confirm.Click += (s, e) =>
                {

                    JavaSystem.Exit(0);
                };

                Button cancel = view.FindViewById<Button>(Resource.Id.PopupCancel);
                cancel.Visibility = ViewStates.Gone;
                
            }



            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


            InitializeElements();
            CalendarDater();


            

            amountOfMissed = 0;
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
            btnCreateTask = FindViewById<Button>(Resource.Id.CreateTask);
            btnCreateTask.Click += OpenCreateView;
            btnAddTask = FindViewById<Button>(Resource.Id.AddTask);
            btnAddTask.Click += CloseCreateView;
            header = FindViewById<LinearLayout>(Resource.Id.Header);
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
            navBar = FindViewById<LinearLayout>(Resource.Id.NavBar);
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
        }


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
        private void UpdateTaskCount()
        {
            int elementCount = scrollLayout.ChildCount;
            if (elementCount == 1)
            {
                taskCount.Text = elementCount.ToString() + " tehtävä";
            }
            else
            {
                taskCount.Text = elementCount.ToString() + " tehtävää";
            }
        }

        private void SearchChanged(object sender, EventArgs e)
        {
            TextView field = (TextView)sender;


            string fieldText = searchField.Text;
            scrollLayout.RemoveAllViews();
            foreach(TaskItem task in taskList)
            {
                if(task.Text.Contains(fieldText))
                {
                    CreateTaskElement(task.Text, task.IsDone, task.DueDate);
                }
            }
        }

        private void CloseCreateView(object sender, EventArgs e)
        {
            string taskname = taskNameField.Text;
            int day;
            int month;
            int year;
            DateTime dueDate;
            if (mainHeader.Visibility == ViewStates.Gone)
            {
                if (string.IsNullOrWhiteSpace(taskname))
                {
                    OpenPopup(GetString(Resource.String.invalidName), GetString(Resource.String.invalidNameDesc), "OK");
                    return;
                }

                foreach (TaskItem t in taskList)
                {
                    if (t.Text.ToLower() == taskname.ToLower())
                    {
                        OpenPopup(GetString(Resource.String.invalidName), GetString(Resource.String.nameExists), "OK");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(dayInput.Text) && !string.IsNullOrWhiteSpace(monthInput.Text) && !string.IsNullOrWhiteSpace(yearInput.Text))
                {
                    try
                    {
                        day = int.Parse(dayInput.Text);
                        month = int.Parse(monthInput.Text);
                        year = int.Parse(yearInput.Text);
                    }
                    catch
                    {
                        OpenPopup(GetString(Resource.String.invalidValue), GetString(Resource.String.invalidDate), "OK");
                        return;
                    }

                    if (day < 1 || month < 1 || year < 1)
                    {
                        OpenPopup(GetString(Resource.String.invalidValue), GetString(Resource.String.dateDoesntExist), "OK");
                        return;
                    }

                    else if (month > 12)
                    {
                        OpenPopup(GetString(Resource.String.invalidValue), GetString(Resource.String.dateDoesntExist), "OK");
                        return;
                    }

                    else if (!IsDayInMonth(day, month, year))
                    {
                        OpenPopup(GetString(Resource.String.invalidValue), GetString(Resource.String.dateDoesntExist), "OK");
                        return;
                    }
                    else
                    {
                        dueDate = new DateTime(year, month, day);

                        if (dueDate < DateTime.Today)
                        {
                            OpenPopup(GetString(Resource.String.invalidValue), GetString(Resource.String.dateInPast), "OK");
                            return;
                        }

                        else if (dueDate > DateTime.MaxValue)
                        {
                            OpenPopup(GetString(Resource.String.invalidValue), GetString(Resource.String.dateTooBig), "OK");
                            return;
                        }

                        else
                        {
                            CreateTaskItem(taskNameField.Text, dueDate);
                            file.WriteFile(taskList);

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

                            scrollBase.Visibility = ViewStates.Visible;
                            scrollLayout.Visibility = ViewStates.Visible;
                            mainHeader.Visibility = ViewStates.Visible;
                            createTaskHeader.Visibility = ViewStates.Gone;
                            taskCountLayout.Visibility = ViewStates.Visible;


                            taskNameField.Text = "";
                            dayInput.Text = "";
                            monthInput.Text = "";
                            yearInput.Text = "";
                        }
                    }
                }
                else
                {
                    OpenPopup(GetString(Resource.String.invalidValue), GetString(Resource.String.invalidDate), "OK");
                    return;
                }

                InputMethodManager imm = (InputMethodManager)GetSystemService(Android.Content.Context.InputMethodService);
                imm.HideSoftInputFromWindow(taskNameField.WindowToken, 0);
            }



        }

        private void OpenCreateView(object sender, EventArgs e)
        {


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
        private void ShowAll(object sender, EventArgs e)
        {
            if (calendarView.Visibility == ViewStates.Visible)
            {
                scrollLayout.RemoveAllViews();
                mainInfo.Visibility = ViewStates.Gone;
                calendarView.Visibility = ViewStates.Gone;
                sortByDueDate.Visibility = ViewStates.Visible;
                sortByCreationDate.Visibility = ViewStates.Visible;
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
                    if(daySelected > DateTime.DaysInMonth(YearSelected, MonthSelected))
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


        /// <summary>
        /// Use this to show a popup on screen. Provide a text for the header, description, and the OK-button.
        /// </summary>
        /// <param name="Header">Header of the popup.</param>
        /// <param name="Desc">Description of the popup.</param>
        /// <param name="YesText">Text of the OK-button.</param>
        private void OpenPopup(string Header, string Desc, string YesText)
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Android.Content.Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.dialog_popup, null);
            view.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
            alert.SetView(view);
            alert.Show();
            alert.Window.SetLayout(DpToPx(300), DpToPx(150));
            alert.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
            Button confirm = view.FindViewById<Button>(Resource.Id.PopupConfirm);
            confirm.Text = YesText;
            TextView header = view.FindViewById<TextView>(Resource.Id.PopupHeader);
            header.Text = Header;
            TextView desc = view.FindViewById<TextView>(Resource.Id.PopupDescription);
            desc.Text = Desc;
            confirm.Click += (s, e) =>
            {
                alert.Dismiss();
            };

            Button cancel = view.FindViewById<Button>(Resource.Id.PopupCancel);
            cancel.Visibility = ViewStates.Gone;
        }

        /// <summary>
        /// This happens when task element is pressed & held down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HoldTaskElement(object sender, EventArgs e)
        {
            RelativeLayout button = (RelativeLayout)sender;
            button.BackgroundTintList = GetColorStateList(GetStyle());
            CheckIfMissedAnymore();

            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Android.Content.Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.dialog_popup, null);
            view.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
            alert.SetView(view);
            alert.Show();
            alert.Window.SetLayout(DpToPx(300), DpToPx(150));
            alert.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
            Button confirm = view.FindViewById<Button>(Resource.Id.PopupConfirm);
            TextView header = view.FindViewById<TextView>(Resource.Id.PopupHeader);
            header.Text = GetString(Resource.String.deleteTaskHeader);
            TextView desc = view.FindViewById<TextView>(Resource.Id.PopupDescription);
            desc.Text = GetString(Resource.String.deleteTaskDescription);
            TextView name = (TextView)button.GetChildAt(1);

            confirm.Click += (s, e) =>
            {
                DeleteTaskItem(name.Text);
                button.RemoveAllViews();
                scrollLayout.RemoveView(button);
                alert.Dismiss();
                UpdateTaskCount();
                CheckIfMissedAnymore();
            };

            Button cancel = view.FindViewById<Button>(Resource.Id.PopupCancel);
            cancel.Click += (s, e) =>
            {
                button.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
                alert.Dismiss();
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
                {0, "Sunnuntai"},
                {1, "Maanantai"},
                {2, "Tiistai"},
                {3, "Keskiviikko"},
                {4, "Torstai"},
                {5, "Perjantai"},
                {6, "Lauantai"}
            };
            string todaystr = dayNames[(int)today.DayOfWeek] + "\n" + today.Day.ToString() + "." + today.Month.ToString() + "." + today.Year.ToString();
            todayHeader.Text = todaystr;

            FindViewById<TextView>(Resource.Id.date1number).Text = today.Day.ToString();
            FindViewById<TextView>(Resource.Id.date1str).Text = dayNames[(int)today.DayOfWeek].Substring(0, 2);

            FindViewById<TextView>(Resource.Id.date2number).Text = today.AddDays(1).Day.ToString();
            FindViewById<TextView>(Resource.Id.date2str).Text = dayNames[(int)today.AddDays(1).DayOfWeek].Substring(0, 2);

            FindViewById<TextView>(Resource.Id.date3number).Text = today.AddDays(2).Day.ToString();
            FindViewById<TextView>(Resource.Id.date3str).Text = dayNames[(int)today.AddDays(2).DayOfWeek].Substring(0, 2);

            FindViewById<TextView>(Resource.Id.date4number).Text = today.AddDays(3).Day.ToString();
            FindViewById<TextView>(Resource.Id.date4str).Text = dayNames[(int)today.AddDays(3).DayOfWeek].Substring(0, 2);

            FindViewById<TextView>(Resource.Id.date5number).Text = today.AddDays(4).Day.ToString();
            FindViewById<TextView>(Resource.Id.date5str).Text = dayNames[(int)today.AddDays(4).DayOfWeek].Substring(0, 2);

            FindViewById<TextView>(Resource.Id.date6number).Text = today.AddDays(5).Day.ToString();
            FindViewById<TextView>(Resource.Id.date6str).Text = dayNames[(int)today.AddDays(5).DayOfWeek].Substring(0, 2);

            FindViewById<TextView>(Resource.Id.date7number).Text = today.AddDays(6).Day.ToString();
            FindViewById<TextView>(Resource.Id.date7str).Text = dayNames[(int)today.AddDays(6).DayOfWeek].Substring(0, 2);
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
            RelativeLayout.LayoutParams cardparams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, DpToPx(80));
            cardparams.SetMargins(DpToPx(20), 0, DpToPx(20), DpToPx(20));
            
            cardBG.LayoutParameters = cardparams;
            cardBG.LongClick += HoldTaskElement;
            cardBG.Click += ExpandCard;


            Button toggleBtn = new Button(this);
            Drawable toggleDefault = GetDrawable(Resource.Drawable.task_radio_button);
            Drawable toggleActive = GetDrawable(Resource.Drawable.task_radio_button_active);
            toggleBtn.Background = toggleDefault;
            RelativeLayout.LayoutParams buttonparams = new RelativeLayout.LayoutParams(DpToPx(45), DpToPx(45));
            buttonparams.SetMargins(0, DpToPx(17), DpToPx(10), 0);
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
            if (card.Height == DpToPx(100))
            {
                LinearLayout.LayoutParams heightParam = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPx(80));
                heightParam.SetMargins(DpToPx(20), 0, DpToPx(20), DpToPx(20));
                card.LayoutParameters = heightParam;
                card.GetChildAt(2).Visibility = ViewStates.Gone;
            }
            else if (card.Height == DpToPx(80))
            {
                LinearLayout.LayoutParams heightParam = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPx(100));
                heightParam.SetMargins(DpToPx(20), 0, DpToPx(20), DpToPx(20));
                card.LayoutParameters = heightParam;
                card.GetChildAt(2).Visibility = ViewStates.Visible;
            }

        }
        /// <summary>
        /// Not needed right now, use if you need
        /// </summary>
        /// <param name="taskToDelete"></param>
        private void DeleteTaskElement(string taskToDelete)
        {
            RelativeLayout layoutToDelete = FindViewById<RelativeLayout>(elementIds[taskToDelete]);

            try
            {
                layoutToDelete.RemoveAllViews();
                scrollLayout.RemoveView(layoutToDelete);
                UpdateTaskCount();
            }
            catch
            {
                Console.Write("error/ item not found");
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
                    if(t.IsDone == true)
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
            TaskItem task = new TaskItem();
            task.CreationTime = DateTime.Now;
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
            int amountOfDaysInMonth = DateTime.DaysInMonth(year, month);
            if (day > amountOfDaysInMonth)
            {
                return false;
            }
            else if (day < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

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
                    foreach(TaskItem task in TaskItem.SortListByDueDate(taskList))
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
    }
}
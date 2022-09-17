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
using Android;
using Android.Icu.Math;
using Android.Views.Animations;
using Android.Animation;
using Android.Provider;
using Android.Content;
using static Android.Widget.TextView;
using System.Runtime.Remoting.Contexts;
using AndroidX.Core.Content.Resources;
using Android.Content.Res;
using Android.Views.InputMethods;
using Android.Graphics;
using System.Drawing.Imaging;
using Android.Telephony;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private int activeDate;
        private string currentTheme;
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
        RelativeLayout taskCountLayout;
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

        Dictionary<string, int> elementIds = new Dictionary<string, int>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            LoadSettings();
            currentTheme = "mainViolet";
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            InitializeElements();
            CalendarDater();
            UpdateTaskCount();
            GetStyle();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private int GetStyle()
        {
            if(currentTheme == "mainblue")
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
                return 0;
            }
        }

        private void LoadSettings()
        {
            ISharedPreferences colorTheme = GetSharedPreferences("ColorTheme", 0);
            string color = colorTheme.GetString("colorTheme", default);
            if (color == "blue")
            {
                SetTheme(Resource.Style.MainBlue);
            }
            else if (color == "green")
            {
                SetTheme(Resource.Style.MainGreen);
            }
            else if (color == "orange")
            {
                SetTheme(Resource.Style.MainOrange);
            }
            else if (color == "violet")
            {
                SetTheme(Resource.Style.MainViolet);
            }
            else if (color == "red")
            {
                SetTheme(Resource.Style.MainRed);
            }
        }
        /// <summary>
        /// Put all element connections here for cleaner code
        /// </summary>
        private void InitializeElements()
        {

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
        taskCountLayout = FindViewById<RelativeLayout>(Resource.Id.taskCountLayout);
        taskCount = FindViewById<TextView>(Resource.Id.taskCountText);

        settingsOpen = FindViewById<RelativeLayout>(Resource.Id.SettingsButton);
        settingsOpen.Click += ButtonAction;
        searchField = FindViewById<EditText>(Resource.Id.SearchField);
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
    }
        private void UpdateTaskCount()
        {
            int elementCount = scrollLayout.ChildCount;
            if(elementCount == 1)
            {
                taskCount.Text = elementCount.ToString() + " tehtävä";
            }
            else
            {
                taskCount.Text = elementCount.ToString() + " tehtävää";
            }
        }
       
        private void CloseCreateView(object sender, EventArgs e)
        {
            if(mainHeader.Visibility == ViewStates.Gone)
            {
                string taskname = taskNameField.Text;
                if (taskname == "")
                {
                    OpenPopup(GetString(Resource.String.invalidName), GetString(Resource.String.invalidNameDesc), "OK");
                    return;
                }
                CreateTaskElement(taskname);
                UpdateTaskCount();
                mainHeader.Visibility = ViewStates.Visible;
                createTaskHeader.Visibility = ViewStates.Gone;
                scrollLayout.Visibility = ViewStates.Visible;
                taskCountLayout.Visibility = ViewStates.Visible;
                taskNameField.Text = "";

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
                scrollLayout.Visibility = ViewStates.Gone;
                taskCountLayout.Visibility = ViewStates.Gone;


                dayInput.Text = thisDay.ToString();
                monthInput.Text = thisMonth.ToString();
                yearInput.Text = thisYear.ToString();
                
            }

        }
        private void ShowAll(object sender, EventArgs e)
        {
            if(calendarView.Visibility == ViewStates.Visible)
            {
                calendarView.Visibility = ViewStates.Gone;
            }
            else if(calendarView.Visibility == ViewStates.Gone)
            {
                calendarView.Visibility = ViewStates.Visible;
            }
        }


        /// <summary>
        /// Call this when you want to toggle between search mode and normal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleSearchMode(object sender, EventArgs e)
        {
            if(searchBar.Visibility == ViewStates.Visible)
            {
                searchBar.Visibility = ViewStates.Gone;
                searchField.Visibility = ViewStates.Visible;
            }
            else if (searchField.Visibility == ViewStates.Visible)
            {
                searchBar.Visibility = ViewStates.Visible;
                searchField.Visibility = ViewStates.Gone;
            }
        }
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
                    dayInput.Text = daySelected.ToString();
                    break;

                case Resource.Id.DayArrowDown:
                    daySelected--;
                    dayInput.Text = daySelected.ToString();
                    break;

                case Resource.Id.MonthArrowUp:
                    MonthSelected++;
                    monthInput.Text = MonthSelected.ToString();
                    break;

                case Resource.Id.YearArrowUp:
                    YearSelected++;
                    yearInput.Text = YearSelected.ToString();
                    break;

                case Resource.Id.YearArrowDown:
                    YearSelected--;
                    yearInput.Text = YearSelected.ToString();
                    break;

                case Resource.Id.MonthArrowDown:
                    MonthSelected--;
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
        private void HoldTaskElement(object sender, EventArgs e)
        {
            RelativeLayout button = (RelativeLayout)sender;
            button.BackgroundTintList = GetColorStateList(GetStyle());

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
            confirm.Click += (s, e) =>
            {
                button.RemoveAllViews();
                scrollLayout.RemoveView(button);
                alert.Dismiss();
                UpdateTaskCount();
            };

            Button cancel = view.FindViewById<Button>(Resource.Id.PopupCancel);
            cancel.Click += (s, e) =>
            {
                button.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
                alert.Dismiss();
            };
            
        }
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
        private void CalendarSelector(object sender, EventArgs e)
        {
            var button = (RelativeLayout)sender;
            switch (button.Id)
            {
                case Resource.Id.date1btn:
                    activeDate = 1;
                    date1Btn.BackgroundTintList = GetColorStateList(GetStyle());

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

                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;
            }
        }


        private void CreateTaskElement(string taskName)
        {
            RelativeLayout cardBG = new RelativeLayout(this);
            Drawable rounded50 = GetDrawable(Resource.Drawable.rounded50px);
            cardBG.Background = rounded50;
            cardBG.SetPadding(DpToPx(20),0,0,0);
            cardBG.Id = View.GenerateViewId();
            RelativeLayout.LayoutParams cardparams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, DpToPx(80));
            cardparams.SetMargins(0, 0, 0, DpToPx(20));
            cardBG.LayoutParameters = cardparams;
            cardBG.LongClick += HoldTaskElement;


            Button toggleBtn = new Button(this);
            Drawable toggleDefault = GetDrawable(Resource.Drawable.task_radio_button);
            toggleBtn.Background = toggleDefault;
            RelativeLayout.LayoutParams buttonparams = new RelativeLayout.LayoutParams(DpToPx(45), DpToPx(45));
            buttonparams.SetMargins(0, 0, DpToPx(10), 0);
            buttonparams.AddRule(LayoutRules.CenterVertical);
            toggleBtn.LayoutParameters = buttonparams;
            toggleBtn.Id = View.GenerateViewId();
            toggleBtn.Click += TaskToggle;
            toggleBtn.Tag = "Inactive";


            TextView header = new TextView(this);
            header.Text = taskName;
            header.TextSize = DpToPx(6);
            header.SetTypeface(header.Typeface, Android.Graphics.TypefaceStyle.Bold);
            RelativeLayout.LayoutParams headerparams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            headerparams.AddRule(LayoutRules.CenterVertical);
            headerparams.AddRule(LayoutRules.RightOf, toggleBtn.Id);
            header.LayoutParameters = headerparams;


            scrollLayout.AddView(cardBG);
            cardBG.AddView(toggleBtn);
            cardBG.AddView(header);
            elementIds.Add(taskName, cardBG.Id);
        }
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
        }

        private void TaskToggle(object sender, EventArgs e)
        {
            Button button =  (Button)sender;
            RelativeLayout buttonParent = (RelativeLayout)button.Parent;
            Drawable active = GetDrawable(Resource.Drawable.task_radio_button_active);
            Drawable inactive = GetDrawable(Resource.Drawable.task_radio_button);

            if (button.Tag.ToString() == "Inactive")
            {
                button.Background = active;
                button.Tag = "Active";
            }
            else if (button.Tag.ToString() == "Active")
            {
                button.Background = inactive;
                button.Tag = "Inactive";
            }
            
            //buttonParent.RemoveAllViews();
            //scrollLayout.RemoveView(buttonParent);
            UpdateTaskCount();
        }

        private int DpToPx(int dpValue)
        {
            int pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dpValue, Resources.DisplayMetrics);
            return pixel;
        }
    }
}
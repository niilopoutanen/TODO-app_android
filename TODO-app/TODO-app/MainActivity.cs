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
using Android.Content.Res;
using Android.Views.InputMethods;


namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private int activeDate;
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
            SetTheme(Resource.Style.MainViolet);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.activity_main);
            CalendarDater();


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
            yearUp = FindViewById<RelativeLayout>(Resource.Id.YearArrowDown);

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
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



       
        private void CloseCreateView(object sender, EventArgs e)
        {
            if(mainHeader.Visibility == ViewStates.Gone)
            {
                string taskname = taskNameField.Text;
                if (taskname == "")
                {
                    return;
                }
                CreateTaskElement(taskname);
                mainHeader.Visibility = ViewStates.Visible;
                createTaskHeader.Visibility = ViewStates.Gone;
                scrollLayout.Visibility = ViewStates.Visible;
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
                    

                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.mainBlue);

                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date2btn:
                    activeDate = 2;
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.mainBlue);

                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date3btn:
                    activeDate = 3;
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.mainBlue);

                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;
                case Resource.Id.date4btn:
                    activeDate = 4;
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.mainBlue);

                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date5btn:
                    activeDate = 5;
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.mainBlue);

                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date6btn:
                    activeDate = 6;
                    date6Btn.BackgroundTintList = GetColorStateList(Resource.Color.mainBlue);

                    date1Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date2Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date3Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date4Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date5Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.colorButton);
                    break;

                case Resource.Id.date7btn:
                    activeDate = 7;
                    date7Btn.BackgroundTintList = GetColorStateList(Resource.Color.mainBlue);

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

            Button toggleBtn = new Button(this);
            Drawable toggleDefault = GetDrawable(Resource.Drawable.task_radio_button);
            toggleBtn.Background = toggleDefault;
            RelativeLayout.LayoutParams buttonparams = new RelativeLayout.LayoutParams(DpToPx(45), DpToPx(45));
            buttonparams.SetMargins(0, 0, DpToPx(10), 0);
            buttonparams.AddRule(LayoutRules.CenterVertical);
            toggleBtn.LayoutParameters = buttonparams;
            toggleBtn.Id = View.GenerateViewId();
            toggleBtn.Click += TaskToggle;


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

            buttonParent.RemoveAllViews();
            scrollLayout.RemoveView(buttonParent);
        }

        private int DpToPx(int dpValue)
        {
            int pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dpValue, Resources.DisplayMetrics);
            return pixel;
        }
    }
}
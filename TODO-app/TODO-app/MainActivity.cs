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

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private bool isTaskCreateVisible = false;
        Button btnCreateTask;
        Button btnAddTask;
        
        LinearLayout header;
        LinearLayout mainHeader;
        LinearLayout createTaskHeader;
        HorizontalScrollView calendarView;
        Button showAll;
        Button searchBar;
        LinearLayout navBar;
        LinearLayout navBarSearch;
        EditText searchField;
        RelativeLayout settingsOpen;

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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
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

            settingsOpen = FindViewById<RelativeLayout>(Resource.Id.SettingsButton);
            settingsOpen.Click += ButtonAction;
            searchField = FindViewById<EditText>(Resource.Id.SearchBarField);
            navBar = FindViewById<LinearLayout>(Resource.Id.NavBar);
            searchBar = FindViewById<Button>(Resource.Id.SearchBar);
            searchBar.Click += ToggleSearchMode;
            navBarSearch = FindViewById<LinearLayout>(Resource.Id.NavBarSearch);

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


        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



       
        private void CloseCreateView(object sender, EventArgs e)
        {
            if(isTaskCreateVisible == true)
            {
                mainHeader.Visibility = ViewStates.Visible;
                createTaskHeader.Visibility = ViewStates.Gone;
                isTaskCreateVisible = false;
            }



        }

        private void OpenCreateView(object sender, EventArgs e)
        {
            if(isTaskCreateVisible == false)
            {
                mainHeader.Visibility = ViewStates.Gone;
                createTaskHeader.Visibility = ViewStates.Visible;
                isTaskCreateVisible = true;



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
        private void ToggleSearchMode(object sender, EventArgs e)
        {
            if(navBar.Visibility == ViewStates.Visible)
            {
                navBar.Visibility = ViewStates.Gone;
                navBarSearch.Visibility = ViewStates.Visible;
            }
            else if (navBarSearch.Visibility == ViewStates.Visible)
            {
                navBarSearch.Visibility = ViewStates.Gone;
                navBar.Visibility = ViewStates.Visible;
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
                    settingsReturn = FindViewById<RelativeLayout>(Resource.Id.SettingsReturn);
                    //settingsReturn.Click += ButtonAction;
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
    }
}
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

            searchField = FindViewById<EditText>(Resource.Id.SearchBarField);
            navBar = FindViewById<LinearLayout>(Resource.Id.NavBar);
            searchBar = FindViewById<Button>(Resource.Id.SearchBar);
            searchBar.Click += ToggleSearchMode;
            navBarSearch = FindViewById<LinearLayout>(Resource.Id.NavBarSearch);
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
    }
}
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
                int viewHeight = header.Height;

                ValueAnimator animator = ValueAnimator.OfInt(viewHeight, viewHeight - 200);
                animator.SetDuration(500);
                animator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
                {
                    var value = (int)animator.AnimatedValue;
                    ViewGroup.LayoutParams layoutParams = header.LayoutParameters;
                    layoutParams.Height = value;
                    header.LayoutParameters = layoutParams;
                };
                mainHeader.Visibility = ViewStates.Visible;
                createTaskHeader.Visibility = ViewStates.Gone;
                //animator.Start();
                isTaskCreateVisible = false;
            }



        }

        private void OpenCreateView(object sender, EventArgs e)
        {
            if(isTaskCreateVisible == false)
            {
                int viewHeight = header.Height;

                ValueAnimator animator = ValueAnimator.OfInt(viewHeight, viewHeight + 200);
                animator.SetDuration(500);
                animator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
                {
                    var value = (int)animator.AnimatedValue;
                    ViewGroup.LayoutParams layoutParams = header.LayoutParameters;
                    layoutParams.Height = value;
                    header.LayoutParameters = layoutParams;
                };
                mainHeader.Visibility = ViewStates.Gone;
                createTaskHeader.Visibility = ViewStates.Visible;
                //animator.Start();
                isTaskCreateVisible = true;
            }

        }
    }
}
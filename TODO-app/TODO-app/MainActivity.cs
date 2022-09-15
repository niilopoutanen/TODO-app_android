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
        Button btnCreateTask;
        LinearLayout header;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnCreateTask = FindViewById<Button>(Resource.Id.CreateTask);
            btnCreateTask.Click += ToCreateTaskView;
            header = FindViewById<LinearLayout>(Resource.Id.Header);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



       
        private void ToCreateTaskView(object sender, EventArgs e)
        {
            int viewHeight = header.Height;
            ValueAnimator animator = ValueAnimator.OfInt(viewHeight, viewHeight+200);
            animator.SetDuration(500);
            animator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
            {
                var value = (int)animator.AnimatedValue;
                ViewGroup.LayoutParams layoutParams = header.LayoutParameters;
                layoutParams.Height = value;
                header.LayoutParameters = layoutParams;
            };
            animator.Start();
            btnCreateTask.Visibility = ViewStates.Gone;
        }
    }
}
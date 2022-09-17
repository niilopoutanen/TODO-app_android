using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TODO_app.Resources.layout
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]


    public class OnBoardingActitivty : Activity
    {
        Button next;
        Button skip;
        TextView onBoardHeader;
        ImageView guideView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.onboarder);
            next = FindViewById<Button>(Resource.Id.nextButton);
            next.Click += NextView;

            guideView = FindViewById<ImageView>(Resource.Id.guideScreen);
            onBoardHeader = FindViewById<TextView>(Resource.Id.onBoardHeader);
            // Create your application here

        }

        private void NextView(object sender, EventArgs e)
        {
            if (onBoardHeader.Text == GetString(Resource.String.guide1))
            {
                guideView.SetImageResource(Resource.Drawable.guide2);
                onBoardHeader.Text = GetString(Resource.String.guide2);
            }
            else if (onBoardHeader.Text == GetString(Resource.String.guide2))
            {
                Intent backToMain = new Intent(this, typeof(MainActivity));
                StartActivity(backToMain);
            }


        }
    }
}
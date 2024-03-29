﻿using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;
using System;

namespace TODO_app.Resources.layout
{
    [Activity(Theme = "@style/AppTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            var mode = Resources.Configuration.UiMode;
            if (Android.OS.Build.VERSION.SdkInt == Android.OS.BuildVersionCodes.Q)
            {
                if (mode == Android.Content.Res.UiMode.NightYes)
                {
                    SetTheme(Resource.Style.AppTheme_Splash);

                }
                else if (mode == Android.Content.Res.UiMode.NightNo)
                {
                    SetTheme(Resource.Style.AppTheme_Splash_Light);

                }

            }
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        [Obsolete]
        public override void OnBackPressed() { }


    }
}
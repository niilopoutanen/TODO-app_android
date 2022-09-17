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
using AndroidX.AppCompat.App;
using System.Runtime.Remoting.Contexts;
using Xamarin.Essentials;
using static Android.Renderscripts.Sampler;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SettingsActivity : AppCompatActivity
    {
        TextView version;
        RelativeLayout sendFeedbackButton;

        TextView Niilobtn;
        TextView Oskaribtn;
        TextView Tomibtn;

        Button blueTheme;
        Button greenTheme;
        Button orangeTheme;
        Button violetTheme;
        Button redTheme;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_settings);
            RelativeLayout settingsReturn = FindViewById<RelativeLayout>(Resource.Id.SettingsReturn);
            settingsReturn.Click += BackToMenu;

            version = FindViewById<TextView>(Resource.Id.VersionText);
            version.Text = AppInfo.Version.ToString();

            sendFeedbackButton = FindViewById<RelativeLayout>(Resource.Id.SendFeedbackBtn);
            sendFeedbackButton.Click += SendFeedback;

            Niilobtn = FindViewById<TextView>(Resource.Id.CreditsNP);
            Oskaribtn = FindViewById<TextView>(Resource.Id.CreditsOM);
            Tomibtn = FindViewById<TextView>(Resource.Id.CreditsTV);

            Niilobtn.Click += CreditsLinks;
            Oskaribtn.Click += CreditsLinks;
            Tomibtn.Click += CreditsLinks;

            blueTheme = FindViewById<Button>(Resource.Id.MainBlueToggle);
            greenTheme = FindViewById<Button>(Resource.Id.MainGreenToggle);
            violetTheme = FindViewById<Button>(Resource.Id.MainVioletToggle);
            orangeTheme = FindViewById<Button>(Resource.Id.MainOrangeToggle);
            redTheme = FindViewById<Button>(Resource.Id.MainRedToggle);

            blueTheme.Click += ChangeTheme;
            greenTheme.Click += ChangeTheme;
            violetTheme.Click += ChangeTheme;
            orangeTheme.Click += ChangeTheme;
            redTheme.Click += ChangeTheme;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
        private void BackToMenu(object sender, EventArgs e)
        {
            Intent mainMenuStarter = new Intent(this, typeof(MainActivity));
            StartActivity(mainMenuStarter);
        }
        private void SendFeedback(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("https://github.com/niilopoutanen/TODO-app_android/issues/new");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
        private void CreditsLinks(object sender, EventArgs e)
        {
            var button = (TextView)sender;
            switch (button.Id)
            {
                case Resource.Id.CreditsNP:
                    var uriN = Android.Net.Uri.Parse("https://github.com/niilopoutanen");
                    var intentN = new Intent(Intent.ActionView, uriN);
                    StartActivity(intentN);
                    break;

                case Resource.Id.CreditsOM:
                    var uriO = Android.Net.Uri.Parse("https://github.com/osaama05");
                    var intentO = new Intent(Intent.ActionView, uriO);
                    StartActivity(intentO);
                    break;

                case Resource.Id.CreditsTV:
                    var uriT = Android.Net.Uri.Parse("https://github.com/Tolpanjuuri");
                    var intentT = new Intent(Intent.ActionView, uriT);
                    StartActivity(intentT);
                    break;
            }

        }


        private void ChangeTheme(object sender, EventArgs e)
        {
            Button colorButton = (Button)sender;
            ISharedPreferences colorTheme = GetSharedPreferences("ColorTheme", 0);
            switch (colorButton.Id)
            {
                case Resource.Id.MainBlueToggle:
                    
                    colorTheme.Edit().PutString("colorTheme", "blue").Commit();
                    break;

                case Resource.Id.MainGreenToggle:
                    colorTheme.Edit().PutString("colorTheme", "green").Commit();
                    break;

                case Resource.Id.MainOrangeToggle:
                    colorTheme.Edit().PutString("colorTheme", "orange").Commit();
                    break;

                case Resource.Id.MainVioletToggle:
                    colorTheme.Edit().PutString("colorTheme", "violet").Commit();
                    break;

                case Resource.Id.MainRedToggle:
                    colorTheme.Edit().PutString("colorTheme", "red").Commit();
                    break;
            }
        }
    }
}
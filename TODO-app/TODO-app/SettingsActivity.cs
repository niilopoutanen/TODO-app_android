using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using AndroidX.AppCompat.App;
using Xamarin.Essentials;
using Firebase.Analytics;
using System.Collections.Generic;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SettingsActivity : AppCompatActivity
    {
        private string savedTheme = "";
        private bool vibration;
        TextView version;
        RelativeLayout sendFeedbackButton;

        TextView Niilobtn;
        TextView Oskaribtn;
        TextView Tomibtn;

        RelativeLayout blueTheme;
        RelativeLayout greenTheme;
        RelativeLayout orangeTheme;
        RelativeLayout violetTheme;
        RelativeLayout redTheme;


        ImageView blueActive;
        ImageView greenActive;
        ImageView orangeActive;
        ImageView violetActive;
        ImageView redActive;

        Switch vibrationToggle;
        Button deleteAllDone;

        private List<TaskItem> taskList = new List<TaskItem>();
        FileClass files = new FileClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            FirebaseAnalytics.GetInstance(this);

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            LoadSettings();
            SetTheme(GetStyle());
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

            blueTheme = FindViewById<RelativeLayout>(Resource.Id.MainBlueToggle);
            greenTheme = FindViewById<RelativeLayout>(Resource.Id.MainGreenToggle);
            violetTheme = FindViewById<RelativeLayout>(Resource.Id.MainVioletToggle);
            orangeTheme = FindViewById<RelativeLayout>(Resource.Id.MainOrangeToggle);
            redTheme = FindViewById<RelativeLayout>(Resource.Id.MainRedToggle);

            blueTheme.Click += ChangeTheme;
            greenTheme.Click += ChangeTheme;
            violetTheme.Click += ChangeTheme;
            orangeTheme.Click += ChangeTheme;
            redTheme.Click += ChangeTheme;

            blueActive = FindViewById<ImageView>(Resource.Id.MainBlueActive);
            greenActive = FindViewById<ImageView>(Resource.Id.MainGreenActive);
            orangeActive = FindViewById<ImageView>(Resource.Id.MainOrangeActive);
            violetActive = FindViewById<ImageView>(Resource.Id.MainVioletActive);
            redActive = FindViewById<ImageView>(Resource.Id.MainRedActive);

            deleteAllDone = FindViewById<Button>(Resource.Id.deleteAllDoneButton);
            vibrationToggle = FindViewById<Switch>(Resource.Id.vibrationSwitch);

            deleteAllDone.Click += DeleteAllDone_Click;
            vibrationToggle.CheckedChange += ToggleVibration;
            if (vibration == true)
            {
                vibrationToggle.Checked = true;
            }
            else if (vibration == false)
            {
                vibrationToggle.Checked = false;
            }

            switch (savedTheme)
            {
                case "blue":
                    blueActive.Visibility = ViewStates.Visible;
                    break;

                case "orange":
                    orangeActive.Visibility = ViewStates.Visible;
                    break;

                case "green":
                    greenActive.Visibility = ViewStates.Visible;
                    break;

                case "violet":
                    violetActive.Visibility = ViewStates.Visible;
                    break;

                case "red":
                    redActive.Visibility = ViewStates.Visible;
                    break;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        private int GetStyle()
        {
            if (savedTheme == "blue")
            {
                return Resource.Color.mainBlue;
            }
            else if (savedTheme == "orange")
            {
                return Resource.Color.mainOrange;
            }
            else if (savedTheme == "green")
            {
                return Resource.Color.mainGreen;
            }
            else if (savedTheme == "violet")
            {
                return Resource.Color.mainViolet;
            }
            else if (savedTheme == "red")
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
            ISharedPreferences vibrationPref = GetSharedPreferences("Vibration", 0);
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
            else
            {
                SetTheme(Resource.Style.MainBlue);
            }
            savedTheme = color;

            vibration = vibrationPref.GetBoolean("vibrationEnabled", default);

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
            blueActive.Visibility = ViewStates.Gone;
            orangeActive.Visibility = ViewStates.Gone;
            greenActive.Visibility = ViewStates.Gone;
            violetActive.Visibility = ViewStates.Gone;
            redActive.Visibility = ViewStates.Gone;
            RelativeLayout colorButton = (RelativeLayout)sender;
            ISharedPreferences colorTheme = GetSharedPreferences("ColorTheme", 0);
            switch (colorButton.Id)
            {
                case Resource.Id.MainBlueToggle:
                    blueActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "blue").Commit();
                    break;

                case Resource.Id.MainGreenToggle:
                    greenActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "green").Commit();
                    break;

                case Resource.Id.MainOrangeToggle:
                    orangeActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "orange").Commit();
                    break;

                case Resource.Id.MainVioletToggle:
                    violetActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "violet").Commit();
                    break;

                case Resource.Id.MainRedToggle:
                    redActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "red").Commit();
                    break;
            }
        }

        private void DeleteAllDone_Click(object sender, EventArgs e)
        {
            files.ReadFile();
            foreach (TaskItem task in taskList)
            {
                if (task.IsDone == true)
                {
                    taskList.Remove(task);
                }
            }
            files.WriteFile(taskList);
            if (vibration == true)
            {
                VibrationEffect invalidHaptic = VibrationEffect.CreateOneShot(100, VibrationEffect.DefaultAmplitude);
                Vibrator hapticSystem = (Vibrator)GetSystemService(VibratorService);
                hapticSystem.Cancel();
                hapticSystem.Vibrate(invalidHaptic);
            }
        }

        private void ToggleVibration(object sender, CompoundButton.CheckedChangeEventArgs e)
        {

            ISharedPreferences vibrationPref = GetSharedPreferences("Vibration", 0);
            
            if (e.IsChecked == true)
            {
                vibrationPref.Edit().PutBoolean("vibrationEnabled", true).Commit();
                vibration = true;
                VibrationEffect invalidHaptic = VibrationEffect.CreateOneShot(100, VibrationEffect.DefaultAmplitude);
                Vibrator hapticSystem = (Vibrator)GetSystemService(VibratorService);
                hapticSystem.Cancel();
                hapticSystem.Vibrate(invalidHaptic);
            }

            else if (e.IsChecked == false)
            {
                vibrationPref.Edit().PutBoolean("vibrationEnabled", false).Commit();
                vibration = false;


            }
        }
    }
}
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
using Android.Util;
using TODO_app.Resources.layout;
using Android.Content.Res;
using Android.Icu.Lang;
using AndroidX.Core.Content;
using Android;
using System.Security.Cryptography;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SettingsActivity : AppCompatActivity
    {
        private string savedTheme = "";
        private bool vibration;
        private bool notifications;
        TextView version;
        RelativeLayout sendFeedbackButton;
        RelativeLayout replayTutorial;
        RelativeLayout colorSelector;
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
        Switch notificationsToggle;
        RelativeLayout notificationTimeBtn;
        Spinner themeSelector;
        RelativeLayout deleteAllDone;
        RelativeLayout deleteAll;
        Vibrator vibrator;
        VibratorManager vibratorManager;
        ISharedPreferences themePref;
        ActivityMethods methods = new ActivityMethods();
        private List<TaskItem> taskList = new List<TaskItem>();
        FileClass files = new FileClass();
        int themechecked = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            FirebaseAnalytics.GetInstance(this);

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            LoadSettings();
            SetTheme(GetStyle());
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_settings);
            RelativeLayout settingsReturn = FindViewById<RelativeLayout>(Resource.Id.SettingsReturn);
            settingsReturn.Click += BackToMenu;
            colorSelector = FindViewById<RelativeLayout>(Resource.Id.colorSelector);
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.SV2)
            {
                colorSelector.Visibility = ViewStates.Gone;
            }
            version = FindViewById<TextView>(Resource.Id.VersionText);
            version.Text = AppInfo.Version.ToString();

            sendFeedbackButton = FindViewById<RelativeLayout>(Resource.Id.SendFeedbackBtn);
            sendFeedbackButton.Click += SendFeedback;

            replayTutorial = FindViewById<RelativeLayout>(Resource.Id.ReplayTutorialbtn);
            replayTutorial.Click += ReplayTutorial;

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

            deleteAllDone = FindViewById<RelativeLayout>(Resource.Id.deleteAllDoneButton);
            deleteAll = FindViewById<RelativeLayout>(Resource.Id.deleteAllButton);
            vibrationToggle = FindViewById<Switch>(Resource.Id.vibrationSwitch);
            notificationsToggle = FindViewById<Switch>(Resource.Id.notificationsSwitch);
            notificationTimeBtn = FindViewById<RelativeLayout>(Resource.Id.changeNotificationTimeBtn);
            themeSelector = FindViewById<Spinner>(Resource.Id.themeSelector);
            string[] themeOptions = { GetString(Resource.String.darkTheme), GetString(Resource.String.lightTheme), GetString(Resource.String.systemTheme) };
            ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, themeOptions);
            adapter.SetDropDownViewResource(Resource.Layout.spinner_item);
            themeSelector.Adapter = adapter;
            themeSelector.ItemSelected += ThemeSelected;
            themeSelector.SetSelection(SetThemeSpinnerDefault());
            themeSelector.TextAlignment = TextAlignment.Center;
            if(Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Q)
            {
                FindViewById<RelativeLayout>(Resource.Id.themePanel).Visibility = ViewStates.Gone;
            }
            vibrator = (Vibrator)GetSystemService(VibratorService);
            vibratorManager = (VibratorManager)GetSystemService(VibratorManagerService);

            deleteAll.Click += DeleteAll_Click;
            deleteAllDone.Click += DeleteAllDone_Click;
            vibrationToggle.CheckedChange += ToggleVibration;
            notificationsToggle.CheckedChange += ToggleNotifications;
            notificationTimeBtn.Click += ChangeNotificationTime;
            switch (vibration)
            {
                case true:
                    vibrationToggle.Checked = true;
                    break;
                case false:
                    vibrationToggle.Checked = false;
                    break;
            }

            switch (notifications)
            {
                case true:
                    notificationsToggle.Checked = true;
                    break;
                case false:
                    notificationsToggle.Checked = false;
                    break;
            }
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) == Android.Content.PM.Permission.Denied)
            {
                notificationsToggle.Checked = false;
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
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) == Android.Content.PM.Permission.Denied)
            {
                notificationsToggle.Checked = false;
            }
        }
        private int SetThemeSpinnerDefault()
        {
            int selectedPosition = 0;
            string selected = themePref.GetString("themeSelected", default);
            switch (selected)
            {
                case "dark":
                    selectedPosition = 0;
                    break;
                case "light":
                    selectedPosition = 1;
                    break;
                case "system":
                    selectedPosition = 2;
                    break;
            }
            return selectedPosition;
        }
        private void ThemeSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if(++themechecked > 1)
            {
                Spinner selector = (Spinner)sender;
                int selectedID = (int)selector.GetItemIdAtPosition(e.Position);
                int selectedPosition = e.Position;
                //if (selectedID == Resource.String.lightTheme)
                //{
                //    themePref.Edit().PutString("themeSelected", "light").Commit();
                //}
                //else if (selectedID == Resource.String.darkTheme)
                //{
                //    themePref.Edit().PutString("themeSelected", "dark").Commit();
                //}
                //else if (selectedID == Resource.String.systemTheme)
                //{
                //    themePref.Edit().PutString("themeSelected", "system").Commit();
                //}
                if (selectedPosition == 0)
                {
                    themePref.Edit().PutString("themeSelected", "dark").Commit();
                    RestartActivity();
                }
                else if (selectedPosition == 1)
                {
                    themePref.Edit().PutString("themeSelected", "light").Commit();
                    RestartActivity();
                }
                else if (selectedPosition == 2)
                {
                    themePref.Edit().PutString("themeSelected", "system").Commit();
                    RestartActivity();
                }
            }

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
            themePref = GetSharedPreferences("Theme", 0);
            string themeSelected = themePref.GetString("themeSelected", default);
            ISharedPreferences vibrationPref = GetSharedPreferences("Vibration", 0);
            ISharedPreferences notificationPref = GetSharedPreferences("Notifications", 0);
            ISharedPreferences colorTheme = GetSharedPreferences("ColorTheme", 0);
            string color = colorTheme.GetString("colorTheme", default);
            switch (color)
            {
                case "blue":
                    SetTheme(Resource.Style.MainBlue);
                    break;
                case "green":
                    SetTheme(Resource.Style.MainGreen);
                    break;
                case "orange":
                    SetTheme(Resource.Style.MainOrange);
                    break;
                case "violet":
                    SetTheme(Resource.Style.MainViolet);
                    break;
                case "red":
                    SetTheme(Resource.Style.MainRed);
                    break;
                case null:
                    SetTheme(Resource.Style.MainBlue);
                    break;
            }
            switch (themeSelected)
            {
                case "dark":
                    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightYes;
                    break;
                case "light":
                    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
                    break;
                case "system":
                    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightFollowSystem;
                    break;
            }
            savedTheme = color;
            vibration = vibrationPref.GetBoolean("vibrationEnabled", default);
            notifications = notificationPref.GetBoolean("notificationsEnabled", default);

        }

        private void BackToMenu(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 100);
            }
            Intent mainMenuStarter = new Intent(this, typeof(MainActivity));
            mainMenuStarter.SetFlags(ActivityFlags.ClearTop);
            StartActivity(mainMenuStarter);
            Finish();
        }
        
        private void SendFeedback(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 50);
            }
            var uri = Android.Net.Uri.Parse("https://github.com/niilopoutanen/TODO-app_android/issues/new");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        private void ReplayTutorial(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 50);
            }
            Intent onBoraderStarter = new Intent(this, typeof(OnBoardingActivity));
            StartActivity(onBoraderStarter);
            Finish();
        }

        private void CreditsLinks(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 50);

            }
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
            if (vibration == true)
            {
                methods.Vibrate(vibrator,vibratorManager, 60);
            }
            switch (colorButton.Id)
            {
                case Resource.Id.MainBlueToggle:
                    blueActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "blue").Commit();
                    RestartActivity();
                    break;

                case Resource.Id.MainGreenToggle:
                    greenActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "green").Commit();
                    RestartActivity();
                    break;

                case Resource.Id.MainOrangeToggle:
                    orangeActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "orange").Commit();
                    RestartActivity();
                    break;

                case Resource.Id.MainVioletToggle:
                    violetActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "violet").Commit();
                    RestartActivity();
                    break;

                case Resource.Id.MainRedToggle:
                    redActive.Visibility = ViewStates.Visible;
                    colorTheme.Edit().PutString("colorTheme", "red").Commit();
                    RestartActivity();
                    break;
            }
        }

        private void DeleteAllDone_Click(object sender, EventArgs e)
        {
            int amountRemoved = 0;
            taskList = files.ReadFile();
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i].IsDone == true)
                {
                    taskList.Remove(taskList[i]);
                    amountRemoved++;
                }
            }
            files.WriteFile(taskList);

            if (amountRemoved > 0)
            {
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, 60);
                }
                OpenPopup(GetString(Resource.String.tasksDeleted), GetString(Resource.String.deleted) + " " + amountRemoved + " " + GetString(Resource.String.task), "OK");
            }
            else if (amountRemoved <= 0)
            {
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, 200);
                }
                OpenPopup(GetString(Resource.String.nothingToDelete), GetString(Resource.String.noCompletedTasks), "OK");

            }
        }

        private void DeleteAll_Click(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 60);
            }
            FileClass fClass = new FileClass();

            List<TaskItem> oldTasks = fClass.ReadFile();
            List<TaskItem> emptyList = new List<TaskItem>();
            fClass.WriteFile(emptyList);
            if(fClass.ReadFile().Count == 0)
            {
                OpenPopup(GetString(Resource.String.tasksDeleted), GetString(Resource.String.deleted) + " " + oldTasks.Count + " " + GetString(Resource.String.task), "OK");
            }
            else
            {
                OpenPopup("Error", "", "OK");
            }

        }
        private void ToggleVibration(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ISharedPreferences vibrationPref = GetSharedPreferences("Vibration", 0);
            
            if (e.IsChecked == true)
            {
                vibrationPref.Edit().PutBoolean("vibrationEnabled", true).Commit();
                vibration = true;
                methods.Vibrate(vibrator, vibratorManager, 100);

            }

            else if (e.IsChecked == false)
            {
                vibrationPref.Edit().PutBoolean("vibrationEnabled", false).Commit();
                vibration = false;
            }
        }
        private void ToggleNotifications(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 60);
            }
            ISharedPreferences notificationPref = GetSharedPreferences("Notifications", 0);

            if (e.IsChecked == true)
            {
                notificationPref.Edit().PutBoolean("notificationsEnabled", true).Commit();
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) == Android.Content.PM.Permission.Denied)
                {
                    string[] perms = { Manifest.Permission.PostNotifications };
                    RequestPermissions(perms, 0);
                }

            }

            else if (e.IsChecked == false)
            {
                notificationPref.Edit().PutBoolean("notificationsEnabled", false).Commit();
            }
        }
        private void ChangeNotificationTime(object sender, EventArgs e)
        {
            if (vibration == true)
            {
                methods.Vibrate(vibrator, vibratorManager, 60);
            }
            int timePicked;
            ISharedPreferences notifTime = GetSharedPreferences("NotificationTime", 0);
            timePicked = notifTime.GetInt("notifTime", default);
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Android.Content.Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.time_popup, null);
            view.BackgroundTintList = GetColorStateList(Resource.Color.colorPrimaryDark);
            alert.SetView(view);
            alert.Show();
            alert.Window.SetLayout(DpToPx(300), DpToPx(220));
            alert.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
            Button confirm = view.FindViewById<Button>(Resource.Id.timePopupConfirm);
            NumberPicker picker = view.FindViewById<NumberPicker>(Resource.Id.timePicker);
            TextView timeHeader = view.FindViewById<TextView>(Resource.Id.timeHeader);
            picker.MinValue = 1;
            picker.MaxValue = 24;
            picker.Value = timePicked;
            picker.ValueChanged += (s, e) =>
            {
                timeHeader.Text = GetString(Resource.String.timeHeader) + " " + picker.Value + ":00";
                timePicked = picker.Value;
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, 30);
                }
            };
            confirm.Click += (s, e) =>
            {
                notifTime.Edit().PutInt("notifTime", timePicked).Commit();
                alert.Dismiss();
                if (vibration == true)
                {
                    methods.Vibrate(vibrator, vibratorManager, 60);
                }
            };
        }
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
        private void RestartActivity()
        {
            Intent restart = new Intent(this, typeof(SettingsActivity));
            StartActivity(restart);
            OverridePendingTransition(0, 0);
            Finish();
        }
        private int DpToPx(int dpValue)
        {
            int pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dpValue, Resources.DisplayMetrics);
            return pixel;
        }

        [Obsolete]
        public override void OnBackPressed() 
        {
            Intent mainMenuStarter = new Intent(this, typeof(MainActivity));
            StartActivity(mainMenuStarter);
        }
    }
}
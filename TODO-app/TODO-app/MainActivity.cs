using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace TODO_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnCreateTask;
        DatePicker datePicker;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnCreateTask = FindViewById<Button>(Resource.Id.CreateTask);
            datePicker = FindViewById<DatePicker>(Resource.Id.datePicker1);
            btnCreateTask.Click += btnCreateTask_Click;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            Dialog popup = new Dialog(this); 
            popup.SetContentView(Resource.Layout.create_task_popup);
            popup.Window.SetSoftInputMode(SoftInput.AdjustResize);
            popup.SetTitle("@string/TaskName");
            popup.Show();
        }
    }
}
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

namespace TODO_app
{
    [Activity(Label = "WhatsNewActivity", Theme = "@style/DarkEdges")]
    public class WhatsNewActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_whatsnew);
        }
    }
}
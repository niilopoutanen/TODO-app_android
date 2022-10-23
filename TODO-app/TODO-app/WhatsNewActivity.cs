using Android.App;
using Android.OS;

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
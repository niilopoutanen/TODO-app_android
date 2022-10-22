using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Threading.Tasks;

namespace TODO_app.Resources.layout
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, NoHistory =true)]
    public class OnBoardingActivity : Activity
    {
        Button next;
        Button skip;
        TextView onBoardHeader;
        ImageView guideView;
        RelativeLayout imageLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.OnBoardTheme);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            SetContentView(Resource.Layout.activity_onboarder);

            next = FindViewById<Button>(Resource.Id.nextButton);
            next.Click += NextView;

            skip = FindViewById<Button>(Resource.Id.skipButton);
            skip.Click += ToMain;
            guideView = FindViewById<ImageView>(Resource.Id.guideScreen);
            onBoardHeader = FindViewById<TextView>(Resource.Id.onBoardHeader);
            imageLayout = FindViewById<RelativeLayout>(Resource.Id.imageLayout);

        }

        private async void NextView(object sender, EventArgs e)
        {
            if (onBoardHeader.Text == GetString(Resource.String.guide1))
            {
                guideView.Animate().Alpha(0).SetDuration(300).Start();
                onBoardHeader.Animate().Alpha(0).SetDuration(300).Start();
                await Task.Delay(400);
                guideView.SetImageResource(Resource.Drawable.guide2);
                onBoardHeader.Text = GetString(Resource.String.guide2);
                guideView.Animate().Alpha(1).SetDuration(200).Start();
                onBoardHeader.Animate().Alpha(1).SetDuration(300).Start();
                await Task.Delay(300);

            }
            else if (onBoardHeader.Text == GetString(Resource.String.guide2))
            {
                guideView.Animate().Alpha(0).SetDuration(300).Start();
                onBoardHeader.Animate().Alpha(0).SetDuration(300).Start();
                await Task.Delay(400);


                guideView.SetImageResource(Resource.Drawable.guide3);
                onBoardHeader.Text = GetString(Resource.String.guide3);
                guideView.Animate().Alpha(1).SetDuration(200).Start();
                onBoardHeader.Animate().Alpha(1).SetDuration(300).Start();
                await Task.Delay(300);

            }
            else if (onBoardHeader.Text == GetString(Resource.String.guide3))
            {
                guideView.Animate().Alpha(0).SetDuration(300).Start();
                onBoardHeader.Animate().Alpha(0).SetDuration(300).Start();
                await Task.Delay(400);


                guideView.SetImageResource(Resource.Drawable.guide4);
                onBoardHeader.Text = GetString(Resource.String.guide4);
                guideView.Animate().Alpha(1).SetDuration(200).Start();
                onBoardHeader.Animate().Alpha(1).SetDuration(300).Start();
                await Task.Delay(300);


            }
            else if (onBoardHeader.Text == GetString(Resource.String.guide4))
            {
                guideView.Animate().Alpha(0).SetDuration(300).Start();
                onBoardHeader.Animate().Alpha(0).SetDuration(300).Start();
                await Task.Delay(400);


                guideView.SetImageResource(Resource.Drawable.guide5);
                onBoardHeader.Text = GetString(Resource.String.guide5);
                guideView.Animate().Alpha(1).SetDuration(200).Start();
                onBoardHeader.Animate().Alpha(1).SetDuration(300).Start();
                await Task.Delay(300);


            }
            else if (onBoardHeader.Text == GetString(Resource.String.guide5))
            {
                guideView.Animate().Alpha(0).SetDuration(300).Start();
                onBoardHeader.Animate().Alpha(0).SetDuration(300).Start();
                await Task.Delay(400);
                guideView.SetImageResource(Resource.Drawable.guide6);
                onBoardHeader.Text = GetString(Resource.String.guide6);

                skip.Visibility = ViewStates.Gone;

                RelativeLayout.LayoutParams startBtnParams = new RelativeLayout.LayoutParams(DpToPx(140), DpToPx(40));
                startBtnParams.RemoveRule(LayoutRules.AlignParentRight);
                startBtnParams.AddRule(LayoutRules.CenterInParent);
                next.LayoutParameters = startBtnParams;
                next.Text = GetString(Resource.String.Start);
                guideView.Animate().Alpha(1).SetDuration(200).Start();
                onBoardHeader.Animate().Alpha(1).SetDuration(300).Start();
                await Task.Delay(300);


            }
            else if (onBoardHeader.Text == GetString(Resource.String.guide6))
            {
                ISharedPreferences hasWatchedGuide = GetSharedPreferences("hasWatchedGuide", 0);
                hasWatchedGuide.Edit().PutBoolean("hasWatchedGuide", true).Commit();
                Intent backToMain = new Intent(this, typeof(MainActivity));
                backToMain.SetFlags(ActivityFlags.ClearTop);
                StartActivity(backToMain);
                Finish();
            }


        }
        private void ToMain(object sender, EventArgs e)
        {
            ISharedPreferences hasWatchedGuide = GetSharedPreferences("hasWatchedGuide", 0);
            hasWatchedGuide.Edit().PutBoolean("hasWatchedGuide", true).Commit();
            Intent backToMain = new Intent(this, typeof(MainActivity));
            backToMain.SetFlags(ActivityFlags.ClearTop);
            StartActivity(backToMain);
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
        }

    }
}
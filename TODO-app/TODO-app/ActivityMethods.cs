using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace TODO_app
{
    internal class ActivityMethods : AppCompatActivity
    {
        public int DpToPx(int dpValue)
        {

            //viittaaminen toiseen luokkaan ei onnistu, Java.NullException
            int pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dpValue, Resources.DisplayMetrics);
            return pixel;
        }

        public void Vibrate(Vibrator vibratorService, VibratorManager vibratorManager, int length)
        {
            Vibrator vibrator;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                VibratorManager manager = vibratorManager;
                vibrator = manager.DefaultVibrator;
            }
            else
            {
                vibrator = vibratorService;
            }
            vibrator.Vibrate(VibrationEffect.CreateOneShot(length, VibrationEffect.DefaultAmplitude));

        }
    }
}
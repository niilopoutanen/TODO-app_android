using Android.OS;
using AndroidX.AppCompat.App;
using System;

namespace TODO_app
{
    internal class ActivityMethods : AppCompatActivity
    {
        public readonly int intensityHard = 85;
        public readonly int intensityMedium = 60;
        public readonly int intensitySmall = 45;

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
            vibrator.Cancel();
            vibrator.Vibrate(VibrationEffect.CreateOneShot(length, VibrationEffect.DefaultAmplitude));

        }

        public static string TooLongStringParser(string inputToParse, int maxChar)
        {
            if (inputToParse.Length > maxChar)
            {
                string newstring = inputToParse.Substring(0, maxChar - 3);
                newstring = newstring + "...";
                return newstring;
            }
            else
            {
                return inputToParse;
            }
        }

        public string ProgressVisualizer(int amountDone, int amountNeeded, bool prefersPercentage)
        {
            if (prefersPercentage == true)
            {
                double percentage = (((double)amountDone / (double)amountNeeded) * 100);
                return Math.Round(percentage, 0).ToString() + "%";
            }
            else
            {
                return amountDone + "/" + amountNeeded;
            }
        }
        public int ProgressBarCalculator(int baseWidth, int amountDone, int amountNeeded)
        {
            double percentage = Math.Round((double)amountDone / (double)amountNeeded * 100, 0);

            double progressWidth = (double)baseWidth / 100 * percentage;

            return (int)Math.Round(progressWidth, 0);
        }
    }
}
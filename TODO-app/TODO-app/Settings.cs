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
    internal class Settings
    {
        private bool darkMode = true;

        public bool DarkMode
        {
            get { return darkMode; }
            set { darkMode = value; }
        }

        public Settings()
        {

        }
    }
}
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
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
    internal class ActivityMethods
    {
        public static void InvalidInput(EditText visual)
        {
            MainActivity inherit = new MainActivity();

            Drawable active = inherit.GetDrawable(Resource.Drawable.rounded50px);
            Drawable invalid = inherit.GetDrawable(Resource.Drawable.rounded50pxInvalid);

            visual.Background = invalid;
        }
    }
}
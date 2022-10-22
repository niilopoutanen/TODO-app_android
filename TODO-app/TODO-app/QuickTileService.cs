using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Service.QuickSettings;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TODO_app
{
    [IntentFilter(new string[] { "android.service.quicksettings.action.QS_TILE" })]
    [Service(Name = "com.tiimi1.todo_app.QuickTileService", Label = "@string/add_task_header", Icon = "@drawable/iconFull", Permission = Android.Manifest.Permission.BindQuickSettingsTile, Exported = true)]
    internal class QuickTileService : TileService
    {
        public override void OnClick()
        {
            base.OnClick();

            Intent createTask = new Intent(this, typeof(CreateTaskActivity));
            createTask.SetFlags(ActivityFlags.NewTask);
            createTask.PutExtra("mode", "tileCreate");
            StartActivityAndCollapse(createTask);
        }
    }
}
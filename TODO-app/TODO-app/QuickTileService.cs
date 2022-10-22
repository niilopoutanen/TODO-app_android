using Android.App;
using Android.Content;
using Android.Service.QuickSettings;

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
        public override void OnStartListening()
        {
            base.OnStartListening();
            Tile tile = QsTile;
            tile.State = TileState.Inactive;
            tile.UpdateTile();
        }
    }
}
using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Eri.Views;

namespace Eri.Droid
{
    //[Activity(Label = "Eri", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //スプラッシュ画面の設定
    [Activity(Label = "Eri", Icon = "@drawable/icon", Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            //スプラッシュ画面の設定
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);
            
            base.OnCreate(savedInstanceState);
            
            base.SetTheme(Resource.Style.MainTheme);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();

            // プログレスバー（ダウンロード、アップロード時）
            // 呼び出し方
            //  表示 =>   MessagingCenter.Send(App, "dialog_progress",true);
            //  非表示 => MessagingCenter.Send(App, "dialog_progress", false);
            AlertDialog alertDialog = null;
            MessagingCenter.Subscribe<LinkPage, bool>(this, "dialog_progress", (page, isVisible) =>
            {
                if (alertDialog != null)
                {
                    alertDialog.Dismiss();
                    alertDialog = null;
                }

                if (isVisible)
                {
                    var progress = new Android.Widget.ProgressBar(this);
                    
                    progress.SetPadding(0, 30, 0, 30);

                    alertDialog = new AlertDialog.Builder(this)
                    .SetTitle("データ連携中です。")
                    .SetView(progress)
                    .SetCancelable(false)
                    .Show();
                }
            }); 
            

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
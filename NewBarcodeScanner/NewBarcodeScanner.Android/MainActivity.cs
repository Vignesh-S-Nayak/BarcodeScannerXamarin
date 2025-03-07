using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using ZXing.Net.Mobile.Forms.Android;

namespace NewBarcodeScanner.Droid
{
    [Activity(Label = "NewBarcodeScanner", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ZXingScannerViewRenderer.Init();
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Xamarin.Essentials.Platform.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();

            // Force any active ZXing scanner views to stop
            var renderer = global::Xamarin.Forms.Platform.Android.Platform.GetRenderer(
                Xamarin.Forms.Application.Current.MainPage);

            if (renderer != null)
            {
                // This helps ensure the camera is released when the app is paused
                renderer.Dispose();
            }
        }
    }
}

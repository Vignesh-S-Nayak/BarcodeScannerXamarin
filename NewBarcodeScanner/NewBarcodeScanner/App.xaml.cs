using Xamarin.Forms;
using NewBarcodeScanner.Services;
using NewBarcodeScanner.Views;

namespace NewBarcodeScanner
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (AuthService.IsLoggedIn)
                MainPage = new AppShell();
            else
                MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

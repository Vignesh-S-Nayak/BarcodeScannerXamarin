using System;
using Xamarin.Forms;
using NewBarcodeScanner.Services;

namespace NewBarcodeScanner.Views
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            // Open the camera page as a modal
            await Navigation.PushModalAsync(new CameraPage());
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            AuthService.Logout();
            Device.BeginInvokeOnMainThread(() => {
                Application.Current.MainPage = new LoginPage();
            });
        }
    }
}

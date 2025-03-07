using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing;
using NewBarcodeScanner.Services;

namespace NewBarcodeScanner.Views
{
    public partial class CameraPage : ContentPage
    {
        private bool isProcessing = false;
        private bool isScanLineAnimating = false;

        // Properties for layout calculations
        public double ScanWindowSize { get; private set; } = 250;
        public double TopOverlayHeight { get; private set; }
        public double BottomOverlayHeight { get; private set; }
        public double SideOverlayWidth { get; private set; }

        public CameraPage()
        {
            InitializeComponent();
            overlayGrid.BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Request camera permission
            var status = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Camera>();
            if (status != Xamarin.Essentials.PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Required", "Camera permission is needed to scan barcodes", "OK");
                await Navigation.PopModalAsync();
                return;
            }

            // Start the scanner
            scannerView.IsScanning = true;
            scannerView.IsAnalyzing = true;

            // Start animating the scan line
            StartScanLineAnimation();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            // Calculate overlay dimensions
            if (width > 0 && height > 0)
            {
                // Make the scan window a bit smaller on smaller screens
                ScanWindowSize = Math.Min(width, height) * 0.7;

                // Calculate overlay dimensions
                SideOverlayWidth = (width - ScanWindowSize) / 2;
                double centerAreaTop = (height - ScanWindowSize) / 2;
                TopOverlayHeight = centerAreaTop;
                BottomOverlayHeight = centerAreaTop;

                // Force property change notification
                OnPropertyChanged(nameof(ScanWindowSize));
                OnPropertyChanged(nameof(TopOverlayHeight));
                OnPropertyChanged(nameof(BottomOverlayHeight));
                OnPropertyChanged(nameof(SideOverlayWidth));
            }
        }

        private async void StartScanLineAnimation()
        {
            if (isScanLineAnimating)
                return;

            isScanLineAnimating = true;

            while (isScanLineAnimating)
            {
                // Move from top to bottom
                await scanLine.TranslateTo(0, -ScanWindowSize / 2 + 2, 0);
                await scanLine.TranslateTo(0, ScanWindowSize / 2 - 2, 1500, Easing.Linear);

                // Move from bottom to top
                await scanLine.TranslateTo(0, ScanWindowSize / 2 - 2, 0);
                await scanLine.TranslateTo(0, -ScanWindowSize / 2 + 2, 1500, Easing.Linear);
            }
        }

        private void OnScanResult(Result result)
        {
            if (result == null || isProcessing) return;

            isProcessing = true;

            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Stop scanning immediately
                    scannerView.IsScanning = false;
                    scannerView.IsAnalyzing = false;
                    isScanLineAnimating = false;

                    // Process the result
                    await ProcessScannedBarcodeAsync(result.Text);
                });
            }
            finally
            {
                isProcessing = false;
            }
        }

        private async Task ProcessScannedBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return;

            // Add or update the scanned item
            await ItemService.AddOrUpdateItem(barcode);

            // Close this page and return to the scan tab, but navigate to the items tab
            await Shell.Current.Navigation.PopModalAsync();

            // Navigate to items tab
            await Shell.Current.GoToAsync("//items");
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Stop scanning
            scannerView.IsScanning = false;
            scannerView.IsAnalyzing = false;
            isScanLineAnimating = false;

            // Go back to the scan page
            await Shell.Current.Navigation.PopModalAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Ensure scanner is stopped when page disappears
            scannerView.IsScanning = false;
            scannerView.IsAnalyzing = false;
            isScanLineAnimating = false;
        }
    }
}

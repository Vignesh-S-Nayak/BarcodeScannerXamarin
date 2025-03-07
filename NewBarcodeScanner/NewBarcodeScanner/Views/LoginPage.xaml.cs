﻿using Xamarin.Forms;
using NewBarcodeScanner.ViewModels;

namespace NewBarcodeScanner.Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as LoginViewModel;
            _viewModel.OnLoginSuccessful += OnLoginSuccessful;
        }

        private void OnLoginSuccessful()
        {
            Device.BeginInvokeOnMainThread(() => {
                Application.Current.MainPage = new AppShell();
            });
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.OnLoginSuccessful -= OnLoginSuccessful;
        }
    }
}

// <copyright file="MainPage.xaml.cs" company="AndrewForster">
// Copyright (c) AndrewForster. All rights reserved.
// </copyright>

namespace VideoCaptureUWP
{
    using VideoCapature.Common;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.DataContext = new MainPageViewModel("VideoCapture (UWP)");
        }

        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
    }
}

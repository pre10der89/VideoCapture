// <copyright file="MainPage.xaml.cs" company="AndrewForster">
// Copyright (c) AndrewForster. All rights reserved.
// </copyright>

namespace VideoCaptureWinRT
{
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public MainPage()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.InitializeComponent();

          // this.DataContext = new MainPageViewModel("VideoCapture (Windows Phone)");
        }
    }
}

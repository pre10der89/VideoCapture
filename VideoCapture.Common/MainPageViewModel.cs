// <copyright file="MainPageViewModel.cs" company="AndrewForster">
// Copyright (c) AndrewForster. All rights reserved.
// </copyright>

namespace VideoCapature.Common
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Mvvm;

    /// <summary>
    /// View Model for the main page
    /// </summary>
    public class MainPageViewModel : BindableBase
    {
        ////////////////////////////////////////
        // Constructors
        ////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// Empty Constructor for main page view model
        /// </summary>
        public MainPageViewModel()
            : this("Title")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// Constructor takes title
        /// </summary>
        /// <param name="title">Title for the page</param>
        public MainPageViewModel(string title)
        {
            this.title = title;
        }

        ////////////////////////////////////////
        // Properties
        ////////////////////////////////////////

        private string title = string.Empty;

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        public bool IsCaptureDevicesSelected
        {
            get
            {
                return this.CheckCaptureDevicesSelected();
            }
        }

        private bool isCapturing;

        public bool IsCapturing
        {
            get { return this.isCapturing; }
            set { this.SetProperty(ref this.isCapturing, value); }
        }

        private readonly ObservableCollection<string> cameraDeviceList = new ObservableCollection<string>();

        public ReadOnlyObservableCollection<string> CameraDevices
        {
            get { return new ReadOnlyObservableCollection<string>(this.cameraDeviceList); }
        }

        private string fileSaveLocation = string.Empty;

        public string FileSaveLocation
        {
            get { return this.fileSaveLocation; }
            set { this.SetProperty(ref this.fileSaveLocation, value); }
        }

        private string currentCamera = string.Empty;

        public string CurrentCamera
        {
            get { return this.currentCamera; }
            set { this.SetProperty(ref this.currentCamera, value); }
        }

        private readonly ObservableCollection<string> microphoneDeviceList = new ObservableCollection<string>();

        public ReadOnlyObservableCollection<string> MicrophoneDevices
        {
            get { return new ReadOnlyObservableCollection<string>(this.microphoneDeviceList); }
        }

        private string currentMicrophone = string.Empty;

        public string CurrentMicrophone
        {
            get { return this.currentMicrophone; }
            set { this.SetProperty(ref this.currentMicrophone, value); }
        }

        ////////////////////////////////////////
        // Commands
        ////////////////////////////////////////

        private ICommand loadCommand;

        public ICommand LoadCommand
        {
            get
            {
                return this.loadCommand ?? (this.loadCommand = new DelegateCommand(this.OnExecuteLoadCommand));
            }
        }

        private void OnExecuteLoadCommand()
        {
            this.cameraDeviceList.Add("Integrated Camera");
            this.cameraDeviceList.Add("Tanberg Camera");

            this.CurrentCamera = "Integrated Camera";

            this.microphoneDeviceList.Add("Integrated Microphone");
            this.microphoneDeviceList.Add("Plantronics");

            this.CurrentMicrophone = "Plantronics";

            this.IsCapturing = true;
        }

        private ICommand cameraSelectedCommand;

        public ICommand CameraSelectedCommand
        {
            get
            {
                return this.cameraSelectedCommand ?? (this.cameraSelectedCommand = new DelegateCommand<object>(this.OnExecuteCameraSelectedCommand));
            }
        }

        private void OnExecuteCameraSelectedCommand(object param)
        {
            if (param is string)
            {
                this.CurrentCamera = param as string;
            }

            this.OnPropertyChanged(PropertySupport.ExtractPropertyName(() => this.IsCaptureDevicesSelected));
        }

        private ICommand microphoneSelectedCommand;

        public ICommand MicrophoneSelectedCommand
        {
            get
            {
                return this.microphoneSelectedCommand ?? (this.microphoneSelectedCommand = new DelegateCommand<object>(this.OnExecuteMicrophoneSelectedCommand));
            }
        }

        private void OnExecuteMicrophoneSelectedCommand(object param)
        {
            if (param is string)
            {
                this.CurrentMicrophone = param as string;
            }

            this.OnPropertyChanged(PropertySupport.ExtractPropertyName(() => this.IsCaptureDevicesSelected));
        }

        private ICommand toggleCaptureStateCommand;

        public ICommand ToggleCaptureStateCommand
        {
            get
            {
                return this.toggleCaptureStateCommand ?? (this.toggleCaptureStateCommand = new DelegateCommand(this.OnExecuteToggleCaptureStateCommand));
            }
        }

        private void OnExecuteToggleCaptureStateCommand()
        {
            this.IsCapturing = !this.IsCapturing;

            if (this.IsCapturing)
            {
                this.FileSaveLocation = string.Empty;
            }
        }

        ////////////////////////////////////////
        // Private Methods
        ////////////////////////////////////////
        private bool CheckCaptureDevicesSelected()
        {
            if (string.IsNullOrEmpty(this.CurrentCamera) || string.IsNullOrEmpty(this.CurrentMicrophone))
            {
                return false;
            }

            return true;
        }
    }
}

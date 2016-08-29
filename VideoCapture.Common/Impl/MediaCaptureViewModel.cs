// <copyright file="MainPageViewModel.cs" company="AndrewForster">
// Copyright (c) AndrewForster. All rights reserved.
// </copyright>

namespace VideoCapature.Common.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Core.Interfaces;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Mvvm;
    using VideoCapture.Common.Core.DTOs;
    using VideoCapture.Common.ViewModel.Interfaces;

    /// <summary>
    /// View Model for the main page
    /// </summary>
    public class MediaCaptureViewModel : BindableBase, IMediaCaptureViewModel
    {
        #region Private Fields

        private readonly IMediaCaptureController mediaCaptureController;

        #endregion

        #region Constructor(s)

        public MediaCaptureViewModel(IMediaCaptureController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            this.mediaCaptureController = controller;
        }

        #endregion

        #region IDisposable Members

        private bool disposedValue = false; // To detect redundant calls

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                this.disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MediaCaptureManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }
        #endregion

        #region IMediaCaptureViewModel Members

        #region Properties

        private bool isInitialized;

        public bool IsInitialized
        {
            get { return this.isInitialized; }
            set { this.SetProperty(ref this.isInitialized, value); }
        }

        private bool isPreviewing;

        public bool IsPreviewing
        {
            get { return this.isPreviewing; }
            set { this.SetProperty(ref this.isPreviewing, value); }
        }

        private bool isRecording;

        public bool IsRecording
        {
            get { return this.isRecording; }
            set { this.SetProperty(ref this.isRecording, value); }
        }

        private bool isRecordingSaved;

        public bool IsRecordingSaved
        {
            get { return this.isRecordingSaved; }
            set { this.SetProperty(ref this.isRecordingSaved, value); }
        }

        private string recordingSavedLocation;

        public string RecordingSavedLocation
        {
            get { return this.recordingSavedLocation; }
            set { this.SetProperty(ref this.recordingSavedLocation, value); }
        }

        #endregion

        #region Commands

        #region Initialize Command

        private ICommand initializeCommand;

        public ICommand InitializeCommand
        {
            get
            {
                return this.initializeCommand ?? (this.initializeCommand = new DelegateCommand(this.OnExecuteInitializeCommand));
            }
        }

        private void OnExecuteInitializeCommand()
        {
            Task.Run(async () =>
            {
                var settings = new MediaCaptureSettingsDTO();

                await this.ExecuteInitialize(settings);
            });
        }

        #endregion

        #region Deinitialize Command

        private ICommand deinitializeCommand;

        public ICommand DeinitializeCommand
        {
            get
            {
                return this.deinitializeCommand ?? (this.deinitializeCommand = new DelegateCommand(this.OnExecuteDeinitializeCommand));
            }
        }

        private void OnExecuteDeinitializeCommand()
        {
            Task.Run(async () =>
            {
                await this.ExecuteDeinitialize();
            });
        }

        #endregion

        #region Toggle Initialization Command

        private ICommand toggleInitializationCommand;

        public ICommand ToggleInitializationCommand
        {
            get
            {
                return this.toggleInitializationCommand ?? (this.toggleInitializationCommand = new DelegateCommand(this.OnExecuteToggleInitializationCommand));
            }
        }

        private void OnExecuteToggleInitializationCommand()
        {
            Task.Run(async () =>
            {
                await this.ExecuteToggleInitialization();
            });
        }

        #endregion

        #region Toggle Previewing Command

        private ICommand togglePreviewingCommand;

        public ICommand TogglePreviewingCommand
        {
            get
            {
                return this.togglePreviewingCommand ?? (this.togglePreviewingCommand = new DelegateCommand(this.OnExecuteTogglePreviewingCommand));
            }
        }

        private void OnExecuteTogglePreviewingCommand()
        {
            Task.Run(async () =>
            {
                await this.ExecuteTogglePreviewing();
            });
        }

        #endregion

        #region Toggle Recording Command

        private ICommand toggleRecordingCommand;

        public ICommand ToggleRecordingCommand
        {
            get
            {
                return this.toggleRecordingCommand ?? (this.toggleRecordingCommand = new DelegateCommand(this.OnExecuteToggleRecordingCommand));
            }
        }

        private void OnExecuteToggleRecordingCommand()
        {
            Task.Run(async () =>
            {
                await this.ExecuteToggleRecording();
            });
        }

        #endregion

        #endregion

        #endregion

        #region Protected Virtual Methods

        protected virtual async Task ExecuteInitialize(MediaCaptureSettingsDTO settings)
        {
            try
            {
                // TODO: Grab the settings from the user to allow for a more dynamic experience... For now we'll go with the defaults...
                await this.mediaCaptureController.InitializeAsync(settings);
            }
            catch (Exception)
            {
                // TODO: Raise an interaction event on the view model to tell the view to display an error...
            }
        }

        protected virtual async Task ExecuteDeinitialize()
        {
            try
            {
                await this.mediaCaptureController.DeinitializeAsync();
            }
            catch (Exception)
            {
                // TODO: Raise an interaction event on the view model to tell the view to display an error...
            }
        }

        protected virtual async Task ExecuteToggleInitialization()
        {
            try
            {
                if (this.mediaCaptureController.State.IsInitialized)
                {
                    await this.mediaCaptureController.DeinitializeAsync();
                }
                else
                {
                    MediaCaptureSettingsDTO settings = new MediaCaptureSettingsDTO();

                    await this.mediaCaptureController.InitializeAsync(settings);
                }
            }
            catch (Exception)
            {
                // TODO: Raise an interaction event on the view model to tell the view to display an error...
            }
        }

        protected virtual async Task ExecuteTogglePreviewing()
        {
            try
            {
                if (this.mediaCaptureController.State.IsPreviewStarted)
                {
                    await this.mediaCaptureController.StopPreviewingAsync();
                }
                else
                {
                    await this.mediaCaptureController.StartPreviewingAsync();
                }
            }
            catch (Exception)
            {
                // TODO: Raise an interaction event on the view model to tell the view to display an error...
            }
        }

        protected virtual async Task ExecuteToggleRecording()
        {
            try
            {
                if (this.mediaCaptureController.State.IsRecordStarted)
                {
                    await this.mediaCaptureController.StopRecordingAsync();
                }
                else
                {
                    await this.mediaCaptureController.StartRecordingAsync();
                }
            }
            catch (Exception)
            {
                // TODO: Raise an interaction event on the view model to tell the view to display an error...
            }
        }

        #endregion
    }
}

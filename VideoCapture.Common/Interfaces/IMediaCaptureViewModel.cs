namespace VideoCapture.Common.ViewModel.Interfaces
{
    using System;
    using System.Windows.Input;

    public interface IMediaCaptureViewModel : IDisposable
    {
        #region Properties

        bool IsInitialized { get; }

        bool IsPreviewing { get; }

        bool IsRecording { get; }

        bool IsRecordingSaved { get;  }

        string RecordingSavedLocation { get; }

        #endregion

        #region Commands

        ICommand InitializeCommand { get; }

        ICommand DeinitializeCommand { get; }

        ICommand ToggleInitializationCommand { get; }

        ICommand TogglePreviewingCommand { get; }

        ICommand ToggleRecordingCommand { get; }

        #endregion

        #region Methods

        #endregion

    }
}

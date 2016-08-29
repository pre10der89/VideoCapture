namespace VideoCapature.Common.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using VideoCapture.Common.Core.DTOs;

    public interface IMediaCaptureController : IDisposable
    {
        #region Properties

        MediaCaptureStateDTO State { get;  }

        #endregion

        #region Events

        event EventHandler<VideoCapture.Common.Core.Events.MediaCaptureUpdatedEventArgs> UpdateEvent;

        event EventHandler<VideoCapture.Common.Core.Events.MediaCaptureFailedEventArgs> MediaCaptureFailedEvent;

        event EventHandler<VideoCapture.Common.Core.Events.RecordLimitationExceededEventArgs> RecordLimitationExceededEvent;

        #endregion

        #region Methods

        Task InitializeAsync(MediaCaptureSettingsDTO settings);

        Task DeinitializeAsync();

        Task StartPreviewingAsync();

        Task StopPreviewingAsync();

        Task StartRecordingAsync();

        Task StopRecordingAsync();

        Task<VideoBufferDTO> SaveRecordingAsync();

        #endregion
    }
}

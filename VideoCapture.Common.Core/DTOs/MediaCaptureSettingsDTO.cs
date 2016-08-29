namespace VideoCapture.Common.Core.DTOs
{
    public class MediaCaptureSettingsDTO
    {
        #region Properties

        public string AudioDeviceId
        {
            get;
            set;
        }

        public string VideoDeviceId
        {
            get;
            set;
        }

        public CaptureModeEnum CaptureMode
        {
            get;
            set;
        }

        public bool IsAutoSaved
        {
            get;
            set;
        }

        public string SaveLocation
        {
            get;
            set;
        }

        #endregion

        #region Constructor(s)

        public MediaCaptureSettingsDTO()
            : this(string.Empty, string.Empty, CaptureModeEnum.AudioAndVideo)
        {
        }

        public MediaCaptureSettingsDTO(string audioDeviceId, string videoDeviceId, CaptureModeEnum mode)
        {
            this.AudioDeviceId = audioDeviceId;
            this.VideoDeviceId = videoDeviceId;
            this.CaptureMode = mode;

            this.IsAutoSaved = true;
        }

        #endregion
    }
}

namespace VideoCapture.Common.Core.DTOs
{
    // TODO: where is ICloneable - Depracated?
    public class MediaCaptureStateDTO
    {
        // TODO: Is the states of Previewing and Recording mutally exclusive? I have both state here because I
        // am assuming that I could record without previewing and we know we can preview without recording...
        #region Properties

        public bool IsInitializing { get; set; }

        public bool IsInitialized { get; set; }

        public bool IsInitializeFailed { get; set; }

        public bool IsDeinitializing { get; set; }

        public bool IsRecordStarting { get; set; }

        public bool IsRecordStarted { get; set; }

        public bool IsRecordStartedFailed { get; set; }

        public bool IsRecordStopping { get; set; }

        public bool IsPreviewStarting { get; set; }

        public bool IsPreviewStarted { get; set; }

        public bool IsPreviewStartedFailed { get; set; }

        public bool IsPreviewStopping { get; set; }

        #endregion

        #region Constructor(s)

        public MediaCaptureStateDTO()
        {
        }

        #endregion

        #region Public Methods

        public MediaCaptureStateDTO DeepCopy()
        {
            // TODO: Probably overkill - could have probalby used MemberwiseCopy since all are booleans,
            // but that might change...
            var copy = new MediaCaptureStateDTO
            {
                IsInitializing = this.IsInitializing,
                IsInitialized = this.IsInitialized,
                IsInitializeFailed = this.IsInitializeFailed,
                IsDeinitializing = this.IsDeinitializing,
                IsRecordStarting = this.IsRecordStarting,
                IsRecordStarted = this.IsRecordStarted,
                IsRecordStartedFailed = this.IsRecordStartedFailed,
                IsRecordStopping = this.IsRecordStopping,
                IsPreviewStarting = this.IsPreviewStarting,
                IsPreviewStarted = this.IsPreviewStarted,
                IsPreviewStartedFailed = this.IsPreviewStartedFailed,
                IsPreviewStopping = this.IsPreviewStopping
            };

            return copy;
        }

        public void Reset()
        {
            this.IsInitializing = false;
            this.IsInitialized = false;
            this.IsInitializeFailed = false;
            this.IsDeinitializing = false;
            this.IsRecordStarting = false;
            this.IsRecordStarted = false;
            this.IsRecordStartedFailed = false;
            this.IsRecordStopping = false;
            this.IsPreviewStarting = false;
            this.IsPreviewStarted = false;
            this.IsPreviewStartedFailed = false;
            this.IsPreviewStopping = false;
        }

        #endregion
    }
}

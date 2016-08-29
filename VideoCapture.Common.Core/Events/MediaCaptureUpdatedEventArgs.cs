namespace VideoCapture.Common.Core.Events
{
    using System;
    using DTOs;

    public class MediaCaptureUpdatedEventArgs : EventArgs
    {
        #region Properties

        public MediaCaptureStateDTO State
        {
            get;
            private set;
        }

        #endregion

        #region Constructor(s)

        public MediaCaptureUpdatedEventArgs(MediaCaptureStateDTO state)
        {
            this.State = state;
        }

        #endregion
    }
}

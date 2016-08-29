namespace VideoCapture.Common.Core.Events
{
    using System;

    public class MediaCaptureFailedEventArgs : EventArgs
    {
        #region Properties

        public uint Code
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        #endregion

        #region Constructor(s)

        public MediaCaptureFailedEventArgs(uint code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        #endregion
    }
}

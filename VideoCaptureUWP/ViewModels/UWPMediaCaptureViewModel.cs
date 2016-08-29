namespace VideoCaptureUWP.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using VideoCapature.Common.ViewModels;
    using VideoCapture.Common.Core.DTOs;
    using VideoCapture.Common.ViewModel.Interfaces;
    using Windows.Media.Capture;
    using Windows.UI.Core;

    public sealed class UWPMediaCaptureViewModel : MediaCaptureViewModel, IMediaCaptureViewModel
    {
        #region Private Fields

        private readonly CoreDispatcher dispatcher;

        #endregion

        #region Constructor(s)

        public UWPMediaCaptureViewModel(MediaCaptureController controller, MediaCapture captureSource, CoreDispatcher dispatcher)
            : base(controller)
        {
            if (captureSource == null)
            {
                throw new ArgumentNullException("captureSource");
            }

            this.captureSource = captureSource;

            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }

            this.dispatcher = dispatcher;
        }

        #endregion

        #region IDisposable Overrides

        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            try
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
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region IUWPMediaCaptureViewModel Members

        #region Properties

        private readonly MediaCapture captureSource;

        public MediaCapture Source
        {
            get { return this.captureSource; }
        }

        #endregion

        #endregion

        #region Override Methods

        protected override async Task ExecuteInitialize(MediaCaptureSettingsDTO settings)
        {
            try
            {
                await base.ExecuteInitialize(settings);
            }
            catch (Exception)
            {
                // TODO: Raise an interaction event on the view model to tell the view to display an error...
            }
        }

        protected override async Task ExecuteDeinitialize()
        {
            try
            {
                await base.ExecuteDeinitialize();
            }
            catch (Exception)
            {
                // TODO: Raise an interaction event on the view model to tell the view to display an error...
            }
        }

        #endregion
    }
}

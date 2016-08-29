namespace VideoCaptureUWP
{
    using System;
    using System.Threading.Tasks;
    using VideoCapature.Common.Core.Interfaces;
    using VideoCapture.Common.Core.DTOs;
    using Windows.Media.Capture;
    using Windows.UI.Core;

    public class MediaCaptureController : IMediaCaptureController
    {
        #region Private Fields

        private readonly object dataProtector = new object();

        private readonly CoreDispatcher dispatcher;

        private readonly MediaCaptureStateDTO mediaCaptureState = new MediaCaptureStateDTO();

        private readonly MediaCapture mediaCapture;

        #endregion

        #region Public Property Accessors

        public MediaCapture Source
        {
            get { return this.mediaCapture; }
        }

        #endregion

        #region Constructor(s)

        public MediaCaptureController(MediaCapture source, CoreDispatcher dispatcher)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.mediaCapture = source;

            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }

            this.dispatcher = dispatcher;
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

        #region IMediaCaptureController Members

        #region Properties

        public MediaCaptureStateDTO State
        {
            get { return this.mediaCaptureState.DeepCopy(); }
        }

        #endregion

        #region Events

        private event EventHandler<VideoCapture.Common.Core.Events.MediaCaptureUpdatedEventArgs> updateEvent;

        public event EventHandler<VideoCapture.Common.Core.Events.MediaCaptureUpdatedEventArgs> UpdateEvent
        {
            add
            {
                lock (this.dataProtector)
                {
                    this.updateEvent += value;
                }
            }

            remove
            {
                lock (this.dataProtector)
                {
                    this.updateEvent -= value;
                }
            }
        }

        private event EventHandler<VideoCapture.Common.Core.Events.MediaCaptureFailedEventArgs> mediaCaptureFailedEvent;

        public event EventHandler<VideoCapture.Common.Core.Events.MediaCaptureFailedEventArgs> MediaCaptureFailedEvent
        {
            add
            {
                lock (this.dataProtector)
                {
                    this.mediaCaptureFailedEvent += value;
                }
            }

            remove
            {
                lock (this.dataProtector)
                {
                    this.mediaCaptureFailedEvent -= value;
                }
            }
        }

        private event EventHandler<VideoCapture.Common.Core.Events.RecordLimitationExceededEventArgs> recordLimitationExceededEvent;

        public event EventHandler<VideoCapture.Common.Core.Events.RecordLimitationExceededEventArgs> RecordLimitationExceededEvent
        {
            add
            {
                lock (this.dataProtector)
                {
                    this.recordLimitationExceededEvent += value;
                }
            }

            remove
            {
                lock (this.dataProtector)
                {
                    this.recordLimitationExceededEvent -= value;
                }
            }
        }

        #endregion

        #region Methods

        public async Task InitializeAsync(MediaCaptureSettingsDTO settings)
        {
            lock (this.dataProtector)
            {
                if (this.mediaCaptureState.IsInitialized)
                {
                    // TODO: Is this too heavy handed to throw an exception here?
                    throw new InvalidOperationException("Media Capture Manager is already initialized...");
                }

                if (this.mediaCaptureState.IsDeinitializing)
                {
                    throw new InvalidOperationException("Media Capture Manager is currently deinitializing...");
                }
            }

            // Reset the state of the object before attempting to initialize it...
            // await this.DeinitializeAsync();

            //////////////////////////////////////////////////////////////////////////////////////////////////
            // Update the state
            //////////////////////////////////////////////////////////////////////////////////////////////////
            lock (this.dataProtector)
            {
                this.mediaCaptureState.IsInitializing = true;
                this.mediaCaptureState.IsInitializeFailed = false;
            }

            // Fire Update Event
            await this.FireMediaCaptureUpdatedEvent();

            // TODO: How do we hanlde an excpetion in the MediaCapture call... Will an exception thrown in that method result in the catch block being executed
            // here? Assuming for the moment that wrapping the MediaCapture methods in a try/catch will work... Need to do more research...
            try
            {
                var mediaCaptureInitSettings = new MediaCaptureInitializationSettings();

                if (settings != null)
                {
                    mediaCaptureInitSettings.AudioDeviceId = settings.AudioDeviceId;
                    mediaCaptureInitSettings.VideoDeviceId = settings.VideoDeviceId;

                    switch (settings.CaptureMode)
                    {
                        case CaptureModeEnum.AudioAndVideo:
                            mediaCaptureInitSettings.StreamingCaptureMode = StreamingCaptureMode.AudioAndVideo;
                            break;
                        case CaptureModeEnum.Audio:
                            mediaCaptureInitSettings.StreamingCaptureMode = StreamingCaptureMode.Audio;
                            break;
                        case CaptureModeEnum.Video:
                            mediaCaptureInitSettings.StreamingCaptureMode = StreamingCaptureMode.Video;
                            break;
                    }
                }

                // Initialize Media Capture Instance
                await this.dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await this.mediaCapture.InitializeAsync(mediaCaptureInitSettings);
                });

                // TODO: Should we subscribe to the media capture events before initializing or after?  When Does the media capture failed event get called
                // Subscribe to Media Capture Events
                this.SubscribeToMediaCaptureEvents();

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Update the state
                //////////////////////////////////////////////////////////////////////////////////////////////////
                lock (this.dataProtector)
                {
                    this.mediaCaptureState.IsInitialized = true;
                    this.mediaCaptureState.IsInitializing = false;
                    this.mediaCaptureState.IsDeinitializing = false;
                }
            }
            catch (Exception)
            {
                // Deinitialize to reset the instance state...
                await this.DeinitializeAsync();

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Update the state
                //////////////////////////////////////////////////////////////////////////////////////////////////
                lock (this.dataProtector)
                {
                    this.mediaCaptureState.IsInitializeFailed = true;
                }
            }
            finally
            {
                // TODO: Will this execute after all the await return in the try block?  Need to test...

                // Fire Update Event
                await this.FireMediaCaptureUpdatedEvent();
            }
        }

        public async Task DeinitializeAsync()
        {
            lock (this.dataProtector)
            {
                if (this.mediaCapture == null)
                {
                    // The media capture instance is null - Reset state flags and return immediately...
                    this.ResetState();

                    return;
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Update the state
                //////////////////////////////////////////////////////////////////////////////////////////////////
                this.mediaCaptureState.IsDeinitializing = true;
            }

            // Fire Update Event
            await this.FireMediaCaptureUpdatedEvent();

            // TODO: How can we protect against another method starting recording or previewing or performing another
            // operation on the media catpure instance while we are deinitializing it... Right now using the IsDeinitializing
            // flag might be the best bet since it doesn't make sense to lock around await statements (nor is it even allowed)...
            if (this.mediaCapture != null)
            {
                this.UnsubscribeFromMediaCaptureEvents();

                await this.dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await this.mediaCapture.StopRecordAsync();
                    await this.mediaCapture.StopPreviewAsync();
                });
            }

            lock (this.dataProtector)
            {
                this.ResetState();
            }

            // Fire Update Event
            await this.FireMediaCaptureUpdatedEvent();
        }

        public async Task StartPreviewingAsync()
        {
            lock (this.dataProtector)
            {
                if (!this.IsCaptureManagerAvailable())
                {
                    throw new InvalidOperationException("The capture manager is not available");
                }

                if (this.mediaCaptureState.IsPreviewStarted || this.mediaCaptureState.IsPreviewStopping || this.mediaCaptureState.IsPreviewStarting)
                {
                    // If the preview is already started, it is in the process of starting, or we are in the process of stopping the previewing then
                    // we return immediately...
                    return;
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Update the state
                //////////////////////////////////////////////////////////////////////////////////////////////////
                this.mediaCaptureState.IsPreviewStarting = true;
                this.mediaCaptureState.IsPreviewStartedFailed = false;
            }

            // Fire Update Event
            await this.FireMediaCaptureUpdatedEvent();

            // TODO: How do we hanlde an excpetion in the MediaCapture call... Will an exception thrown in that method result in the catch block being executed
            // here? Assuming for the moment that wrapping the MediaCapture methods in a try/catch will work... Need to do more research...
            try
            {
                await this.dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await this.mediaCapture.StartPreviewAsync();
                });

                lock (this.dataProtector)
                {
                    //////////////////////////////////////////////////////////////////////////////////////////////////
                    // Update the state
                    //////////////////////////////////////////////////////////////////////////////////////////////////
                    this.mediaCaptureState.IsPreviewStarted = true;
                    this.mediaCaptureState.IsPreviewStarting = false;
                    this.mediaCaptureState.IsPreviewStopping = false;
                }
            }
            catch (Exception)
            {
                lock (this.dataProtector)
                {
                    //////////////////////////////////////////////////////////////////////////////////////////////////
                    // Update the state
                    //////////////////////////////////////////////////////////////////////////////////////////////////
                    this.mediaCaptureState.IsPreviewStartedFailed = true;

                    this.mediaCaptureState.IsPreviewStarted = false;
                    this.mediaCaptureState.IsPreviewStarting = false;
                    this.mediaCaptureState.IsPreviewStopping = false;
                }
            }
            finally
            {
                // Fire Update Event
                await this.FireMediaCaptureUpdatedEvent();
            }
        }

        public async Task StopPreviewingAsync()
        {
            lock (this.dataProtector)
            {
                if (!this.IsCaptureManagerAvailable())
                {
                    // The capture manager is not available because it is either not initialized, is deinitialzing, or
                    // is otherwise unavailable... We'll consider this a no op...
                    return;
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Update the state
                //////////////////////////////////////////////////////////////////////////////////////////////////
                this.mediaCaptureState.IsPreviewStopping = true;
            }

            // Fire Update Event
            await this.FireMediaCaptureUpdatedEvent();

            // TODO: How do we hanlde an excpetion in the MediaCapture call... Will an exception thrown in that method result in the catch block being executed
            // here? Assuming for the moment that wrapping the MediaCapture methods in a try/catch will work... Need to do more research...
            try
            {
                await this.dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await this.mediaCapture.StopPreviewAsync();
                });
            }
            catch (Exception)
            {
            }
            finally
            {
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Update the state
                //////////////////////////////////////////////////////////////////////////////////////////////////

                lock (this.dataProtector)
                {
                    this.mediaCaptureState.IsPreviewStarting = false;
                    this.mediaCaptureState.IsPreviewStarted = false;
                    this.mediaCaptureState.IsPreviewStopping = false;
                    this.mediaCaptureState.IsPreviewStartedFailed = false;
                }

                // Fire Update Event
                await this.FireMediaCaptureUpdatedEvent();
            }
        }

        public Task StartRecordingAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopRecordingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<VideoBufferDTO> SaveRecordingAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region Event Handlers

        private void SubscribeToMediaCaptureEvents()
        {
            lock (this.dataProtector)
            {
                this.UnsubscribeFromMediaCaptureEvents();

                if (this.mediaCapture != null)
                {
                    this.mediaCapture.Failed += this.OnMediaCaptureFailed;
                    this.mediaCapture.RecordLimitationExceeded += this.OnMediaCaptureRecordLimitationExceeded;
                }
            }
        }

        private void UnsubscribeFromMediaCaptureEvents()
        {
            lock (this.dataProtector)
            {
                if (this.mediaCapture != null)
                {
                    this.mediaCapture.Failed -= this.OnMediaCaptureFailed;
                    this.mediaCapture.RecordLimitationExceeded -= this.OnMediaCaptureRecordLimitationExceeded;
                }
            }
        }

        private async void OnMediaCaptureFailed(MediaCapture sender, Windows.Media.Capture.MediaCaptureFailedEventArgs erroreventargs)
        {
            await this.FireMediaCaptureFailedEvent(erroreventargs);
        }

        private async void OnMediaCaptureRecordLimitationExceeded(MediaCapture sender)
        {
            // await _mediaCapture.StopRecordAsync();
            await this.FireRecordLimitationExceededEvent();
        }

        #endregion

        #region Private Methods

        private bool IsCaptureManagerAvailable() // TODO: Better name?
        {
            lock (this.dataProtector)
            {
                if (this.mediaCaptureState.IsDeinitializing || this.mediaCaptureState.IsInitializing || !this.mediaCaptureState.IsInitialized)
                {
                    return false;
                }

                if (this.mediaCapture == null)
                {
                    return false;
                }
            }

            return true;
        }

        private void ResetState()
        {
            lock (this.dataProtector)
            {
                this.mediaCaptureState.Reset();
            }
        }

        private async Task FireMediaCaptureUpdatedEvent()
        {
            Delegate[] listeners;

            MediaCaptureStateDTO currentState;

            lock (this.dataProtector)
            {
                if (this.updateEvent == null)
                {
                    return;
                }

                listeners = this.updateEvent.GetInvocationList();

                if (listeners.Length == 0)
                {
                    return;
                }

                currentState = this.mediaCaptureState.DeepCopy();
            }

            await Task.Run(() =>
            {
                foreach (var listener in listeners)
                {
                    var args = new VideoCapture.Common.Core.Events.MediaCaptureUpdatedEventArgs(currentState);

                    var currentEventHandler = listener as EventHandler<VideoCapture.Common.Core.Events.MediaCaptureUpdatedEventArgs>;

                    if (currentEventHandler != null)
                    {
                        try
                        {
                            currentEventHandler(this, args);
                        }
                        catch (Exception)
                        {
                            // Ignore
                        }
                    }
                }
            });
        }

        private async Task FireMediaCaptureFailedEvent(Windows.Media.Capture.MediaCaptureFailedEventArgs erroreventargs)
        {
            if (erroreventargs == null)
            {
                throw new ArgumentNullException("erroreventargs");
            }

            Delegate[] listeners = null;

            lock (this.dataProtector)
            {
                if (this.mediaCaptureFailedEvent == null)
                {
                    return;
                }

                listeners = this.mediaCaptureFailedEvent.GetInvocationList();

                if (listeners.Length == 0)
                {
                    return;
                }
            }

            await Task.Run(() =>
            {
                foreach (var listener in listeners)
                {
                    var args = new VideoCapture.Common.Core.Events.MediaCaptureFailedEventArgs(erroreventargs.Code, erroreventargs.Message);

                    var currentEventHandler = listener as EventHandler<VideoCapture.Common.Core.Events.MediaCaptureFailedEventArgs>;

                    if (currentEventHandler != null)
                    {
                        try
                        {
                            currentEventHandler(this, args);
                        }
                        catch (Exception)
                        {
                            // Ignore
                        }
                    }
                }
            });
        }

        private async Task FireRecordLimitationExceededEvent()
        {
            Delegate[] listeners = null;

            lock (this.dataProtector)
            {
                if (this.recordLimitationExceededEvent == null)
                {
                    return;
                }

                listeners = this.recordLimitationExceededEvent.GetInvocationList();

                if (listeners.Length == 0)
                {
                    return;
                }
            }

            await Task.Run(() =>
            {
                foreach (var listener in listeners)
                {
                    var args = new VideoCapture.Common.Core.Events.RecordLimitationExceededEventArgs();

                    var currentEventHandler = listener as EventHandler<VideoCapture.Common.Core.Events.RecordLimitationExceededEventArgs>;

                    if (currentEventHandler != null)
                    {
                        try
                        {
                            currentEventHandler(this, args);
                        }
                        catch (Exception)
                        {
                            // Ignore
                        }
                    }
                }
            });
        }

        #endregion
    }
}

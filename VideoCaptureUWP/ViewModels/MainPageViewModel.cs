namespace VideoCaptureUWP.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Mvvm;
    using Windows.Media.Capture;
    using Windows.UI.Core;

    public class MainPageViewModel : BindableBase
    {
        #region Private Fields

        private readonly CoreDispatcher dispatcher;

        #endregion

        #region Constructor(s)

        public MainPageViewModel(string title, CoreDispatcher dispatcher)
        {
            this.title = title;

            this.dispatcher = dispatcher;
        }

        #endregion

        #region IMainPageViewModel Members

        #region Properties

        private string title = string.Empty;

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        private UWPMediaCaptureViewModel mediaCaptureViewModel;

        public UWPMediaCaptureViewModel MediaCaptureVM
        {
            get { return this.mediaCaptureViewModel; }
            private set { this.SetProperty(ref this.mediaCaptureViewModel, value); }
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
                await this.ExecuteInitialize();
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
        }

        #endregion

        #endregion

        #endregion

        #region Private Methods

        private async Task ExecuteInitialize()
        {
            await this.dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var captureSource = new MediaCapture();

                var mediaCaptureController = new MediaCaptureController(captureSource, this.dispatcher);

                this.MediaCaptureVM = new UWPMediaCaptureViewModel(mediaCaptureController, captureSource, this.dispatcher);

                if (this.MediaCaptureVM.InitializeCommand.CanExecute(null))
                {
                    this.MediaCaptureVM.InitializeCommand.Execute(null);
                }
            });
        }

        private async Task ExecuteDeinitialize()
        {
            await this.dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var viewModel = this.MediaCaptureVM;

                this.MediaCaptureVM = null;

                viewModel.Dispose();
            });
        }

        #endregion
    }
}

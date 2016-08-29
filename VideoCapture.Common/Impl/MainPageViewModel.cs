namespace VideoCapature.Common.ViewModels
{
    using System;
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Mvvm;
    using VideoCapture.Common.ViewModel.Interfaces;

    public class MainPageViewModel2 : BindableBase
    {
        #region Constructor(s)

        // TODO: Probably would be better to pass in a factory here instead of THE view model... that
        // way we could create a new view model inside the main page view model to render to another
        // capture element...
        public MainPageViewModel2(string title, IMediaCaptureViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            this.mediaCaptureViewModel = viewModel;

            this.title = title;
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

        private readonly IMediaCaptureViewModel mediaCaptureViewModel;

        public IMediaCaptureViewModel MediaCaptureViewModel
        {
            get { return this.mediaCaptureViewModel; }
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
    }
}

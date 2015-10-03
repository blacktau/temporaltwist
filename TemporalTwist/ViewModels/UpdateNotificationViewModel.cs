namespace TemporalTwist.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using TemporalTwist.Core;

    public class UpdateNotificationViewModel : ClosableViewModelBase
    {
        private readonly UpdateChecker updateChecker;

        public UpdateNotificationViewModel(IMessenger messenger, UpdateChecker updateChecker)
            : base(messenger)
        {
            this.updateChecker = updateChecker;
            this.DownloadCommand = new RelayCommand(this.LaunchDownload);
        }

        public ICommand DownloadCommand { get; private set; }

        public Version CurrentVersion => UpdateChecker.CurrentVersion;

        public Version NewVersion => this.updateChecker.NewVersion;

        public string DownloadUrl => this.updateChecker.DownloadUrl;

        private void LaunchDownload()
        {
            Process.Start(this.DownloadUrl);
        }
    }
}
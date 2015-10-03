namespace TemporalTwist.ViewModels
{
    using System;
    using System.Reflection;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using TemporalTwist.Configuration;
    using TemporalTwist.Core;
    using TemporalTwist.Interfaces.Services;

    public class ConfigurationViewModel : ClosableViewModelBase
    {
        private readonly Configuration configuration;

        private readonly IConfigurationService configurationService;

        private bool isChecking;

        public ConfigurationViewModel(IMessenger messenger, IConfigurationService configurationService) : base(messenger)
        {
            this.configurationService = configurationService;
            this.FormatEditorViewModel = new FormatEditorViewModel();
            this.UpdateCheckCommand = new RelayCommand(this.CheckForUpdates, () => !this.IsChecking);

            this.configuration = this.configurationService.GetConfiguration();
        }

        public FormatEditorViewModel FormatEditorViewModel { get; private set; }

        public ICommand UpdateCheckCommand { get; private set; }

        public bool IsChecking
        {
            get
            {
                return this.isChecking;
            }

            private set
            {
                if (value == this.isChecking)
                {
                    return;
                }

                this.isChecking = value;
                this.RaisePropertyChanged();
            }
        }

        public bool UpdateCheckAtStartup
        {
            get
            {
                return this.configuration.CheckForUpdatesAtStart;
            }

            set
            {
                var currentValue = this.configuration.CheckForUpdatesAtStart;
                if (currentValue == value)
                {
                    return;
                }

                this.configuration.CheckForUpdatesAtStart = value;
                this.configurationService.SaveConfiguration();

                this.RaisePropertyChanged();
            }
        }

        public Version CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version;

        private void CheckForUpdates()
        {
            this.IsChecking = true;
            var checker = new UpdateChecker(this.UpdateChecked);
            checker.CheckForUpdates();
        }

        private void UpdateChecked()
        {
            this.IsChecking = false;
        }
    }
}
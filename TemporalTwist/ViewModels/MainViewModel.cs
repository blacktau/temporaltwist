namespace TemporalTwist.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Microsoft.Win32;

    using TemporalTwist.Engine;
    using TemporalTwist.Interfaces;
    using Model;
    using TemporalTwist.Services;
    using TemporalTwist.Views;

    public class MainViewModel : ViewModelBase
    {
        #region Constants and Fields
        
        private readonly ConsoleViewModel consoleViewModel;

        private JobViewModel job;

        private IJobProcessor processor;

        private readonly ConfigurationService configurationService;

        private readonly IConfigurationViewModelFactory configurationViewModelFactory;

        private readonly IJobViewModelFactory jobViewModelFactory;
        private IJobProcessorFactory jobProcessorFactory;

        #endregion

        #region Constructors and Destructors

        public MainViewModel(
            ConsoleViewModel consoleViewModel, 
            ConfigurationService configurationService, 
            IConfigurationViewModelFactory configurationViewModelFactory, 
            IJobViewModelFactory jobViewModelFactory, 
            IJobProcessorFactory jobProcessorFactory)
        {
            this.configurationService = configurationService;
            this.configurationViewModelFactory = configurationViewModelFactory;
            this.jobViewModelFactory = jobViewModelFactory;
            this.jobProcessorFactory = jobProcessorFactory;

            this.consoleViewModel = consoleViewModel;

            this.LoadConfiguration();

            this.InitialiseCommands();

            Application.Current.Exit += (sender, args) => this.SaveConfiguration();
        }

        #region Properties

        private bool IsStartable => this.Job.IsStartable;

        protected bool CanShowConsole => !this.consoleViewModel.IsVisible;

        #endregion

        #region Public Properties

        public ICommand CloseCommand { get; private set; }

        public ICommand AddFilesCommand { get; private set; }

        public ICommand ConfigureCommand { get; private set; }

        public ICommand DropCommand { get; private set; }

        public ICommand RemoveFilesCommand { get; private set; }

        public ICommand ResetCommand { get; private set; }

        public ICommand ShowConsoleCommand { get; private set; }

        public ICommand StartCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        public JobViewModel Job
        {
            get
            {
                return this.job;
            }

            set
            {
                if (this.job == value)
                {
                    return;
                }

                this.job = value;
                this.RaisePropertyChanged(nameof(this.Job));
            }
        }

        #endregion

        

        #region Public Methods

        public void SaveConfiguration()
        {
            this.configurationService.SaveConfiguration();
        }

        #endregion

        private void LoadConfiguration()
        {
            var configuration = this.configurationService.GetConfiguration();

            if (configuration != null)
            {
                FormatList.Instance.AddRange(configuration.Formats);
                this.job = this.jobViewModelFactory.CreateJobViewModel();
            }
        }

        private void InitialiseCommands()
        {
            this.AddFilesCommand = new RelayCommand(this.AddFiles, () => this.job.IsIdle);
            this.RemoveFilesCommand = new RelayCommand(
                this.RemoveFiles,
                () => this.Job.HasPendingItems && this.job.IsIdle);

            this.StartCommand = new RelayCommand(this.StartProcessing, () => this.IsStartable);

            this.StopCommand = new RelayCommand(this.StopProcessing, () => !this.job.IsIdle);

            this.ResetCommand = new RelayCommand(this.ResetJob, () => this.job.IsIdle && this.job.ItemsProcessed > 0);

            this.ConfigureCommand = new RelayCommand(this.ShowConfiguration, () => this.job.IsIdle);
            this.ShowConsoleCommand = new RelayCommand(this.ShowConsole, () => this.CanShowConsole);
            this.CloseCommand = new RelayCommand(() => { this.SaveConfiguration(); this.consoleViewModel?.CloseCommand.Execute(null); });
            this.DropCommand = new RelayCommand<object>(this.AddDroppedFiles, p => this.job.IsIdle);
        }

        #endregion

        #region Methods

        private void ShowConfiguration()
        {
            var viewModel = this.configurationViewModelFactory.CreateConfigurationViewModel();
            var formatView = new ConfigurationEditorWindow { DataContext = viewModel };
            viewModel.WindowCloseRequester = formatView.Close;
            formatView.ShowDialog();
        }

        private void AddDroppedFiles(object data)
        {
            var files = data as string[];
            if (files != null)
            {
                var validFiles = files.Where(SupportedFileTypes.IsFileSupported).ToArray();
                this.job.AddItems(validFiles);
            }
        }

        private void AddFiles()
        {
            var fileDialog = new OpenFileDialog
                                 {
                                     CheckFileExists = true, 
                                     CheckPathExists = true, 
                                     Multiselect = true, 
                                     Title = "Add Audio Files", 
                                     Filter = SupportedFileTypes.GetFileFilter()
                                 };
            if (fileDialog.ShowDialog() == false)
            {
                return;
            }

            var selectedFiles = fileDialog.FileNames;
            this.job.AddItems(selectedFiles);
        }

        private void RemoveFiles()
        {
            var selectedItems = new IJobItem[this.job.SelectedJobItems.Count];
            this.job.SelectedJobItems.CopyTo(selectedItems, 0);
            foreach (var item in selectedItems)
            {
                this.job.JobItems.Remove(item);
            }

            this.job.SelectedJobItems.Clear();
        }

        private void ResetJob()
        {
            this.job.Reset();
        }

        private void ShowConsole()
        {
            var console = new ConsoleWindow { DataContext = this.consoleViewModel };
            console.IsVisibleChanged += this.consoleViewModel.IsVisibleChanged;
            this.consoleViewModel.WindowCloseRequester = console.Close;
            console.Show();
        }

        private void StartProcessing()
        {
            if (this.job == null)
            {
                return;
            }

            if (this.job.IsComplete)
            {
                this.job.Reset();
            }

            var start = DateTime.Now;
            this.job.IsIdle = false;

            this.processor = this.jobProcessorFactory.CreateJobProcessor(HandleProcessingFinished);

            this.processor.RunAsync(this.job);
        }

        private void HandleProcessingFinished(IJob job)
        {
            MessageBox.Show("Completed: " + (DateTime.Now - job.StartTime));
            this.job.IsIdle = true;
        }

        private void StopProcessing()
        {
            this.processor?.Cancel();
        }

        #endregion
    }
}
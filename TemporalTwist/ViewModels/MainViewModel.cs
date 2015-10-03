namespace TemporalTwist.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Win32;

    using Interfaces;
    using Model;

    using TemporalTwist.Interfaces.Factories;
    using TemporalTwist.Interfaces.Services;
    using TemporalTwist.Messaging;

    using Views;

    using Window = TemporalTwist.Views.Window;

    public class MainViewModel : ViewModelBase
    {
        #region Constants and Fields
        
        private readonly IConfigurationService configurationService;
        private readonly IConfigurationViewModelFactory configurationViewModelFactory;
        private readonly IJobViewModelFactory jobViewModelFactory;
        private readonly IJobProcessorFactory jobProcessorFactory;

        private readonly IShutdownService shutdownService;

        private JobViewModel job;
        private IJobProcessor processor;

        #endregion

        #region Constructors and Destructors

        public MainViewModel(
            IMessenger messenger,
            IConfigurationService configurationService, 
            IConfigurationViewModelFactory configurationViewModelFactory, 
            IJobViewModelFactory jobViewModelFactory, 
            IJobProcessorFactory jobProcessorFactory, 
            IShutdownService shutdownService) : base(messenger)
        {
            this.configurationService = configurationService;
            this.configurationViewModelFactory = configurationViewModelFactory;
            this.jobViewModelFactory = jobViewModelFactory;
            this.jobProcessorFactory = jobProcessorFactory;
            this.shutdownService = shutdownService;

            this.LoadConfiguration();

            this.InitialiseCommands();

            Application.Current.Exit += (sender, args) => this.SaveConfiguration();
        }

        #endregion

        #region Public Properties

        public ICommand AddFilesCommand { get; private set; }

        public ICommand CloseCommand { get; private set; }

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
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Properties

        private bool IsStartable => this.Job.IsStartable;

        #endregion

        #region Public Methods

        public void SaveConfiguration()
        {
            this.configurationService.SaveConfiguration();
        }

        #endregion

        #region Methods

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
            this.ShowConsoleCommand = new RelayCommand(this.ShowConsole);
            this.CloseCommand = new RelayCommand(this.OnClose);
            this.DropCommand = new RelayCommand<object>(this.AddDroppedFiles, p => this.job.IsIdle);
        }

        private void OnClose()
        {
            this.SaveConfiguration();
            this.shutdownService.RequestShutdown();
        }

        private void ShowConfiguration()
        {
            this.MessengerInstance.Send(new OpenWindowRequest(Window.ConfigurationEditor));
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
            this.MessengerInstance.Send(new OpenWindowRequest(Window.Console));
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

            
            this.job.IsIdle = false;

            this.processor = this.jobProcessorFactory.CreateJobProcessor(this.HandleProcessingFinished);

            this.processor.RunAsync(this.job);
        }

        private void HandleProcessingFinished(IJob completedJob)
        {   
            MessageBox.Show("Completed: " + (DateTime.Now - completedJob.StartTime));
            this.job.IsIdle = true;
        }

        private void StopProcessing()
        {
            this.processor?.Cancel();
        }

        #endregion
    }
}
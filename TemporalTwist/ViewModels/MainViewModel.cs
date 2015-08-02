// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="None">
//   Copyright (c) 2009, Sean Garrett
//   All rights reserved.
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and 
//      the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * The names of the contributors may not be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <remarks>
//   The View Model for the main window.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewModels
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Microsoft.Win32;

    using TemporalTwist.Engine;
    using TemporalTwist.Interfaces;
    using TemporalTwist.Model;
    using TemporalTwist.Services;
    using TemporalTwist.Views;

    public class MainViewModel : ViewModelBase
    {
        #region Constants and Fields
        
        private readonly ConsoleViewModel consoleViewModel;

        private JobViewModel job;

        private JobProcessor processor;

        private readonly ConfigurationService configurationService;

        private readonly IConfigurationViewModelFactory configurationViewModelFactory;

        private readonly IJobViewModelFactory jobViewModelFactory;

        #endregion

        #region Constructors and Destructors

        public MainViewModel(ConsoleViewModel consoleViewModel, ConfigurationService configurationService, IConfigurationViewModelFactory configurationViewModelFactory, IJobViewModelFactory jobViewModelFactory)
        {
            this.configurationService = configurationService;
            this.configurationViewModelFactory = configurationViewModelFactory;
            this.jobViewModelFactory = jobViewModelFactory;

            this.consoleViewModel = consoleViewModel;

            this.LoadConfiguration();

            this.InitialiseCommands();

            Application.Current.Exit += (sender, args) => this.SaveConfiguration();
        }

        #region Properties

        protected bool CanShowConsole => !this.consoleViewModel.IsVisible;

        private bool IsStartable =>  this.Job.IsStartable;

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
            this.processor = new JobProcessor
                                 {
                                     ProcessingFinished = () =>
                                         {
                                             MessageBox.Show("Completed: " + (DateTime.Now - start));
                                             this.job.IsIdle = true;
                                         }, 
                                     ConsoleUpdateHandler = this.consoleViewModel.HandleConsoleUpdate
                                 };
            this.processor.RunAsync(this.job);
        }

        private void StopProcessing()
        {
            this.processor?.Cancel();
        }

        #endregion
    }
}
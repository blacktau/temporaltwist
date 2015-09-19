namespace TemporalTwist.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;

    using Core;

    using GalaSoft.MvvmLight.Command;


    using Model;

    using TemporalTwist.Factories;
    using Interfaces;
    using System;

    public class JobViewModel : BaseViewModel, IJob
    {
        private readonly Job job;

        private bool isIdle = true;

        public JobViewModel(JobFactory jobFactory)
        {
            this.job = jobFactory.CreateJob();
            
            this.ChooseOutputFolderCommand = new RelayCommand(this.ChooseOutputFolder);
            this.SelectedJobItems = new ObservableCollection<IJobItem>();
        }

        public bool IsStartable => this.HasPendingItems && this.IsIdle && this.Format != null && !string.IsNullOrEmpty(this.OutputFolder);

        public bool HasPendingItems => this.JobItems.Any(i => i.State != JobItemState.Done);

        public Format Format
        {
            get
            {
                return this.job.Format;
            }

            set
            {
                if (this.job.Format == value)
                {
                    return;
                }

                this.job.Format = value;
                this.RaisePropertyChanged(nameof(this.Format));
            }
        }

        public decimal Tempo
        {
            get
            {
                return this.job.Tempo;
            }

            set
            {
                if (this.job.Tempo == value)
                {
                    return;
                }

                this.job.Tempo = value;
                this.RaisePropertyChanged(nameof(this.Tempo));
                this.RaisePropertyChanged(nameof(this.TempoPercentage));
            }
        }

        public int TempoPercentage
        {
            get
            {
                return (int)(this.job.Tempo * 10);
            }

            set
            {
                var newTempo = (decimal)value / 10;
                if (this.job.Tempo == newTempo)
                {
                    return;
                }

                this.Tempo = newTempo;
            }
        }

        public string OutputFolder
        {
            get
            {
                return this.job.OutputFolder;
            }

            set
            {
                if (this.job.OutputFolder == value)
                {
                    return;
                }

                this.job.OutputFolder = value;
                this.RaisePropertyChanged(nameof(this.OutputFolder));
            }
        }

        public bool IsIdle
        {
            get
            {
                return this.isIdle;
            }

            set
            {
                if (this.isIdle == value)
                {
                    return;
                }
                this.isIdle = value;
                this.RaisePropertyChanged(nameof(this.IsIdle));
            }
        }

        public ObservableCollection<IJobItem> JobItems
        {
            get
            {
                return this.job.JobItems;
            }

            set
            {
                if (this.job.JobItems == value)
                {
                    return;
                }

                this.job.JobItems = value;
                this.RaisePropertyChanged(nameof(this.JobItems));
            }
        }

        public ObservableCollection<IJobItem> SelectedJobItems { get; private set; }

        public ICommand ChooseOutputFolderCommand { get; private set; }

        public int ItemsProcessed
        {
            get
            {
                return this.job?.JobItems.Count(item => item.State != JobItemState.None) ?? 0;
            }
        }

        public bool IsComplete  
        {
            get
            {
                return this.JobItems.All(jobItem => jobItem.Progress >= 100);
            }
        }

        public DateTime StartTime
        {
            get
            {
                return job.StartTime;
            }

            set
            {
                job.StartTime = value;
            }
        }

        internal void AddItems(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                var jobItem = new JobItemViewModel { SourceFile = filePath, Progress = 0 };
                if (!this.job.JobItems.Contains(jobItem))
                {
                    this.job.JobItems.Add(jobItem);
                }
            }
        }

        internal void Reset()
        {
            foreach (var item in this.JobItems)
            {
                item.Reset();
            }
        }

        private void ChooseOutputFolder()
        {

            var folderBrowser = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
            };

            if (!string.IsNullOrEmpty(this.OutputFolder))
            {
                folderBrowser.SelectedPath = this.OutputFolder;
            }

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.OutputFolder = folderBrowser.SelectedPath;
            }
        }
    }
}

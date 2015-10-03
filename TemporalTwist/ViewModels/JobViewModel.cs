namespace TemporalTwist.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using TemporalTwist.Factories;
    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Factories;
    using TemporalTwist.Model;

    public class JobViewModel : ViewModelBase, IJob
    {
        private readonly Job job;

        private bool isIdle = true;

        public JobViewModel(IJobFactory jobFactory)
        {
            this.job = jobFactory.CreateJob();

            this.ChooseOutputFolderCommand = new RelayCommand(this.ChooseOutputFolder);
            this.SelectedJobItems = new ObservableCollection<IJobItem>();
        }

        public bool IsStartable => this.HasPendingItems && this.IsIdle && this.Preset != null && !string.IsNullOrEmpty(this.OutputFolder);

        public bool HasPendingItems => this.JobItems.Any(i => i.State != JobItemState.Done);

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
                this.RaisePropertyChanged();
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

        public Preset Preset
        {
            get
            {
                return this.job.Preset;
            }

            set
            {
                if (this.job.Preset == value)
                {
                    return;
                }

                this.job.Preset = value;
                this.RaisePropertyChanged();
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
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.TempoPercentage));
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
                this.RaisePropertyChanged();
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
                this.RaisePropertyChanged();
            }
        }

        public DateTime StartTime
        {
            get
            {
                return this.job.StartTime;
            }

            set
            {
                this.job.StartTime = value;
            }
        }

        internal void AddItems(string[] filePaths)
        {
            foreach (var filePath in filePaths)
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
            var folderBrowser = new FolderBrowserDialog { ShowNewFolderButton = true };

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
namespace TemporalTwist.ViewModels
{
    using System;
    using System.Collections.Generic;

    using TemporalTwist.Core;
    using Model;
    using Interfaces;

    internal class JobItemViewModel : BaseViewModel, IJobItem
    {
        private readonly JobItem jobItem;

        public JobItemViewModel(JobItem jobItem)
        {
            this.jobItem = jobItem;
        }

        public JobItemViewModel()
            : this(new JobItem())
        {
        }

        public string SourceFile
        {
            get
            {
                return this.jobItem.SourceFile;
            }

            set
            {
                if (this.jobItem.SourceFile == value)
                {
                    return;
                }

                this.jobItem.SourceFile = value;
                this.RaisePropertyChanged();
            }
        }

        public string DestinationFile
        {
            get
            {
                return this.jobItem.DestinationFile;
            }

            set
            {
                if (this.jobItem.DestinationFile == value)
                {
                    return;
                }

                this.jobItem.DestinationFile = value;
                this.RaisePropertyChanged();
            }
        }

        public string LastFile => this.jobItem.LastFile;

        public List<string> TemporaryFiles => this.jobItem.TemporaryFiles;

        public double Progress
        {
            get
            {
                return this.jobItem.Progress;
            }

            set
            {
                if (Math.Abs(this.jobItem.Progress - value) < 0.01)
                {
                    return;
                }

                this.jobItem.Progress = value;
                this.RaisePropertyChanged();
            }
        }

        public JobItemState State
        {
            get
            {
                return this.jobItem.State;
            }

            set
            {
                if (this.jobItem.State == value)
                {
                    return;
                }

                this.jobItem.State = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsBeingProcessed
        {
            get
            {
                return this.jobItem.IsBeingProcessed;
            }

            set
            {
                if (this.jobItem.IsBeingProcessed == value)
                {
                    return;
                }

                this.jobItem.IsBeingProcessed = value;
                this.RaisePropertyChanged();
            }
        }

        public void Reset()
        {
            this.jobItem.Reset();
        }
    }
}
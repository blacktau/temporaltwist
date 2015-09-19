namespace TemporalTwist.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Core;
    using Interfaces;

    internal class JobItem : ObservableObject, IEquatable<IJobItem>, IJobItem
    {
        private bool isBeingProcessed;

        private double progress;

        private string sourceFile;

        private JobItemState state;

        public JobItem()
        {
            this.State = JobItemState.None;
            this.TemporaryFiles = new List<string>();
        }

        public string DestinationFile { get; set; }

        public bool IsBeingProcessed
        {
            get
            {
                return this.isBeingProcessed;
            }

            set
            {
                if (this.isBeingProcessed == value)
                {
                    return;
                }

                this.isBeingProcessed = value;
                this.RaisePropertyChanged(nameof(this.IsBeingProcessed));
            }
        }

        public string LastFile => this.TemporaryFiles[this.TemporaryFiles.Count - 1];

        public double Progress
        {
            get
            {
                return this.progress;
            }

            set
            {
                if (Math.Abs(this.progress - value) < 0.001)
                {
                    return;
                }

                this.progress = value;
                this.RaisePropertyChanged(nameof(this.Progress));
            }
        }

        public JobItemState State
        {
            get
            {
                return this.state;
            }

            set
            {
                if (this.state == value)
                {
                    return;
                }

                this.state = value;
                this.RaisePropertyChanged(nameof(this.State));
            }
        }

        public string SourceFile
        {
            get
            {
                return this.sourceFile;
            }

            set
            {
                if (this.sourceFile != value)
                {
                    this.sourceFile = value;
                    this.RaisePropertyChanged(nameof(this.SourceFile));
                }
            }
        }

        public List<string> TemporaryFiles { get; }

        public bool Equals(IJobItem other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.SourceFile.Equals(this.SourceFile);
        }

        public void Reset()
        {
            this.State = JobItemState.None;
            this.Progress = 0;
            foreach (var file in this.TemporaryFiles)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var inter = obj.GetType().GetInterface(typeof(IJobItem).Name);
            if (inter == null)
            {
                return false;
            }

            return this.Equals((IJobItem)obj);
        }

        public override int GetHashCode()
        {
            return this.SourceFile?.GetHashCode() ?? 0;
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobViewModel.cs" company="None">
//   Copyright (c) 2009, Sean Garrett
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and 
//      the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * The names of the contributors may not be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <remarks>
//   view model for the Job object
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

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

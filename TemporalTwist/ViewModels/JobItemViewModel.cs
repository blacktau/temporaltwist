// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobItemViewModel.cs" company="None">
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
//   ViewModel for a JobItem
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewModels
{
    using System;
    using System.Collections.Generic;

    using TemporalTwist.Core;
    using TemporalTwist.Model;

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
                this.RaisePropertyChanged(nameof(this.SourceFile));
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
                this.RaisePropertyChanged(nameof(this.SourceFile));
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
                this.RaisePropertyChanged(nameof(this.Progress));
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
                this.RaisePropertyChanged(nameof(this.State));
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
                this.RaisePropertyChanged(nameof(this.IsBeingProcessed));
            }
        }

        public void Reset()
        {
            this.jobItem.Reset();
        }
    }
}
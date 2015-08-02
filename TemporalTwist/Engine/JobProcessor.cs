// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobProcessor.cs" company="None">
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
//   Central engine for processing jobs.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace TemporalTwist.Engine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Model;

    using Steps;




    internal class JobProcessor
    {



        private readonly Dictionary<JobItemState, Step> stateMap;




        private BackgroundWorker worker;




        public JobProcessor()
        {
            Action<string> consoleUpdateHandler = x =>
                                                  {
                                                      if (ConsoleUpdateHandler != null)
                                                      {
                                                          ConsoleUpdateHandler.Invoke(x);
                                                      }
                                                  };
            this.stateMap = new Dictionary<JobItemState, Step>
                            {
                                { JobItemState.None, new InitialisationStep() },
                                { JobItemState.Initialised, new DecodingStep(consoleUpdateHandler) },
                                { JobItemState.Decoded, new TempoAdjustmentStep(consoleUpdateHandler) },
                                { JobItemState.TempoAdjusted, new EncodingStep(consoleUpdateHandler) },
                                { JobItemState.Encoded, new TagCopyingStep() },
                                { JobItemState.Tagged, new FileCopyStep() },
                                { JobItemState.Copied, new CleanupStep() }
                            };
        }





        public Action ProcessingFinished { get; set; }





        public Action<string> ConsoleUpdateHandler { get; set; }





        public void RunAsync(IJob job)
        {
            this.worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            this.worker.DoWork += (sender, e) => 
            {
                if (e != null)
                {
                    this.ProcessJob((IJob)e.Argument);
                }
            };
            this.worker.RunWorkerAsync(job);
        }




        public void Cancel()
        {
            if (this.worker != null && this.worker.IsBusy)
            {
                this.worker.CancelAsync();
            }
        }





        private void ProcessJob(IJob job)
        {
            if (job == null)
            {
                return;
            }

            var progressIncrement = 100.0 / this.stateMap.Count;

            foreach (var jobItem in job.JobItems)
            {
                jobItem.IsBeingProcessed = true;
                while (jobItem.State != JobItemState.Done)
                {
                    var nextStep = this.stateMap[jobItem.State];
                    nextStep.ProcessItem(job, jobItem);
                    jobItem.State++;
                    jobItem.Progress = progressIncrement + jobItem.Progress;
                    if (this.worker.CancellationPending)
                    {
                        break;
                    }
                }

                jobItem.IsBeingProcessed = false;

                if (this.worker.CancellationPending)
                {
                    break;
                }
            }

            if (this.ProcessingFinished != null)
            {
                this.ProcessingFinished.Invoke();
            }
        }


    }
}
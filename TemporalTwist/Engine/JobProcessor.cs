namespace TemporalTwist.Engine
{
    using System;
    using System.ComponentModel;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Model;

    public class JobProcessor : IJobProcessor
    {
        private readonly IConsoleOutputBus consoleOutputProcessor;

        private readonly Action<IJob> processingFinishedCallback;

        private readonly IStepStateMapper stepStateMapper;

        private BackgroundWorker worker;

        public JobProcessor(IStepStateMapper stepStateMapper, IConsoleOutputBus consoleOutputProcessor, Action<IJob> processingFinishedCallback)
        {
            this.consoleOutputProcessor = consoleOutputProcessor;
            this.stepStateMapper = stepStateMapper;
            this.processingFinishedCallback = processingFinishedCallback;
        }

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

            job.StartTime = DateTime.Now;
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

            var progressIncrement = 100.0 / this.stepStateMapper.Count;

            foreach (var jobItem in job.JobItems)
            {
                jobItem.IsBeingProcessed = true;
                while (jobItem.State != JobItemState.Done)
                {
                    var nextStep = this.stepStateMapper.GetStepForState(jobItem.State);
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

            this.processingFinishedCallback?.Invoke(job);
        }
    }
}
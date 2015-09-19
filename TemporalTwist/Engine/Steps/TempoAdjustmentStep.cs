namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.Globalization;
    using System.IO;

    using Model;
    using Interfaces.Steps;
    using Interfaces;

    public class TempoAdjustmentStep : ExternalExecutionStep, ITempoAdjustmentStep
    {
        public TempoAdjustmentStep(IConsoleOutputBus consoleOutputHandler)
            : base("soundstretch.exe", consoleOutputHandler)
        {
        }

        public override void ProcessItem(IJob job, IJobItem item)
        {
            var tempo = (int)((job.Tempo * 100) - 100);
            var tempfile = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks + ".wav");
            this.Execute(
                string.Format(
                    CultureInfo.InvariantCulture, 
                    "\"{0}\" \"{1}\" -tempo={2}", 
                    item.LastFile, 
                    tempfile, 
                    tempo));
            item.TemporaryFiles.Add(tempfile);
        }
    }
}
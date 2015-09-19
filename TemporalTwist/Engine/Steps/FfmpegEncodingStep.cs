namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.Globalization;
    using System.IO;

    using Model;
    using Interfaces.Steps;
    using Interfaces;

    public class FfmpegEncodingStep : ExternalExecutionStep, IFfmpegEncodingStep
    {
        public FfmpegEncodingStep(IConsoleOutputBus consoleOutputHandler)
            : base("ffmpeg.exe", consoleOutputHandler)
        {
        }

        public override void ProcessItem(IJob job, IJobItem item)
        {
            var bitRate = job.Format.BitRate;
            var sampleRate = job.Format.SampleRate;
            var tempdir = Path.GetTempPath();
            var tempfile = Path.Combine(tempdir, DateTime.Now.Ticks + "." + job.Format.Extension);
            var exec = string.Format(
                CultureInfo.InvariantCulture, 
                "-i \"{0}\" -ab {1} -ar {2} -ac 2 \"{3}\"", 
                item.LastFile, 
                bitRate, 
                sampleRate, 
                tempfile);
            this.Execute(exec);
            item.TemporaryFiles.Add(tempfile);
        }
    }
}
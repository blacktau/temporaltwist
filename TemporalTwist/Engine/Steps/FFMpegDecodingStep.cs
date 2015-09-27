namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.Globalization;
    using System.IO;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;

    public class FfmpegDecodingStep : ExternalExecutionStep, IDecodingStep
    {
        public FfmpegDecodingStep(IConsoleOutputBus consoleOutputHandler)
            : base("ffmpeg.exe", consoleOutputHandler)
        {
        }

        public override void ProcessItem(IJob job, IJobItem item)
        {
            var tempfile = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks + ".wav");
            item.TemporaryFiles.Add(tempfile);
            this.Execute(string.Format(CultureInfo.InvariantCulture, "-i \"{0}\" \"{1}\" ", item.SourceFile, tempfile));
        }
    }
}
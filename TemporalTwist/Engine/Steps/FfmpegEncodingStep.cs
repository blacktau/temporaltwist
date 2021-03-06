﻿namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.Globalization;
    using System.IO;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;

    public class FfmpegEncodingStep : ExternalExecutionStep, IEncodingStep
    {
        public FfmpegEncodingStep(IConsoleOutputBus consoleOutputHandler)
            : base("ffmpeg.exe", consoleOutputHandler)
        {
        }

        public override void ProcessItem(IJob job, IJobItem item)
        {
            var bitRate = job.Preset.BitRate;
            var sampleRate = job.Preset.SampleRate;
            var tempdir = Path.GetTempPath();
            var tempfile = Path.Combine(tempdir, DateTime.Now.Ticks + "." + job.Preset.Extension);
            var exec = string.Format(CultureInfo.InvariantCulture, "-i \"{0}\" -ab {1} -ar {2} -ac 2 \"{3}\"", item.LastFile, bitRate, sampleRate, tempfile);
            this.Execute(exec);
            item.TemporaryFiles.Add(tempfile);
        }
    }
}
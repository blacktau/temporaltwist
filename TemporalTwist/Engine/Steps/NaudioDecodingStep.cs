namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.IO;

    using NAudio.MediaFoundation;
    using NAudio.Wave;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;

    public class NaudioDecodingStep : Step, IDecodingStep
    {
        public override void ProcessItem(IJob job, IJobItem item)
        {
            var tempfile = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks + ".wav");
            item.TemporaryFiles.Add(tempfile);

            using (var reader = new MediaFoundationReader(item.SourceFile))
            {
                WaveFileWriter.CreateWaveFile(tempfile, reader);
            }
        }
    }
}
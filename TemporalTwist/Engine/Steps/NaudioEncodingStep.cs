namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.IO;

    using NAudio.MediaFoundation;
    using NAudio.Wave;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;

    public class NaudioEncodingStep : Step, IEncodingStep, IDisposable
    {
        public NaudioEncodingStep()
        {
            MediaFoundationApi.Startup();
        }

        public override void ProcessItem(IJob job, IJobItem item)
        {
            var bitRate = job.Format.BitRate;
            var sampleRate = job.Format.SampleRate;
            var tempdir = Path.GetTempPath();
            var tempfile = Path.Combine(tempdir, DateTime.Now.Ticks + "." + job.Format.Extension);

            var subType = this.GetAudioSubtypeForExtension(job.Format.Extension);
            var waveFormat = new WaveFormat(sampleRate, 2);

            var mediaType = MediaFoundationEncoder.SelectMediaType(subType, waveFormat, bitRate);

            if (mediaType != null)
            {
                using (var decoder = new MediaFoundationReader(item.LastFile))
                {
                    using (var encoder = new MediaFoundationEncoder(mediaType))
                    {
                        encoder.Encode(tempfile, decoder);
                    }
                }
            }

            item.TemporaryFiles.Add(tempfile);
        }

        public void Dispose()
        {
            MediaFoundationApi.Shutdown();
        }

        private Guid GetAudioSubtypeForExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case "mp3":
                    return AudioSubtypes.MFAudioFormat_MP3;

                default:
                    return AudioSubtypes.MFAudioFormat_PCM;
            }
        }
    }
}
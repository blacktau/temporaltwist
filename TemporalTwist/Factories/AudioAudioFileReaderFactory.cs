namespace TemporalTwist.Factories
{
    using System.IO;

    using NAudio.Vorbis;
    using NAudio.Wave;
    using NAudio.WindowsMediaFormat;

    using TemporalTwist.Interfaces;

    public class AudioAudioFileReaderFactory : IAudioFileReaderFactory
    {
        public WaveStream GetReaderForFile(string path)
        {
            var extension = Path.GetExtension(path);
            if (extension == null)
            {
                return null;
            }

            extension = extension.ToLower();

            switch (extension)
            {
                case "mp3":
                case "wav":
                case "aiff":
                    return new AudioFileReader(path);
                case "ogg":
                    return new VorbisWaveReader(path);
                case "wma":
                    return new WMAFileReader(path);
            }

            return null;
        }
    }
}
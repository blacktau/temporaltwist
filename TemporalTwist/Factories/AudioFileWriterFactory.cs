namespace TemporalTwist.Factories
{
    using System.IO;

    using NAudio.Vorbis;
    using NAudio.Wave;
    using NAudio.WindowsMediaFormat;

    public class AudioFileWriterFactory
    {
        public WaveStream GetWriterForFile(string path)
        {
/*            var extension = Path.GetExtension(path);
            if (extension == null)
            {
                return null;
            }

            extension = extension.ToLower();

            
            switch (extension)
            {
                case "wav":

                case "mp3":
                case "aiff":
                    return new WaveFileWriter(path);
                case "ogg":
                    return new VorbisWaveReader(path);
                case "wma":
                    return new WMAFileReader(path);
            }*/

            return null;
        }
    }
}
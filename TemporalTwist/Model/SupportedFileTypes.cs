namespace TemporalTwist.Model
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal static class SupportedFileTypes
    {
        private static readonly Dictionary<string, string> FileTypes = new Dictionary<string, string>
                                                                           {
                                                                               { ".mp3", "MPEG Audio Layer 3" },
                                                                               { ".mp4", "MP4 Format" },
                                                                               { ".m4a", "MP4 Audio" },
                                                                               { ".m4b", "MP4 Audio (bookmarkable)" },
                                                                               { ".ogg", "Ogg Vorbis" },
                                                                               { ".wav", "WAV Format" }
                                                                           };

        public static string GetFileFilter()
        {
            var builder = new StringBuilder();
            foreach (var pair in FileTypes)
            {
                builder.AppendFormat("{1} (*{0})|*{0}|", pair.Key, pair.Value);
            }

            builder.Append("All Files (*.*)|*.*");
            return builder.ToString();
        }

        public static bool IsFileSupported(string path)
        {
            var fileInfo = new FileInfo(path);
            var extension = fileInfo.Extension.ToLower();
            return FileTypes.ContainsKey(extension);
        }
    }
}
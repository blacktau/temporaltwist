namespace TemporalTwist.Configuration
{
    using System.Collections.Generic;

    using TemporalTwist.Model;

    public class Configuration
    {
        public List<Format> Formats { get; set; }

        public bool CheckForUpdatesAtStart { get; set; }

        public string OutputFolder { get; set; }

        public string SelectedFormat { get; set; }

        public decimal Tempo { get; set; }
    }
}
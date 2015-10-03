namespace TemporalTwist.Configuration
{
    using System.Collections.Generic;

    using TemporalTwist.Model;

    public class Configuration : IConfiguration
    {
        public List<Preset> Formats { get; set; }

        public bool CheckForUpdatesAtStart { get; set; }

        public string OutputFolder { get; set; }

        public string SelectedPreset { get; set; }

        public decimal Tempo { get; set; }
    }
}
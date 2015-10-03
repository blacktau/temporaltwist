namespace TemporalTwist.Configuration
{
    using System.Collections.Generic;

    using TemporalTwist.Model;

    public interface IConfiguration
    {
        List<Preset> Formats { get; set; }

        bool CheckForUpdatesAtStart { get; set; }

        string OutputFolder { get; set; }

        string SelectedPreset { get; set; }

        decimal Tempo { get; set; }
    }
}
namespace TemporalTwist.Interfaces
{
    using System;
    using System.Collections.ObjectModel;
    using Model;

    public interface IJob
    {
        Format Format { get; set; }

        decimal Tempo { get; set; }

        string OutputFolder { get; set; }

        ObservableCollection<IJobItem> JobItems { get; set; }
        DateTime StartTime { get; set; }
    }
}
namespace TemporalTwist.Interfaces
{
    using System.Collections.Generic;
    using Model;

    public interface IJobItem
    {
        string SourceFile { get; set; }

        string DestinationFile { get; set; }

        string LastFile { get; }

        List<string> TemporaryFiles { get; }

        double Progress { get; set; }

        bool IsBeingProcessed { get; set; }

        JobItemState State { get; set; }

        void Reset();
    }
}
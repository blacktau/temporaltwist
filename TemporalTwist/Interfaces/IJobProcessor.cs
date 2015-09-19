namespace TemporalTwist.Interfaces
{
    using System;

    public interface IJobProcessor
    {
        void Cancel();

        void RunAsync(IJob job);
    }
}
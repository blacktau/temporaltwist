namespace TemporalTwist.Interfaces.Factories
{
    using System;

    public interface IJobProcessorFactory
    {
        IJobProcessor CreateJobProcessor(Action<IJob> processingFinishedCallback);
    }
}

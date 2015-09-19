namespace TemporalTwist.Interfaces.Steps
{
    using TemporalTwist.Model;

    public interface IStep
    {
        void ProcessItem(IJob job, IJobItem item);
    }
}
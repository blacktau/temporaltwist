namespace TemporalTwist.Interfaces
{
    using TemporalTwist.Interfaces.Steps;
    using TemporalTwist.Model;

    public interface IStepStateMapper
    {
        int Count { get; }

        IStep GetStepForState(JobItemState itemState);
    }
}
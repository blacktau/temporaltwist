namespace TemporalTwist.Interfaces
{
    using TemporalTwist.ViewModels;

    public interface IJobViewModelFactory
    {
        JobViewModel CreateJobViewModel();
    }
}
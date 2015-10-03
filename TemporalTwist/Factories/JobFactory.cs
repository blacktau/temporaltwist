namespace TemporalTwist.Factories
{
    using TemporalTwist.Interfaces.Factories;
    using TemporalTwist.Interfaces.Services;
    using TemporalTwist.Model;

    public class JobFactory : IJobFactory
    {
        private readonly IConfigurationService configurationService;

        public JobFactory(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public Job CreateJob()
        {
            var configuration = this.configurationService.GetConfiguration();
            return new Job(configuration);
        }
    }
}
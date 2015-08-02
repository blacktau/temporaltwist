namespace TemporalTwist.Factories
{
    using System.Linq;

    using TemporalTwist.Configuration;
    using TemporalTwist.Model;
    using TemporalTwist.Services;

    public class JobFactory
    {
        private readonly ConfigurationService configurationService;

        public JobFactory(ConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public Job CreateJob()
        {
            var configuration = this.configurationService.GetConfiguration();
            var job = new Job(configuration);

            return job;
        }
    }
}
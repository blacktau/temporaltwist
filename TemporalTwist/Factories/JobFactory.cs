namespace TemporalTwist.Factories
{
    using Model;

    using Services;

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
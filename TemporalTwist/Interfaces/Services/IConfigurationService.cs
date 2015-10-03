namespace TemporalTwist.Interfaces.Services
{
    using TemporalTwist.Configuration;

    public interface IConfigurationService
    {
        Configuration GetConfiguration();

        void SaveConfiguration();
    }
}
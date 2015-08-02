namespace TemporalTwist.Interfaces
{
    using System.Security.Cryptography.X509Certificates;

    using TemporalTwist.ViewModels;

    public interface IConfigurationViewModelFactory
    {
        ConfigurationViewModel CreateConfigurationViewModel();
    }
}
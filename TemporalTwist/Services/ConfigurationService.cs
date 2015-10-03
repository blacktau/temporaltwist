namespace TemporalTwist.Services
{
    using System.IO;
    using System.Reflection;

    using Newtonsoft.Json;

    using TemporalTwist.Configuration;
    using TemporalTwist.Interfaces.Services;

    public class ConfigurationService : IConfigurationService
    {
        private Configuration configuration;

        public Configuration GetConfiguration()
        {
            if (this.configuration == null)
            {
                var configurationFile = GetConfigurationFile();
                this.configuration = JsonConvert.DeserializeObject<Configuration>(configurationFile);
            }

            return this.configuration;
        }

        public void SaveConfiguration()
        {
            var json = JsonConvert.SerializeObject(this.configuration, Formatting.Indented);
            File.WriteAllText(GetConfigurationFilePath(), json);
        }

        private static string GetConfigurationFile()
        {
            return File.ReadAllText(GetConfigurationFilePath());
        }

        private static string GetConfigurationFilePath()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directory != null)
            {
                return Path.Combine(directory, "configuration.json");
            }

            throw new FileNotFoundException(Assembly.GetExecutingAssembly().Location);
        }
    }
}
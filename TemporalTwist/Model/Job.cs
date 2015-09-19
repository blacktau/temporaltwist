namespace TemporalTwist.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Configuration;
    using Interfaces;

    public class Job : IJob
    {
        private readonly Configuration configuration;

        public Job(Configuration configuration)
        {
            this.configuration = configuration;
            this.JobItems = new ObservableCollection<IJobItem>();
        }

        public Format Format
        {
            get
            {
                return GetFormatFromConfiguration(this.configuration);
            }

            set
            {
                this.configuration.SelectedFormat = value.Name;
            }
        }

        public decimal Tempo
        {
            get
            {
                return this.configuration.Tempo;
            }

            set
            {
                this.configuration.Tempo = value;
            }
        }

        public string OutputFolder
        {
            get
            {
                return this.configuration.OutputFolder;
            }

            set
            {
                this.configuration.OutputFolder = value;
            }
        }

        public ObservableCollection<IJobItem> JobItems { get; set; }

        public DateTime StartTime { get; set; }

        private static Format GetFormatFromConfiguration(Configuration configuration)
        {
            return configuration.Formats.FirstOrDefault(f => f.Name.Equals(configuration.SelectedFormat));
        }
    }
}
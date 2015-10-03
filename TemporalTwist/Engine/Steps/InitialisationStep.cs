namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.IO;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;

    public class InitialisationStep : Step, IInitialisationStep
    {
        public override void ProcessItem(IJob job, IJobItem item)
        {
            var sourceFile = new FileInfo(item.SourceFile);

            var sourceName = this.GetSourceName(sourceFile);

            item.DestinationFile = this.CreateDestinationFilePath(sourceName, job);
        }

        private static string GetDestinationExtension(IJob job)
        {
            return job.Preset.CustomExtension.StartsWith(".", StringComparison.CurrentCultureIgnoreCase) ? job.Preset.CustomExtension : "." + job.Preset.CustomExtension;
        }

        private string GetSourceName(FileInfo sourceFile)
        {
            var idx = sourceFile.Name.LastIndexOf(sourceFile.Extension, StringComparison.OrdinalIgnoreCase);
            return sourceFile.Name.Remove(idx);
        }

        private string CreateDestinationFilePath(string sourceName, IJob job)
        {
            var extension = GetDestinationExtension(job);

            return Path.Combine(job.OutputFolder, sourceName + extension);
        }
    }
}
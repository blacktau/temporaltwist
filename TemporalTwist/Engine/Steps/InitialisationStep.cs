namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.IO;

    using Interfaces;
    using Interfaces.Steps;

    public class InitialisationStep : Step, IInitialisationStep
    {
        public override void ProcessItem(IJob job, IJobItem item)
        {
            var sourceFile = new FileInfo(item.SourceFile);

            var sourceName = this.GetSourceName(sourceFile);
                        
            item.DestinationFile = CreateDestinationFilePath(sourceName, job);
        }

        private string GetSourceName(FileInfo sourceFile)
        {
            var idx = sourceFile.Name.LastIndexOf(sourceFile.Extension, StringComparison.OrdinalIgnoreCase);
            return sourceFile.Name.Remove(idx);
        }

        private string CreateDestinationFilePath(string sourceName, IJob job)
        {
            string extension = GetDestinationExtension(job);

            return Path.Combine(job.OutputFolder, sourceName + extension);
        }

        private static string GetDestinationExtension(IJob job)
        {
            return job.Format.CustomExtension.StartsWith(".", StringComparison.CurrentCultureIgnoreCase)
                                ? job.Format.CustomExtension
                                : "." + job.Format.CustomExtension;
        }
    }
}
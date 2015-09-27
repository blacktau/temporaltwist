namespace TemporalTwist.Engine.Steps
{
    using System.IO;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;

    public class FileCopyStep : Step, IFileCopyStep
    {
        public override void ProcessItem(IJob job, IJobItem item)
        {
            if (!Directory.Exists(job.OutputFolder))
            {
                Directory.CreateDirectory(job.OutputFolder);
            }

            File.Copy(item.LastFile, item.DestinationFile, true);
        }
    }
}
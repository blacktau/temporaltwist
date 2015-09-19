namespace TemporalTwist.Engine.Steps
{
    using System.IO;

    using Interfaces.Steps;
    using Interfaces;

    public class CleanupStep : Step, ICleanupStep
    {
        public override void ProcessItem(IJob job, IJobItem item)
        {
            foreach (var file in item.TemporaryFiles)
            {
                File.Delete(file);
            }
        }
    }
}
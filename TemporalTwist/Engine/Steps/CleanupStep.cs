namespace TemporalTwist.Engine.Steps
{
    using System.IO;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;

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
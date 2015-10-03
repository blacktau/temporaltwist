namespace TemporalTwist.Interfaces.Factories
{
    using TemporalTwist.Interfaces.Steps;

    public interface IStepFactory
    {
        IInitialisationStep CreateInitialisationStep();

        IDecodingStep CreateFfmpegDecodingStep();

        IEncodingStep CreateFfmpegEncodingStep();

        ITempoAdjustmentStep CreateTempoAdjustmentStep();

        ITagCopyingStep CreateTagCopyingStep();

        IFileCopyStep CreateFileCopyStep();

        ICleanupStep CreateCleanupStep();
    }
}
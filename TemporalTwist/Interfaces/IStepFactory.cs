namespace TemporalTwist.Interfaces
{
    using Engine.Steps;
    using Steps;

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
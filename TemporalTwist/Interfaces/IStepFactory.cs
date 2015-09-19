namespace TemporalTwist.Interfaces
{
    using Engine.Steps;
    using Steps;

    public interface IStepFactory
    {
        IInitialisationStep CreateInitialisationStep();

        IFfmpegDecodingStep CreateFfmpegDecodingStep();

        IFfmpegEncodingStep CreateFfmpegEncodingStep();

        ITempoAdjustmentStep CreateTempoAdjustmentStep();

        ITagCopyingStep CreateTagCopyingStep();

        IFileCopyStep CreateFileCopyStep();

        ICleanupStep CreateCleanupStep();
    }
}
namespace TemporalTwist.Interfaces.Factories
{
    using NAudio.Wave;

    public interface IAudioFileReaderFactory
    {
        WaveStream GetReaderForFile(string path);
    }
}
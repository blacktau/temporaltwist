namespace TemporalTwist.Interfaces
{
    using NAudio.Wave;

    public interface IAudioFileReaderFactory
    {
        WaveStream GetReaderForFile(string path);
    }
}
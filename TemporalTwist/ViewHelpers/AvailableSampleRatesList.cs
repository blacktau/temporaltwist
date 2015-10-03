namespace TemporalTwist.ViewHelpers
{
    using System.Collections.Generic;

    internal class AvailableSampleRatesList : List<int>
    {
        public AvailableSampleRatesList()
        {
            this.AddRange(new[] { 32000, 44100, 48000 });
        }
    }
}
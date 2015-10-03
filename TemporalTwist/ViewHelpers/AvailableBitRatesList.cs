namespace TemporalTwist.ViewHelpers
{
    using System.Collections.Generic;

    internal class AvailableBitRatesList : List<int>
    {
        public AvailableBitRatesList()
        {
            this.AddRange(new[] { 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000, 160000, 192000, 224000, 256000, 320000 });
        }
    }
}
namespace TemporalTwist.Model
{
    using System.Collections.Generic;

    internal class FormatList : List<Format>
    {
        private FormatList()
        {
        }

        public static FormatList Instance { get; } = new FormatList();
    }
}
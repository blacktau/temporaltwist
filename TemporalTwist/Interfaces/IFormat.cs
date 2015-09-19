namespace TemporalTwist.Model
{
    using System;
    using System.Collections.Generic;

    using TagLib;

    public interface IFormat : ICloneable, IEquatable<IFormat>
    {
        string Extension { get; set; }

        string CustomExtension { get; set; }

        int BitRate { get; set; }

        int SampleRate { get; set; }

        string Name { get; set; }

        IList<TagTypes> TagTypes { get; set; }
    }
}
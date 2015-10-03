namespace TemporalTwist.ViewHelpers
{
    using System;
    using System.Collections.Generic;

    using TagLib;

    internal class AvailableTagTypesList : List<TagTypes>
    {
        public AvailableTagTypesList()
        {
            this.InitialiseTagTypes();
        }

        private void InitialiseTagTypes()
        {
            foreach (var tagType in Enum.GetValues(typeof(TagTypes)))
            {
                // exclude all tagTypes and none because it doesn't make sense.
                var tag = (TagTypes)tagType;
                if (tag != TagTypes.None && tag != TagTypes.AllTags)
                {
                    this.Add((TagTypes)tagType);
                }
            }

            this.Sort(
                (x, y) =>
                    {
                        var nameX = Enum.GetName(typeof(TagTypes), x);
                        var nameY = Enum.GetName(typeof(TagTypes), y);
                        return string.Compare(nameX, nameY, StringComparison.InvariantCultureIgnoreCase);
                    });
        }
    }
}
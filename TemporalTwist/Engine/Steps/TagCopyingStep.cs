namespace TemporalTwist.Engine.Steps
{
    using System;

    using TagLib;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;

    public class TagCopyingStep : Step, ITagCopyingStep
    {
        public override void ProcessItem(IJob job, IJobItem item)
        {
            var sourceTagFile = File.Create(item.SourceFile);
            var targetTagFile = File.Create(item.LastFile);

            var allTagTypes = (TagTypes[])Enum.GetValues(typeof(TagTypes));
            foreach (var type in allTagTypes)
            {
                var sourceTag = sourceTagFile.GetTag(type, false);
                if (sourceTag != null)
                {
                    foreach (var tagType in job.Format.TagTypes)
                    {
                        var targetTag = targetTagFile.GetTag(tagType, true);
                        if (targetTag != null)
                        {
                            CopyTags(sourceTag, targetTag);
                        }
                    }
                }
            }

            targetTagFile.Save();
        }

        private static void CopyTags(Tag source, Tag target)
        {
            source.CopyTo(target, true);
            var l = source.Pictures.Length;
            var pictures = new IPicture[l];
            for (var i = 0; i < source.Pictures.Length; i++)
            {
                pictures[i] = new Picture(source.Pictures[i].Data)
                                  {
                                      MimeType = source.Pictures[i].MimeType,
                                      Description = source.Pictures[i].Description,
                                      Type = source.Pictures[i].Type
                                  };
            }

            target.Pictures = pictures;
        }
    }
}
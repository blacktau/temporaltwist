// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagCopyingStep.cs" company="None">
//   Copyright (c) 2009, Sean Garrett
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and 
//      the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * The names of the contributors may not be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <remarks>
//   A Step which copies the Tags from the source file to the destination file. 
//   It tries a one to one copy if the source files extension is the same as the of the destination.
//   If the extensions are not the same the Tag format defined in the job.Format is used.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace TemporalTwist.Engine.Steps
{
    using System;

    using Model;

    using TagLib;




    internal class TagCopyingStep : Step
    {





        internal override void ProcessItem(IJob job, IJobItem item)
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
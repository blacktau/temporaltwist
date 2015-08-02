// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupportedFileTypes.cs" company="None">
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
// centralised list of supported file types. only used for input currently.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace TemporalTwist.Model
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;




    internal static class SupportedFileTypes
    {
        private static readonly Dictionary<string, string> _fileTypes = new Dictionary<string, string>
                                                           {
                                                               { ".mp3", "MPEG Audio Layer 3" },
                                                               { ".mp4", "MP4 Format" },
                                                               { ".m4a", "MP4 Audio" },
                                                               { ".m4b", "MP4 Audio (bookmarkable)" },
                                                               { ".ogg", "Ogg Vorbis" },
                                                               { ".wav", "WAV Format" }
                                                           };

        public static string GetFileFilter()
        {
            var builder = new StringBuilder();
            foreach (var pair in _fileTypes)
            {
                builder.AppendFormat("{1} (*{0})|*{0}|", pair.Key, pair.Value);
            }

            builder.Append("All Files (*.*)|*.*");
            return builder.ToString();
        }

        public static bool IsFileSupported(string path)
        {
            var fileInfo = new FileInfo(path);
            var extension = fileInfo.Extension.ToLower();
            return _fileTypes.ContainsKey(extension);
        }
    }
}

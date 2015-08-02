// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagTypeListConfigurationValidator.cs" company="None">
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
//   validates if an object can be converted to a TagTypeArray
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace TemporalTwist.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using TagLib;




    internal class TagTypeListConfigurationValidator : ConfigurationValidatorBase
    {





        public override void Validate(object value)
        {
            if (value != null && value is IEnumerable<TagTypes>)
            {
                return;
            }

            var val = value as string;

            if (val != null)
            {
                var parts = val.Split(',');
                var result = new TagTypes[parts.Length];
                for (var i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    result[i] = (TagTypes)Enum.Parse(typeof(TagTypes), part, true);
                }
            }

            throw new ArgumentException("Invalid input for IList<TagTypes>");
        }








        public override bool CanValidate(Type type)
        {
            return type == typeof(string) || type == typeof(IEnumerable<TagTypes>);
        }
    }
}
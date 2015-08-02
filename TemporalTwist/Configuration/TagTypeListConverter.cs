// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagTypeListConverter.cs" company="None">
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
//   converts a string to tag type array and back again
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace TemporalTwist.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;

    using TagLib;




    internal class TagTypeListConverter : TypeConverter
    {











        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }











        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }










        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var val = value as string;
            if (val != null)
            {
                var parts = val.Split(',');
                var result = new List<TagTypes>(parts.Length);
                for (var i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    try
                    {
                        result.Add((TagTypes)Enum.Parse(typeof(TagTypes), part, true));
                    }
                    catch
                    {
                        // noop - we just want to ignore invalid values
                    }
                }

                return result;
            }

            return base.ConvertFrom(context, culture, value);
        }



















        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var tagTypes = value as IList<TagTypes>;
            if (tagTypes != null && destinationType == typeof(string))
            {
                var sb = new StringBuilder();
                foreach (var tagType in tagTypes)
                {
                    sb.AppendFormat("{0},", Enum.GetName(typeof(TagTypes), tagType));
                }

                if (sb.Length > 1)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                
                return sb.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
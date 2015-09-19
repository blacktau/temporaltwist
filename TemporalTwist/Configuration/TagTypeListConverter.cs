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
                foreach (var part in parts)
                {
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

        public override object ConvertTo(
            ITypeDescriptorContext context, 
            CultureInfo culture, 
            object value, 
            Type destinationType)
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
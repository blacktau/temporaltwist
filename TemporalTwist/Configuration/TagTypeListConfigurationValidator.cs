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
            if (value is IEnumerable<TagTypes>)
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
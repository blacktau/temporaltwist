namespace TemporalTwist.Configuration
{
    using System;
    using System.Configuration;
    using System.Globalization;

    internal class DecimalValidator : ConfigurationValidatorBase
    {
        private readonly decimal maxValue;

        private readonly decimal minValue;

        public DecimalValidator(decimal minValue, decimal maxValue)
        {
            this.minValue = decimal.MinValue;
            this.maxValue = decimal.MaxValue;

            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), "minValue must be less or equal too maxValue");
            }
        }

        public override bool CanValidate(Type type)
        {
            return type == typeof(decimal);
        }

        public override void Validate(object value)
        {
            if (!(value is decimal))
            {
                throw new ArgumentException("can only validate decimals", nameof(value));
            }

            var decimalValue = (decimal)value;
            if (decimalValue <= this.minValue || decimalValue >= this.maxValue)
            {
                var message = string.Format(CultureInfo.CurrentCulture, "value must be in the range {0} to {1}", this.minValue, this.maxValue);
                throw new ArgumentOutOfRangeException(nameof(value),  message);
            }
        }
    }
}
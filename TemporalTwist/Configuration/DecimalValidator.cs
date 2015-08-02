﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimalValidator.cs" company="None">
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
//   A validator similar to the IntegerValidator but for Decimals. Not all options available on the IntegerValidator 
//   have been implemented. only what is required now.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace TemporalTwist.Configuration
{
    using System;
    using System.Configuration;




    internal class DecimalValidator : ConfigurationValidatorBase 
    {



        private readonly decimal _minValue;




        private readonly decimal _maxValue;







        public DecimalValidator(decimal minValue, decimal maxValue)
        {
            _minValue = decimal.MinValue;
            _maxValue = decimal.MaxValue;

            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException("minValue", "minValue must be less or equal too maxValue");
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
                throw new ArgumentException("can only validate decimals", "value");
            }

            var decimalValue = (decimal) value;
            if (decimalValue <= _minValue || decimalValue >= _maxValue)
            {
                throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, "value must be in the range {0} to {1}", _minValue, _maxValue));
            }
        }
    }
}

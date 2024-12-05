using System;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class NumericRangeAttribute : ValidationAttributeBase
    {
        private readonly double _max;
        private readonly double _min;

        internal NumericRangeAttribute(double min, double max)
        {
            _min = min;
            _max = max;
        }

        internal override (bool isValid, string error) Validate(object value)
        {
            if (value == null)
                return (false, "Value cannot be null");

            if (!IsNumeric(value))
                return (false, $"Expected numeric type but got {value.GetType().Name}");

            var number = Convert.ToDouble(value);
            if (number < _min)
                return (false, $"Value must be at least {_min}");

            if (number > _max)
                return (false, $"Value must be at most {_max}");

            return (true, null);
        }

        private static bool IsNumeric(object value) =>
            value is sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal;
    }
}
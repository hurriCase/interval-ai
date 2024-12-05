using System;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class StringLengthAttribute : ValidationAttributeBase
    {
        private readonly int _max;
        private readonly int _min;

        internal StringLengthAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        internal override (bool isValid, string error) Validate(object value)
        {
            if (value is not string str)
                return (false, $"Expected string but got {value?.GetType().Name ?? "null"}");

            if (str.Length < _min)
                return (false, $"Length must be at least {_min}");

            if (str.Length > _max)
                return (false, $"Length must be at most {_max}");

            return (true, null);
        }
    }
}
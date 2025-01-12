using System;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class RequiredAttribute : ValidationAttributeBase
    {
        internal override (bool isValid, string error) Validate(object value)
        {
            return value switch
            {
                null => (false, "Value cannot be null"),
                string str when string.IsNullOrWhiteSpace(str) => (false, "Value cannot be empty or whitespace"),
                int i when i == 0 => (false, "Value cannot be zero"),
                double d when Math.Abs(d) < double.Epsilon => (false, "Value cannot be zero"),
                float f when Math.Abs(f) < float.Epsilon => (false, "Value cannot be zero"),
                decimal d when d == 0 => (false, "Value cannot be zero"),
                long l when l == 0 => (false, "Value cannot be zero"),
                _ => (true, null)
            };
        }
    }
}
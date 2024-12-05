using System;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    internal abstract class ValidationAttributeBase : Attribute
    {
        internal abstract (bool isValid, string error) Validate(object value);
    }
}
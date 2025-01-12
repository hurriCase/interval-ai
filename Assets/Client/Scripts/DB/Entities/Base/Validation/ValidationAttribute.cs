using System;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class ValidationAttribute : Attribute { }
}
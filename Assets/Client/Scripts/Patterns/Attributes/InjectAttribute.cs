using System;

namespace Client.Scripts.Patterns.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class InjectAttribute : Attribute { }
}
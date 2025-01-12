using System;
using UnityEngine;

namespace Client.Scripts.Patterns.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class RequiredFieldAttribute : PropertyAttribute { }
}
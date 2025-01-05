using System;
using UnityEngine;

namespace Client.Scripts.Patterns
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class RequiredFieldAttribute : PropertyAttribute { }
}
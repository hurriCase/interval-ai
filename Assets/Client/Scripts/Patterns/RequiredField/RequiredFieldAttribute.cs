using System;
using UnityEngine;

namespace Client.Scripts.Patterns.RequiredField
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class RequiredFieldAttribute : PropertyAttribute { }
}
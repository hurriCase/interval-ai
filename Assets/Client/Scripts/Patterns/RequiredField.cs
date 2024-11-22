using System;
using UnityEngine;

namespace Client.Scripts.Patterns
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredField : PropertyAttribute { }
}
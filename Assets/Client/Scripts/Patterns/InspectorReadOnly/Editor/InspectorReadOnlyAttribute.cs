using UnityEngine;

namespace Client.Scripts.Patterns.InspectorReadOnly.Editor
{
    /// <summary>
    /// Attribute that makes a field read-only in the Unity Inspector.
    /// Can be applied to any serialized field to prevent editing while still showing the value.
    /// </summary>
    /// <remarks>
    /// Usage: [InspectorReadOnly] private string myField;
    /// </remarks>
    internal sealed class InspectorReadOnlyAttribute : PropertyAttribute { }
}
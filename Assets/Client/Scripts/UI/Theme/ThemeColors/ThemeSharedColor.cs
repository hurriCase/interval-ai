using System;
using Client.Scripts.UI.Theme.Base;
using CustomUtils.Runtime.Attributes;
using UnityEngine;

namespace Client.Scripts.UI.Theme.ThemeColors
{
    [Serializable]
    internal sealed class ThemeSharedColor : IThemeColor
    {
        [field: SerializeField, InspectorReadOnly] public string Name { get; private set; }
        [field: SerializeField, InspectorReadOnly] internal Color Color { get; private set; }
    }
}
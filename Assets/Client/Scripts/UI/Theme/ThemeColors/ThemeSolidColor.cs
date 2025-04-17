using System;
using Client.Scripts.UI.Theme.Base;
using CustomAttributes.Runtime.Attributes;
using UnityEngine;

namespace Client.Scripts.UI.Theme.ThemeColors
{
    [Serializable]
    internal sealed class ThemeSolidColor : IThemeColor
    {
        [field: SerializeField, InspectorReadOnly] public string Name { get; private set; }
        [field: SerializeField, InspectorReadOnly] internal Color LightThemeColor { get; private set; }
        [field: SerializeField, InspectorReadOnly] internal Color DarkThemeColor { get; private set; }
    }
}
using System;
using UnityEngine;

namespace Client.Scripts.UI.Base.Colors
{
    [Serializable]
    internal struct ThemeGradientColor : IThemeColor
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] internal Gradient LightThemeColor { get; private set; }
        [field: SerializeField] internal Gradient DarkThemeColor { get; private set; }
    }
}
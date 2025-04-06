using System;
using Client.Scripts.UI.Theme.Base;
using UnityEngine;

namespace Client.Scripts.UI.Theme.ThemeColors
{
    [Serializable]
    internal struct ThemeGradientColor : IThemeColor
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] internal Gradient LightThemeColor { get; private set; }
        [field: SerializeField] internal Gradient DarkThemeColor { get; private set; }
    }
}
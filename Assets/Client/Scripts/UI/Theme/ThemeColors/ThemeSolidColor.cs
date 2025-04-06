using System;
using Client.Scripts.UI.Theme.Base;
using UnityEngine;

namespace Client.Scripts.UI.Theme.ThemeColors
{
    [Serializable]
    internal struct ThemeSolidColor : IThemeColor
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] internal Color LightThemeColor { get; private set; }
        [field: SerializeField] internal Color DarkThemeColor { get; private set; }
    }
}
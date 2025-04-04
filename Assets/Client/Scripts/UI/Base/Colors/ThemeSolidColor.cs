using System;
using UnityEngine;

namespace Client.Scripts.UI.Base.Colors
{
    [Serializable]
    internal struct ThemeSolidColor : IThemeColor
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] internal Color LightThemeColor { get; private set; }
        [field: SerializeField] internal Color DarkThemeColor { get; private set; }
    }
}
using System;
using Client.Scripts.UI.Base.Theme;
using UnityEngine;

namespace Client.Scripts.UI.Base.Colors.Base
{
    [Serializable]
    internal abstract class ThemeColorBase<TColor> : IThemeColor
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] internal TColor LightThemeColor { get; private set; }
        [field: SerializeField] internal TColor DarkThemeColor { get; private set; }
    }
}
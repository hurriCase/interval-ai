using System;
using Client.Scripts.UI.Theme.Base;
using UnityEngine;

namespace Client.Scripts.UI.Theme.ThemeColors
{
    [Serializable]
    internal struct ThemeSharedColor : IThemeColor
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] internal Color Color { get; private set; }
    }
}
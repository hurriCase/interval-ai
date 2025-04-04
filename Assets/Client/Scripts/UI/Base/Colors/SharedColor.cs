using System;
using UnityEngine;

namespace Client.Scripts.UI.Base.Colors
{
    [Serializable]
    internal struct SharedColor : IThemeColor
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] internal Color Color { get; private set; }
    }
}
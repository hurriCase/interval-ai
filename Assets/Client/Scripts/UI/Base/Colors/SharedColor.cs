using System;
using UnityEngine;

namespace Client.Scripts.UI.Base.Colors
{
    [Serializable]
    internal sealed class SharedColor
    {
        [field: SerializeField] internal string Name { get; private set; }
        [field: SerializeField] internal Color Color { get; private set; }
    }
}
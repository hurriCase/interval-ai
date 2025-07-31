using System;
using UnityEngine;

namespace Source.Scripts.Main.UI.Screens.Generation.Behaviours
{
    [Serializable]
    internal struct MessageData
    {
        [field: SerializeField] internal Sprite BackgroundImage { get; private set; }
        [field: SerializeField] internal DirectionType DirectionType { get; private set; }
    }
}
using System;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Chat.Behaviours
{
    [Serializable]
    internal struct MessageData
    {
        [field: SerializeField] internal Sprite BackgroundImage { get; private set; }
        [field: SerializeField] internal DirectionType DirectionType { get; private set; }
    }
}
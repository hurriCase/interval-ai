using System;
using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Shared
{
    [Serializable]
    internal struct DateRange
    {
        [field: SerializeField] internal DateType DateType { get; private set; }
        [field: SerializeField] internal int Amount { get; private set; }
    }
}
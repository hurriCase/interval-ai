using System;
using UnityEngine;

namespace Source.Scripts.UI.Components.DateLabel
{
    [Serializable]
    internal sealed class DisplayRuleData
    {
        [field: SerializeField] public int DayCount { get; private set; }
        [field: SerializeField] public DisplayType DisplayType { get; private set; }
    }
}
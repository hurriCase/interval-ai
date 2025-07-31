using System.Collections.Generic;
using Source.Scripts.Main.Data.Base;
using Source.Scripts.Main.UI.Shared;
using UnityEngine;

namespace Source.Scripts.Main.Data
{
    internal sealed class ProgressGraphSettings : ScriptableObject, IProgressGraphSettings
    {
        [field: SerializeField] public List<DateRange> GraphProgressRanges { get; private set; }
        [field: SerializeField] public int GraphPointsCount { get; private set; }
    }
}
using System;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    [Serializable]
    internal struct GraphProgressRange
    {
        [field: SerializeField] internal GraphPeriod GraphPeriod { get; private set; }
        [field: SerializeField] internal int Amount { get; private set; }
    }
}
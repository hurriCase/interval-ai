using System;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts.GraphProgress
{
    [Serializable]
    internal struct GraphProgressType
    {
        [field: SerializeField] internal DateType DateType { get; private set; }
        [field: SerializeField] internal int Amount { get; private set; }
    }
}
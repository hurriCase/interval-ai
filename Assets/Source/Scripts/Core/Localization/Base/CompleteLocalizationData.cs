using System;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    [Serializable]
    internal struct CompleteLocalizationData
    {
        [field: SerializeField] internal string ButtonPositive { get; private set; }
        [field: SerializeField] internal string ButtonNegative { get; private set; }
    }
}
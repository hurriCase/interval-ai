using System;
using CustomUtils.Runtime.Localization;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    [Serializable]
    internal struct CompleteLocalizationData
    {
        [field: SerializeField] internal LocalizationKey ButtonPositive { get; private set; }
        [field: SerializeField] internal LocalizationKey ButtonNegative { get; private set; }
    }
}
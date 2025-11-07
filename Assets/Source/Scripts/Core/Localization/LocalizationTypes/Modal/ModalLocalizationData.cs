using System;
using CustomUtils.Runtime.Localization;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LocalizationTypes.Modal
{
    [Serializable]
    internal struct ModalLocalizationData
    {
        [field: SerializeField] internal LocalizationKey TitleKey { get; private set; }
        [field: SerializeField] internal LocalizationKey MessageKey { get; private set; }
        [field: SerializeField] internal LocalizationKey PositiveKey { get; private set; }
        [field: SerializeField] internal LocalizationKey NegativeKey { get; private set; }
    }
}
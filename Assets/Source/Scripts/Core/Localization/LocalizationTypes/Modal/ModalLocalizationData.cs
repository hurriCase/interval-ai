using System;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LocalizationTypes.Modal
{
    [Serializable]
    internal struct ModalLocalizationData
    {
        [field: SerializeField] internal string TitleKey { get; private set; }
        [field: SerializeField] internal string MessageKey { get; private set; }
        [field: SerializeField] internal string PositiveKey { get; private set; }
        [field: SerializeField] internal string NegativeKey { get; private set; }
    }
}
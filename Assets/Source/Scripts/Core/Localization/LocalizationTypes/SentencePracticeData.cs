using System;
using CustomUtils.Runtime.Localization;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LocalizationTypes
{
    [Serializable]
    internal struct SentencePracticeData
    {
        [field: SerializeField] internal LocalizationKey PracticeButtonKey { get; private set; }
        [field: SerializeField] internal LocalizationKey RemarkKey { get; private set; }
    }
}
using System;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LocalizationTypes
{
    [Serializable]
    internal struct SentencePracticeData
    {
        [field: SerializeField] internal string PracticeButtonKey { get; private set; }
        [field: SerializeField] internal string RemarkKey { get; private set; }
    }
}
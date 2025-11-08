using System;
using CustomUtils.Runtime.Localization;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LocalizationTypes.Date
{
    [Serializable]
    internal sealed class PluralLocalization
    {
        [field: SerializeField] internal LocalizationKey SingularLocalizationKey { get; private set; }
        [field: SerializeField] internal LocalizationKey FewLocalizationKey { get; private set; }
        [field: SerializeField] internal LocalizationKey ManyLocalizationKey { get; private set; }
    }
}
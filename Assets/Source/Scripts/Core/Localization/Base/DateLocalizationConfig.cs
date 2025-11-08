using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class DateLocalizationConfig : ScriptableObject
    {
        [field: SerializeField] internal EnumArray<DateType, PluralLocalization> DateLocalizations { get; private set; }
            = new(EnumMode.SkipFirst);
    }
}
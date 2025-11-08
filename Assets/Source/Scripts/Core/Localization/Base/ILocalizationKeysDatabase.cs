using System;
using JetBrains.Annotations;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal interface ILocalizationKeysDatabase
    {
        [MustUseReturnValue]
        string GetDateLocalization(DateType dateType, int count);

        [MustUseReturnValue]
        string GetLearnedCountLocalization(int count);

        [MustUseReturnValue]
        string GetLanguageLocalization(SystemLanguage systemLanguage);

        [MustUseReturnValue]
        string GetLocalizationByValue<TEnum>(TEnum enumValue)
            where TEnum : unmanaged, Enum;
    }
}
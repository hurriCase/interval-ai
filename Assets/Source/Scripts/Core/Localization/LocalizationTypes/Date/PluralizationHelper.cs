using System;
using CustomUtils.Runtime.Localization;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LocalizationTypes.Date
{
    internal static class PluralizationHelper
    {
        internal static LocalizationKey GetPluralForm(
            PluralLocalization pluralLocalization,
            int count,
            SystemLanguage language) =>
            language switch
            {
                // Supported languages with specific rules
                SystemLanguage.Russian => GetRussianPluralForm(pluralLocalization, count),
                SystemLanguage.English => GetOneBasedPluralForm(pluralLocalization, count),
                SystemLanguage.German => GetOneBasedPluralForm(pluralLocalization, count),
                SystemLanguage.Spanish => GetOneBasedPluralForm(pluralLocalization, count),
                SystemLanguage.Portuguese => GetOneBasedPluralForm(pluralLocalization, count),
                SystemLanguage.Italian => GetOneBasedPluralForm(pluralLocalization, count),
                SystemLanguage.French => GetFrenchPluralForm(pluralLocalization, count), // Special: 0,1 → singular

                // No pluralization languages (Asian)
                SystemLanguage.Korean => pluralLocalization.SingularLocalizationKey,
                SystemLanguage.Japanese => pluralLocalization.SingularLocalizationKey,
                SystemLanguage.Chinese => pluralLocalization.SingularLocalizationKey,
                SystemLanguage.Thai => pluralLocalization.SingularLocalizationKey,
                SystemLanguage.Indonesian => pluralLocalization.SingularLocalizationKey,

                _ => GetOneBasedPluralForm(pluralLocalization, count)
            };

        private static LocalizationKey GetOneBasedPluralForm(PluralLocalization pluralLocalization, int count)
            => Math.Abs(count) == 1
                ? pluralLocalization.SingularLocalizationKey
                : pluralLocalization.ManyLocalizationKey;

        private static LocalizationKey GetRussianPluralForm(PluralLocalization pluralLocalization, int count)
        {
            var absCount = Math.Abs(count);
            var lastDigit = absCount % 10;
            var lastTwoDigits = absCount % 100;

            return lastDigit switch
            {
                1 when lastTwoDigits != 11 => pluralLocalization.SingularLocalizationKey,
                >= 2 and <= 4 when lastTwoDigits is < 12 or > 14 => pluralLocalization.FewLocalizationKey,
                _ => pluralLocalization.ManyLocalizationKey
            };
        }

        private static LocalizationKey GetFrenchPluralForm(PluralLocalization pluralLocalization, int count)
            => Math.Abs(count) <= 1
                ? pluralLocalization.SingularLocalizationKey
                : pluralLocalization.ManyLocalizationKey;
    }
}
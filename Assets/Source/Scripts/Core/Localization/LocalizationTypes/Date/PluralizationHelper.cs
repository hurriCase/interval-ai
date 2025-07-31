using System;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LocalizationTypes.Date
{
    internal static class PluralizationHelper
    {
        internal static PluralForm GetPluralForm(int count, SystemLanguage language) =>
            language switch
            {
                // Supported languages with specific rules
                SystemLanguage.Russian => GetRussianPluralForm(count),
                SystemLanguage.English => GetOneBasedPluralForm(count),
                SystemLanguage.German => GetOneBasedPluralForm(count),
                SystemLanguage.Spanish => GetOneBasedPluralForm(count),
                SystemLanguage.Portuguese => GetOneBasedPluralForm(count),
                SystemLanguage.Italian => GetOneBasedPluralForm(count),
                SystemLanguage.French => GetFrenchPluralForm(count), // Special: 0,1 → singular

                // No pluralization languages (Asian)
                SystemLanguage.Korean => PluralForm.Singular,
                SystemLanguage.Japanese => PluralForm.Singular,
                SystemLanguage.Chinese => PluralForm.Singular,
                SystemLanguage.Thai => PluralForm.Singular,
                SystemLanguage.Indonesian => PluralForm.Singular,

                _ => GetOneBasedPluralForm(count)
            };

        private static PluralForm GetOneBasedPluralForm(int count)
            => Math.Abs(count) == 1 ? PluralForm.Singular : PluralForm.Many;

        private static PluralForm GetRussianPluralForm(int count)
        {
            var absCount = Math.Abs(count);
            var lastDigit = absCount % 10;
            var lastTwoDigits = absCount % 100;

            return lastDigit switch
            {
                1 when lastTwoDigits != 11 => PluralForm.Singular,
                >= 2 and <= 4 when lastTwoDigits is < 12 or > 14 => PluralForm.Few,
                _ => PluralForm.Many
            };
        }

        private static PluralForm GetFrenchPluralForm(int count)
            => Math.Abs(count) <= 1 ? PluralForm.Singular : PluralForm.Many;
    }
}
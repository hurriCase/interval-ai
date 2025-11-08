using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class LocalizationKeysDatabase : ScriptableObject, ILocalizationKeysDatabase
    {
        [SerializeField] private EnumArray<PluralForm, LocalizationKey> _learnedCounts = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<SystemLanguage, LocalizationKey> _languages = new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<DateType, EnumArray<PluralForm, LocalizationKey>> _date
            = new(static () => new EnumArray<PluralForm, LocalizationKey>(EnumMode.SkipFirst), EnumMode.SkipFirst);

        public string GetLanguageLocalization(SystemLanguage systemLanguage) =>
            LocalizationController.Localize(_languages[systemLanguage]);

        public string GetDateLocalization(DateType dateType, int count)
        {
            var pluralForm = PluralizationHelper.GetPluralForm(count, LocalizationController.Language.Value);
            return LocalizationController.Localize(_date[dateType][pluralForm]);
        }

        public string GetLearnedCountLocalization(int count)
        {
            var pluralForm = PluralizationHelper.GetPluralForm(count, LocalizationController.Language.Value);
            return LocalizationController.Localize(_learnedCounts[pluralForm]);
        }
    }
}
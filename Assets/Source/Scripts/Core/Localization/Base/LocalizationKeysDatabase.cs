using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.Theme.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class LocalizationKeysDatabase : ScriptableObject, ILocalizationKeysDatabase
    {
        [SerializeField] private EnumArray<LocalizationType, string> _localizationData = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<LearningState, string> _progressLearningStates = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<PluralForm, string> _learnedCounts = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<CategoryType, string> _categoryTypes = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<WordOrderType, string> _wordOrder = new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<DateType, EnumArray<PluralForm, string>> _date
            = new(() => new EnumArray<PluralForm, string>(EnumMode.SkipFirst), EnumMode.SkipFirst);

        [SerializeField] private EnumArray<PracticeState, EnumArray<CompleteType, string>> _learningCompletes =
            new(() => new EnumArray<CompleteType, string>(EnumMode.SkipFirst), EnumMode.SkipFirst);

        [SerializeField] private EnumArray<WordReviewSourceType, string> _wordReviewSourceTypes
            = new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<ThemeType, string> _themeTypes = new(EnumMode.SkipFirst);

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ILocalizationDatabase _localizationDatabase;

        public string GetLocalization(LocalizationType type) =>
            LocalizationController.Localize(_localizationData[type]);

        public string GetLearningStateLocalization(LearningState state) =>
            LocalizationController.Localize(_progressLearningStates[state]);

        public string GetLearningStateLocalization(CategoryType categoryType) =>
            LocalizationController.Localize(_categoryTypes[categoryType]);

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

        public string GetCompletesLocalization(PracticeState practiceState, CompleteType completeType) =>
            LocalizationController.Localize(_learningCompletes[practiceState][completeType]);

        public string GetLocalization(Type enumType, int enumIndex)
        {
            if (enumType == typeof(ThemeType))
                return _themeTypes[enumIndex].GetLocalization();

            if (enumType == typeof(WordReviewSourceType))
                return _wordReviewSourceTypes[enumIndex].GetLocalization();

            if (enumType == typeof(WordOrderType))
                return _wordOrder[enumIndex].GetLocalization();

            if (enumType != typeof(LanguageType))
                return enumType == typeof(SystemLanguage)
                    ? _localizationDatabase.Languages[enumIndex]
                    : enumIndex.ToString().GetLocalization();

            var languageType = _settingsRepository.LanguageByType.Value[enumIndex].Value;
            return _localizationDatabase.Languages[languageType];
        }
    }
}
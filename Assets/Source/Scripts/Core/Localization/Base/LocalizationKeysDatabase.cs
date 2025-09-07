using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.Theme.Base;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class LocalizationKeysDatabase : ScriptableObject, ILocalizationKeysDatabase
    {
        [field: SerializeField]
        public EnumArray<PracticeState, CompleteLocalizationData> LearningCompleteButtons { get; private set; } =
            new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<LocalizationType, string> _localizationData = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<LearningState, string> _progressLearningStates = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<PluralForm, string> _learnedCounts = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<CategoryType, string> _categoryTypes = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<WordOrderType, string> _wordOrder = new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<DateType, EnumArray<PluralForm, string>> _date
            = new(() => new EnumArray<PluralForm, string>(EnumMode.SkipFirst), EnumMode.SkipFirst);

        [SerializeField]
        private EnumArray<PracticeState, EnumArray<CompleteType, string>> _learningCompleteDescriptions =
            new(() => new EnumArray<CompleteType, string>(EnumMode.SkipFirst),
                EnumMode.SkipFirst);

        [SerializeField] private EnumArray<WordReviewSourceType, string> _wordReviewSourceTypes
            = new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<ThemeType, string> _themeTypes = new(EnumMode.SkipFirst);

        private ILanguageSettingsRepository _languageSettingsRepository;
        private ILocalizationDatabase _localizationDatabase;

        [Inject]
        internal void Inject(
            ILanguageSettingsRepository languageSettingsRepository,
            ILocalizationDatabase localizationDatabase)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _localizationDatabase = localizationDatabase;
        }

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

        public string GetCompleteDescriptionLocalization(
            PracticeState practiceState,
            CompleteType completeType) =>
            LocalizationController.Localize(_learningCompleteDescriptions[practiceState][completeType]);

        public string GetLocalizationByValue<TEnum>(TEnum enumValue)
            where TEnum : unmanaged, Enum
        {
            var enumType = typeof(TEnum);
            var enumIndex = UnsafeEnumConverter<TEnum>.ToInt32(enumValue);

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

            var languageType = _languageSettingsRepository.LanguageByType.CurrentValue[enumIndex];
            return _localizationDatabase.Languages[languageType];
        }
    }
}
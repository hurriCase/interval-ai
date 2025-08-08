using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class LocalizationKeysDatabase : ScriptableObject, ILocalizationKeysDatabase
    {
        [SerializeField] private EnumArray<LocalizationType, string> _localizationData = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<LearningState, string> _progressLearningStates = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<PluralForm, string> _learnedCounts = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<CategoryType, string> _categoryTypes = new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<DateType, EnumArray<PluralForm, string>> _date
            = new(() => new EnumArray<PluralForm, string>(EnumMode.SkipFirst), EnumMode.SkipFirst);

        [SerializeField] private EnumArray<PracticeState, EnumArray<CompleteType, string>> _learningCompletes =
            new(() => new EnumArray<CompleteType, string>(EnumMode.SkipFirst), EnumMode.SkipFirst);

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
    }
}
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Localization;
using Source.Scripts.Core.Localization.Date;
using Source.Scripts.Data;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Main.Source.Scripts.Main.Data.Base;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.Data
{
    internal sealed class LocalizationKeysDatabase : ScriptableObject, ILocalizationKeysDatabase
    {
        [SerializeField] private EnumArray<LocalizationType, string> _localizationData = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<LearningState, string> _progressLearningStates = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<DateType, EnumArray<PluralForm, string>> _date = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<PluralForm, string> _learnedCounts = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<PracticeState, EnumArray<CompleteState, string>> _learningCompletes =
            new(EnumMode.SkipFirst);

        public string GetLocalization(LocalizationType type) =>
            LocalizationController.Localize(_localizationData[type]);

        public string GetLearningStateLocalization(LearningState state) =>
            LocalizationController.Localize(_progressLearningStates[state]);

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

        public string GetCompletesLocalization(PracticeState practiceState, CompleteState completeState) =>
            LocalizationController.Localize(_learningCompletes[practiceState][completeState]);
    }
}
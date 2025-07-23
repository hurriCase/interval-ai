using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Data;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization.Date;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Base;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete;
using UnityEngine;

namespace Source.Scripts.UI.Localization
{
    [Resource(ResourcePaths.DatabaseFullPath, nameof(LocalizationKeysDatabase), ResourcePaths.DatabaseResourcePath)]
    internal sealed class LocalizationKeysDatabase : SingletonScriptableObject<LocalizationKeysDatabase>
    {
        [SerializeField] private EnumArray<LocalizationType, string> _localizationData = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<LearningState, string> _learningStatesLocalizationData
            = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<DateType, EnumArray<PluralForm, string>> _dateLocalizationData
            = new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<PluralForm, string> _learnedCountLocalizationData = new(EnumMode.SkipFirst);
        [field: SerializeField]
        internal EnumArray<PracticeState, EnumArray<CompleteState, string>> CompleteLocalizationData
        {
            get;
            private set;
        } = new(EnumMode.SkipFirst);

        internal string GetLocalization(LocalizationType type) => _localizationData[type];

        internal string GetLearningStateLocalization(LearningState state) => _learningStatesLocalizationData[state];

        internal string GetDateLocalization(DateType dateType, int count)
        {
            var pluralForm = PluralizationHelper.GetPluralForm(count, LocalizationController.Language.Value);
            return _dateLocalizationData[dateType][pluralForm];
        }

        internal string GetLearnedCountLocalization(int count)
        {
            var pluralForm = PluralizationHelper.GetPluralForm(count, LocalizationController.Language.Value);
            return _learnedCountLocalizationData[pluralForm];
        }
    }
}
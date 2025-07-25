using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Localization.Date;
using Source.Scripts.Data;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.Core.Localization
{
    [Resource(Data.ResourcePaths.DatabaseFullPath, nameof(LocalizationKeysDatabase), Data.ResourcePaths.DatabaseResourcePath)]
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

        [field: SerializeField]
        internal EnumArray<LanguageLevel, string> LanguageLevelLocalizationData { get; private set; } =
            new(EnumMode.SkipFirst);

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
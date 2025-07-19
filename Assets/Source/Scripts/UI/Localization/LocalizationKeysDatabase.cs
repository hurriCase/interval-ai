using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Data;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization.Date;
using UnityEngine;
using ResourcePaths = Source.Scripts.Core.ResourcePaths;

namespace Source.Scripts.UI.Localization
{
    [Resource(ResourcePaths.ResourcePath, nameof(LocalizationKeysDatabase))]
    internal sealed class LocalizationKeysDatabase : SingletonScriptableObject<LocalizationKeysDatabase>
    {
        [SerializeField] private EnumArray<LocalizationType, string> _localizationData = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<LearningState, string> _learningStatesLocalizationData
            = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<DateType, EnumArray<PluralForm, string>> _dateLocalizationData
            = new(EnumMode.SkipFirst);

        internal string GetLocalization(LocalizationType type) => _localizationData[type];

        internal string GetLearningStateLocalization(LearningState state) => _learningStatesLocalizationData[state];

        internal string GetDateLocalization(DateType dateType, int count)
        {
            var pluralForm = PluralizationHelper.GetPluralForm(count, LocalizationController.Language.Value);
            return _dateLocalizationData[dateType][pluralForm];
        }
    }
}
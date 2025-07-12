using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Core;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts.GraphProgress;
using UnityEngine;

namespace Source.Scripts.UI.Localization
{
    [Resource(ResourcePaths.ResourcePath, nameof(LocalizationKeysDatabase))]
    internal sealed class LocalizationKeysDatabase : SingletonScriptableObject<LocalizationKeysDatabase>
    {
        [SerializeField] private EnumArray<LocalizationType, string> _localizationData;
        [SerializeField] private EnumArray<LearningState, string> _learningStatesLocalizationData;
        [SerializeField] private EnumArray<DateType, string> _dateLocalizationData;

        internal string GetLocalization(LocalizationType type) => _localizationData[type];
        internal string GetLearningStateLocalization(LearningState state) => _learningStatesLocalizationData[state];
        internal string GetDateLocalization(DateType type) => _dateLocalizationData[type];
    }
}
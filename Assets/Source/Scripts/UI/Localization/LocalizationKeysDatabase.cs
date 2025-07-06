using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
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
        [SerializeField] private List<LocalizationData<LocalizationType>> _localizationData;
        [SerializeField] private LocalizationData<LearningState>[] _learningStatesLocalizationData = new LocalizationData<LearningState>[5];
        [SerializeField] private LocalizationData<DateType>[] _dateLocalizationData = new LocalizationData<DateType>[3];

        internal string GetLocalization(LocalizationType type)
        {
            foreach (var localizationData in _localizationData)
            {
                if (localizationData.Type == type)
                    return localizationData.Key;
            }

            Debug.LogError($"[LocalizationKeysDatabase::GetLocalization] No localization for type {type}");
            return string.Empty;
        }

        internal string GetLearningStateLocalization(int index) => _learningStatesLocalizationData[index].Key;
        internal string GetDateLocalization(int index) => _dateLocalizationData[index].Key;
    }
}
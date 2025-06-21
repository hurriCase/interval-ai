using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core;
using UnityEngine;

namespace Source.Scripts.UI.Localization
{
    [Resource(ResourcePaths.ResourcePath, nameof(LocalizationKeysDatabase))]
    internal sealed class LocalizationKeysDatabase : SingletonScriptableObject<LocalizationKeysDatabase>
    {
        [SerializeField] private List<LocalizationData> _localizationData;

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
    }

    internal static class LocalizationTypeExtensions
    {
        internal static string GetLocalization(this LocalizationType type)
            => LocalizationController.Localize(LocalizationKeysDatabase.Instance.GetLocalization(type));
    }
}
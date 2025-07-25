using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.Core.Localization
{
    [Resource(Data.ResourcePaths.DatabaseFullPath, nameof(LanguagesNameDatabase), Data.ResourcePaths.DatabaseResourcePath)]
    internal sealed class LanguagesNameDatabase : SingletonScriptableObject<LanguagesNameDatabase>
    {
        [field: SerializeField] internal EnumArray<Language, string> LanguageLocalizationData { get; private set; } =
            new(EnumMode.SkipFirst);
    }
}
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.DI.Repositories.Settings.Base;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class LocalizationDatabase : ScriptableObject, ILocalizationDatabase
    {
        [field: SerializeField] public EnumArray<Language, string> Languages { get; private set; } =
            new(EnumMode.SkipFirst);

        [field: SerializeField] public EnumArray<LanguageLevel, string> LanguageLevelKeys { get; private set; } =
            new(EnumMode.SkipFirst);
    }
}
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class LocalizationDatabase : ScriptableObject, ILocalizationDatabase
    {
        [field: SerializeField] public EnumArray<SystemLanguage, string> Languages { get; private set; }

        [field: SerializeField] public EnumArray<LanguageLevel, string> LanguageLevelKeys { get; private set; } =
            new(EnumMode.SkipFirst);
    }
}
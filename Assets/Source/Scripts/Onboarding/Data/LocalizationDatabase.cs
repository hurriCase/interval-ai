using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.Words.Base;
using Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data.Base;
using UnityEngine;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data
{
    internal sealed class LocalizationDatabase : ScriptableObject, ILocalizationDatabase
    {
        [field: SerializeField] public EnumArray<Language, string> Languages { get; private set; } =
            new(EnumMode.SkipFirst);

        [field: SerializeField] public EnumArray<LanguageLevel, string> LanguageLevelKeys { get; private set; } =
            new(EnumMode.SkipFirst);
    }
}
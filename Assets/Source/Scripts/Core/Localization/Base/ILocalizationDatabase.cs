using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.LocalizationTypes.Modal;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal interface ILocalizationDatabase
    {
        EnumArray<SystemLanguage, string> Languages { get; }
        EnumArray<LanguageLevel, LocalizationKey> LanguageLevelKeys { get; }
        EnumArray<ModalLocalizationType, ModalLocalizationData> ModalLocalizations { get; }
        EnumArray<SentencePracticeState, SentencePracticeData> SentencePractices { get; }
        string GetLanguageName(SystemLanguage language);
    }
}
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Localization.LocalizationTypes.Modal;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal interface ILocalizationDatabase
    {
        EnumArray<SystemLanguage, string> Languages { get; }
        EnumArray<LanguageLevel, string> LanguageLevelKeys { get; }
        EnumArray<ModalLocalizationType, ModalLocalizationData> ModalLocalizations { get; }
        string GetLanguageName(SystemLanguage language);
    }
}
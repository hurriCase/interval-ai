using CustomUtils.Runtime.CustomTypes.Collections;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal interface ILocalizationDatabase
    {
        EnumArray<SystemLanguage, string> Languages { get; }
        string GetLanguageName(SystemLanguage language);
    }
}
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.DI.Repositories.Settings.Base;
using Source.Scripts.Core.DI.Repositories.Words.Base;

namespace Source.Scripts.Core.Localization.Base
{
    internal interface ILocalizationDatabase
    {
        EnumArray<Language, string> Languages { get; }
        EnumArray<LanguageLevel, string> LanguageLevelKeys { get; }
    }
}
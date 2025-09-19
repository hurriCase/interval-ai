using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Localization.LocalizationTypes.Modal;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class LocalizationDatabase : ScriptableObject, ILocalizationDatabase
    {
        [field: SerializeField] public EnumArray<SystemLanguage, string> Languages { get; private set; }

        [field: SerializeField] public EnumArray<LanguageLevel, string> LanguageLevelKeys { get; private set; } =
            new(EnumMode.SkipFirst);

        [field: SerializeField]
        public EnumArray<ModalLocalizationType, ModalLocalizationData> ModalLocalizations { get; private set; } =
            new(EnumMode.SkipFirst);

        public string GetLanguageName(SystemLanguage language)
        {
            // ReSharper disable once HeapView.BoxingAllocation . It's fine, because this is done for safety reason
            var localization = Languages[language].IsValid() is false
                ? language.ToString()
                : Languages[language];

            return localization;
        }
    }
}
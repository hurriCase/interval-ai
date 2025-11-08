using CustomUtils.Runtime.CustomTypes.Collections;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class LocalizationDatabase : ScriptableObject, ILocalizationDatabase
    {
        [field: SerializeField] public EnumArray<SystemLanguage, string> Languages { get; private set; }

        public string GetLanguageName(SystemLanguage language)
        {
            // ReSharper disable once HeapView.BoxingAllocation . It's fine, because this is done for safety reason
            var localization = string.IsNullOrEmpty(Languages[language])
                ? language.ToString()
                : Languages[language];

            return localization;
        }
    }
}
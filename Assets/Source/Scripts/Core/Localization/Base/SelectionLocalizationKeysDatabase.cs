using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.Theme.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Core.Localization.Base
{
    internal sealed class SelectionLocalizationKeysDatabase : ScriptableObject, ISelectionLocalizationKeysDatabase
    {
        [SerializeField] private EnumArray<WordReviewSourceType, string> _wordReviewSourceTypes
            = new(EnumMode.SkipFirst);
        [SerializeField] private EnumArray<ThemeType, string> _thereTypes = new(EnumMode.SkipFirst);

        [Inject] private ILocalizationDatabase _localizationDatabase;

        public string GetLocalization<TEnum>(TEnum enumValue)
        {
            var localizationKey = enumValue switch
            {
                ThemeType theme => _thereTypes[theme],
                SystemLanguage systemLanguage => _localizationDatabase.Languages[systemLanguage],
                WordReviewSourceType wordReviewSourceType => _wordReviewSourceTypes[wordReviewSourceType],
                _ => enumValue.ToString()
            };

            return localizationKey.GetLocalization();
        }
    }
}
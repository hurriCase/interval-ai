using CustomUtils.Unsafe;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [CreateAssetMenu(
        fileName = nameof(LanguageTypeLocalization),
        menuName = LocalizationsPath + nameof(LanguageTypeLocalization)
    )]
    internal sealed class LanguageTypeLocalization : EnumLocalizationDataBase
    {
        [SerializeField] private LanguageKeyLocalizationConfig _localizationConfig;

        private ILanguageSettingsRepository _languageSettingsRepository;

        [Inject]
        internal void Inject(ILanguageSettingsRepository languageSettingsRepository)
        {
            _languageSettingsRepository = languageSettingsRepository;
        }

        internal override string GetLocalization<TEnumParameter>(TEnumParameter currentEnum)
        {
            var enumValue = UnsafeEnumConverter<TEnumParameter>.ToInt32(currentEnum);
            var systemLanguage = _languageSettingsRepository.LanguageByType.CurrentValue[enumValue];
            return _localizationConfig.GetLocalization(systemLanguage);
        }
    }
}
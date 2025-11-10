using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Unsafe;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [CreateAssetMenu(
        fileName = nameof(LanguageLocalizationConfig),
        menuName = LocalizationsPath + nameof(LanguageLocalizationConfig)
    )]
    internal sealed class LanguageLocalizationConfig : EnumLocalizationDataBase
    {
        [SerializeField] private EnumArray<SystemLanguage, string> _localizations;

        internal override string GetLocalization<TEnumParameter>(TEnumParameter currentEnum)
        {
            var enumValue = UnsafeEnumConverter<TEnumParameter>.ToInt32(currentEnum);
            return _localizations[enumValue];
        }
    }
}
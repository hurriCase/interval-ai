using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [CreateAssetMenu(
        fileName = nameof(LanguageKeyLocalizationConfig),
        menuName = LocalizationsPath + nameof(LanguageKeyLocalizationConfig)
    )]
    internal sealed class LanguageKeyLocalizationConfig : GenericEnumLocalizationConfig<SystemLanguage> { }
}
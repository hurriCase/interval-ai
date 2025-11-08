using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [CreateAssetMenu(
        fileName = nameof(LanguageLocalizationConfig),
        menuName = LocalizationsPath + nameof(LanguageLocalizationConfig)
    )]
    internal class LanguageLocalizationConfig : GenericEnumLocalizationConfig<SystemLanguage> { }
}
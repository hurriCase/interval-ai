using CustomUtils.Runtime.UI.Theme;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [CreateAssetMenu(
        fileName = nameof(ThemeLocalizationConfig),
        menuName = LocalizationsPath + nameof(ThemeLocalizationConfig)
    )]
    internal sealed class ThemeLocalizationConfig : GenericEnumLocalizationConfig<ThemeType> { }
}
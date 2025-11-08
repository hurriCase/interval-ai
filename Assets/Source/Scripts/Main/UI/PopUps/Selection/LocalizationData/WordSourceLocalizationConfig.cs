using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [CreateAssetMenu(
        fileName = nameof(WordSourceLocalizationConfig),
        menuName = LocalizationsPath + nameof(WordSourceLocalizationConfig)
    )]
    internal sealed class WordSourceLocalizationConfig : GenericEnumLocalizationConfig<WordReviewSourceType> { }
}
using Source.Scripts.Core.Repositories.Categories.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    [CreateAssetMenu(
        fileName = nameof(WordOrderLocalizationConfig),
        menuName = LocalizationsPath + nameof(WordOrderLocalizationConfig)
    )]
    internal sealed class WordOrderLocalizationConfig : GenericEnumLocalizationConfig<WordOrderType> { }
}
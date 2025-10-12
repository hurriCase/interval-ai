using Source.Scripts.Core.Localization.Translator.Translations;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal class TranslatableInfoItem : AdditionalInfoItemBase<Translation>
    {
        [SerializeField] private TextMeshProUGUI _learningText;
        [SerializeField] private TextMeshProUGUI _nativeText;

        internal override void UpdateView(Translation translationSet)
        {
            _learningText.text = translationSet.Learning;
            _nativeText.text = translationSet.Native;
        }
    }
}
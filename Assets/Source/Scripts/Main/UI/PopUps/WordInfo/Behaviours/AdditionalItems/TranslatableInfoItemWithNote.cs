using Source.Scripts.Core.Localization.Translator.Translations;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal sealed class TranslatableInfoItemWithNote : AdditionalInfoItemBase<AnnotatedTranslation>
    {
        [SerializeField] private TextMeshProUGUI _learningText;
        [SerializeField] private TextMeshProUGUI _nativeText;
        [SerializeField] private TextMeshProUGUI _noteText;

        internal override void UpdateView(AnnotatedTranslation annotatedTranslation)
        {
            _noteText.text = annotatedTranslation.Note;

            _learningText.text = annotatedTranslation.Translation.Learning;
            _nativeText.text = annotatedTranslation.Translation.Native;
        }
    }
}
using Source.Scripts.Core.Repositories.Words.Word;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal sealed class TranslatableInfoItemWithNote : TranslatableInfoItem, IAdditionalInfoItemBase<AnnotatedTranslation>
    {
        [SerializeField] private TextMeshProUGUI _noteText;

        public void UpdateView(AnnotatedTranslation annotatedTranslation)
        {
            base.UpdateView(annotatedTranslation.Translation);

            _noteText.text = annotatedTranslation.Note;
        }
    }
}
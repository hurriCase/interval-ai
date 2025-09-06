using Source.Scripts.Core.Repositories.Words.Word;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal sealed class TranslatableInfoItemWithNote : TranslatableInfoItem, IAdditionalInfoItemBase<AnnotatedTranslation>
    {
        [SerializeField] private TextMeshProUGUI _noteText;

        public void Init(AnnotatedTranslation annotatedTranslation)
        {
            base.Init(annotatedTranslation.Translation);

            _noteText.text = annotatedTranslation.Note;
        }
    }
}
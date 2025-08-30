using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.UI.Components.Accordion;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal class TranslatableInfoItem : AccordionItem, IAdditionalInfoItemBase<Translation>
    {
        [SerializeField] private AccordionComponent _accordionComponent;
        [SerializeField] private TextMeshProUGUI _learningText;
        [SerializeField] private TextMeshProUGUI _nativeText;
        [SerializeField] private ButtonComponent _audioButton;
        [SerializeField] private ButtonComponent _translateButton;
        [SerializeField] private ButtonComponent _pinButton;

        public void Init(Translation translationSet)
        {
            _accordionComponent.Init();

            _learningText.text = translationSet.Learning;
            _nativeText.text = translationSet.Native;
        }
    }
}
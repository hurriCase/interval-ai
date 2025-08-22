using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules
{
    internal sealed class AnswerModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private TextMeshProUGUI _hiddenWord;
        [SerializeField] private TextMeshProUGUI _shownExampleText;
        [SerializeField] private TextMeshProUGUI _hiddenExampleText;
        [SerializeField] private AccordionComponent _accordionComponent;

        internal override void Init(CardBehaviour cardBehaviour)
        {
            base.Init(cardBehaviour);

            _accordionComponent.Init();
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            _hiddenWord.text = currentWord.GetHiddenWord(practiceSettingsRepository);
            _shownExampleText.text = currentWord.GetShownExample(practiceSettingsRepository);
            _hiddenExampleText.text = currentWord.GetHiddenExample(practiceSettingsRepository);
        }
    }
}
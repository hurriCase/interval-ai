using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules
{
    internal sealed class InputModuleBehaviour : TransitionPracticeModuleBase<ButtonComponent>
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _hintButton;

        private int _shownSymbolCount;

        public override void Init(CardBehaviour cardBehaviour)
        {
            base.Init(cardBehaviour);

            _hintButton.OnClickAsObservable().SubscribeAndRegister(this, self => self.AddHintCharacter());
        }

        private void AddHintCharacter()
        {
            var hiddenWord = currentWord.Word.GetHiddenText(practiceSettingsRepository);
            if (_shownSymbolCount >= hiddenWord.Length)
                return;

            _inputField.text += hiddenWord[_shownSymbolCount];
            _shownSymbolCount++;
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            _inputField.text = string.Empty;
            _shownSymbolCount = 0;
        }
    }
}
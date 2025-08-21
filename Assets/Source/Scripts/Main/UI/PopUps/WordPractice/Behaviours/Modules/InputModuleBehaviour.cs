using R3;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules
{
    internal sealed class InputModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _hintButton;

        private int _shownSymbolCount;

        internal override void Init(CardBehaviour cardBehaviour)
        {
            base.Init(cardBehaviour);

            _hintButton.OnClickAsObservable().SubscribeAndRegister(this, self => self.AddHintCharacter());
        }

        private void AddHintCharacter()
        {
            var hiddenWord = currentWord.GetHiddenWord(settingsRepository);
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
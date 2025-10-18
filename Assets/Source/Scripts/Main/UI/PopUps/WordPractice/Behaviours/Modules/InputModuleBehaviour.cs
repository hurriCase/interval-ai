using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.Translator.Translations;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules
{
    internal sealed class InputModuleBehaviour : TransitionPracticeModuleBase<ThemeButton>
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _hintButton;

        private int _shownSymbolCount;

        public override void Init(PracticeState practiceState)
        {
            base.Init(practiceState);

            _hintButton.OnClickAsObservable().SubscribeUntilDestroy(this, self => self.AddHintCharacter());
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
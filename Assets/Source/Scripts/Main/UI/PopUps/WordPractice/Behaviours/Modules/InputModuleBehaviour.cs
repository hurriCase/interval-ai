using R3;
using Source.Scripts.Data.Repositories.Categories;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules
{
    internal sealed class InputModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _hintButton;

        private int _shownSymbolCount;

        internal override void Init()
        {
            base.Init();

            _hintButton.OnClickAsObservable()
                .Subscribe(this, (_, behaviour) =>
                {
                    var hiddenWord = behaviour.currentWord.GetHiddenWord(settingsRepository);
                    if (behaviour._shownSymbolCount >= hiddenWord.Length)
                        return;

                    behaviour._inputField.text += hiddenWord[behaviour._shownSymbolCount];
                    behaviour._shownSymbolCount++;
                })
                .RegisterTo(destroyCancellationToken);
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            _inputField.text = string.Empty;
            _shownSymbolCount = 0;
        }
    }
}
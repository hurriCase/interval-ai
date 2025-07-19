using System;
using R3;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules
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
                    if (behaviour._shownSymbolCount >= behaviour.currentWord.HiddenWord.Length)
                        return;

                    behaviour._inputField.text += behaviour.currentWord.HiddenWord[behaviour._shownSymbolCount];
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
using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    internal sealed class InputFieldComponent : TMP_InputField
    {
        [field: SerializeField] internal ButtonComponent EditButton { get; private set; }

        public Observable<string> OnTextChanged => _textChanged;
        private readonly Subject<string> _textChanged = new();

        private bool _editWasPressed;

        protected override void Start()
        {
            base.Start();

            if (EditButton)
                EditButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.SwitchEditingState());

            onEndEdit.AsObservable().SubscribeUntilDestroy(this, static (text, self) => self.FinishEditing(text));
        }

        private void SwitchEditingState()
        {
            if (_editWasPressed)
            {
                _editWasPressed = false;
                return;
            }

            _editWasPressed = true;
            readOnly = false;
            ActivateInputField();
        }

        private void FinishEditing(string newText)
        {
            readOnly = true;

            _textChanged.OnNext(newText);
        }
    }
}
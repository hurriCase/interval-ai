using System.Threading;
using CustomUtils.Runtime.Storage;
using R3;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    internal sealed class InputFieldComponent : TMP_InputField
    {
        [field: SerializeField] internal ButtonComponent EditButton { get; private set; }

        public Observable<string> CurrentTextSubjectObservable => _currentTextSubjectObservable;
        private readonly Subject<string> _currentTextSubjectObservable = new();

        private bool _editWasPressed;

        protected override void Start()
        {
            base.Start();

            if (EditButton)
                EditButton.OnClickAsObservable()
                    .Subscribe(this, static (_, screen) => screen.SwitchEditingState())
                    .RegisterTo(destroyCancellationToken);

            onEndEdit.AddListener(FinishEditing);
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
            if (string.IsNullOrEmpty(newText) is false)
                _currentTextSubjectObservable.OnNext(newText);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            onEndEdit.RemoveListener(FinishEditing);
        }
    }
}
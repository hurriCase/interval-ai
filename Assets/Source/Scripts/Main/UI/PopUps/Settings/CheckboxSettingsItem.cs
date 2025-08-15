using CustomUtils.Runtime.Storage;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Components;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class CheckboxSettingsItem : MonoBehaviour
    {
        [SerializeField] private CheckboxComponent _checkbox;
        [SerializeField] private ButtonComponent _button;

        private PersistentReactiveProperty<bool> _targetProperty;

        internal void Init(PersistentReactiveProperty<bool> targetProperty)
        {
            _targetProperty = targetProperty;

            _checkbox.OnValueChangedAsObservable()
                .Subscribe(this, static (isOn, self) => self._targetProperty.Value = isOn)
                .RegisterTo(destroyCancellationToken);

            _button.OnPointerClickAsObservable()
                .Subscribe(this, static (_, self) => self.ToggleProperty())
                .RegisterTo(destroyCancellationToken);

            _targetProperty
                .Subscribe(this, static (isSend, self) => self._checkbox.isOn = isSend)
                .RegisterTo(destroyCancellationToken);
        }

        private void ToggleProperty()
        {
            _targetProperty.Value = _targetProperty.Value is false;
        }
    }
}
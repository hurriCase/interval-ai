using CustomUtils.Runtime.Storage;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Others;
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
                .SubscribeAndRegister(this, static (isOn, self) => self._targetProperty.Value = isOn);

            _button.OnPointerClickAsObservable().SubscribeAndRegister(this,
                static self => self._targetProperty.Value = self._targetProperty.Value is false);

            _targetProperty.SubscribeAndRegister(this, static (isOn, self) => self._checkbox.isOn = isOn);
        }
    }
}
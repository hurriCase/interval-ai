using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Storage;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Components.Button;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class CheckboxSettingsItem : MonoBehaviour
    {
        [SerializeField] private ToggleComponent _checkbox;
        [SerializeField] private ButtonComponent _button;

        internal void Init(PersistentReactiveProperty<bool> targetProperty)
        {
            _checkbox.isOn = targetProperty.Value;

            _checkbox.OnValueChangedAsObservable()
                .Subscribe(targetProperty, static (isOn, targetProperty) => targetProperty.Value = isOn)
                .RegisterTo(destroyCancellationToken);

            _button.OnPointerClickAsObservable()
                .Subscribe(targetProperty, static (_, targetProperty) =>
                    targetProperty.Value = targetProperty.Value is false)
                .RegisterTo(destroyCancellationToken);

            targetProperty.SubscribeAndRegister(this, static (isOn, self) => self._checkbox.isOn = isOn);
        }
    }
}
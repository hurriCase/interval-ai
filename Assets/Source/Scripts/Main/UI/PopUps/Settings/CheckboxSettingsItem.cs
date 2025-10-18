using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Storage;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class CheckboxSettingsItem : MonoBehaviour
    {
        [SerializeField] private StateToggle _checkbox;
        [SerializeField] private ThemeButton _button;

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

            targetProperty.SubscribeUntilDestroy(this, static (isOn, self) => self._checkbox.isOn = isOn);
        }
    }
}
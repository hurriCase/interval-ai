using R3;
using R3.Triggers;
using Source.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Selectables
{
    internal sealed class ToggleComponent : Toggle
    {
        [field: SerializeField] internal SelectableColorMapping SelectableColorMapping { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            this.OnPointerClickAsObservable()
                .Subscribe(static _ => AudioHandler.Instance.PlayOneShotSound(SoundType.Button))
                .RegisterTo(destroyCancellationToken);

            this.OnValueChangedAsObservable()
                .Subscribe(this, static (isOn, component) => component
                    .DoStateTransition(isOn ? SelectionState.Selected : component.currentSelectionState, false))
                .RegisterTo(destroyCancellationToken);

            ApplyTheme();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (isOn && state != SelectionState.Disabled)
                base.DoStateTransition(SelectionState.Selected, instant);
            else
                base.DoStateTransition(state, instant);
        }

        private void ApplyTheme()
        {
            if (!SelectableColorMapping || transition != Transition.ColorTint)
                return;

            colors = SelectableColorMapping.GetThemeBlockColors(colors);
        }
    }
}
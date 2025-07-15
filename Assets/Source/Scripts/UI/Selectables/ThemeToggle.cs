using R3;
using R3.Triggers;
using Source.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Selectables
{
    internal sealed class ThemeToggle : Toggle
    {
        [field: SerializeField] internal SelectableColorMapping SelectableColorMapping { get; private set; }
        [field: SerializeField] internal ThemeGraphicMapping[] AdditionalGraphics { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            this.OnPointerClickAsObservable()
                .Subscribe(static _ => AudioHandler.Instance.PlayOneShotSound(SoundType.Button))
                .RegisterTo(destroyCancellationToken);

            this.OnValueChangedAsObservable()
                .Subscribe(this, static (isOn, toggle) => toggle
                    .DoStateTransition(isOn ? SelectionState.Selected : toggle.currentSelectionState, false))
                .RegisterTo(destroyCancellationToken);

            ApplyTheme();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (isOn && state != SelectionState.Disabled)
            {
                base.DoStateTransition(SelectionState.Selected, instant);

                ApplyAdditionalGraphics(SelectionState.Selected);
            }
            else
            {
                base.DoStateTransition(state, instant);

                ApplyAdditionalGraphics(state);
            }
        }

        private void ApplyAdditionalGraphics(SelectionState state)
        {
            var mappedState = MapSelectionStateToSelectableState(state);

            foreach (var graphicMapping in AdditionalGraphics)
                graphicMapping.ApplyColor(mappedState);
        }

        private void ApplyTheme()
        {
            if (!SelectableColorMapping || transition != Transition.ColorTint)
                return;

            colors = SelectableColorMapping.GetThemeBlockColors(colors);
        }

        private SelectableStateType MapSelectionStateToSelectableState(SelectionState state) =>
            state switch
            {
                SelectionState.Normal => SelectableStateType.Normal,
                SelectionState.Highlighted => SelectableStateType.Highlighted,
                SelectionState.Pressed => SelectableStateType.Pressed,
                SelectionState.Selected => SelectableStateType.Selected,
                SelectionState.Disabled => SelectableStateType.Disabled,
                _ => SelectableStateType.Normal
            };
    }
}
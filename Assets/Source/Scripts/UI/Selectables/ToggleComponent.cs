using R3;
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

            this.OnValueChangedAsObservable()
                .Subscribe(this, static (isOn, component) =>
                {
                    AudioHandler.Instance.PlayOneShotSound(SoundType.Button);
                    component.DoStateTransition(isOn ? SelectionState.Selected : SelectionState.Normal,
                        Application.isPlaying is false);
                })
                .RegisterTo(destroyCancellationToken);

            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (!SelectableColorMapping || transition != Transition.ColorTint)
                return;

            colors = SelectableColorMapping.GetThemeBlockColors(colors);
        }
    }
}
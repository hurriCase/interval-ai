using R3;
using Source.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Selectables
{
    internal sealed class ButtonComponent : Button
    {
        [field: SerializeField] internal SelectableColorMapping SelectableColorMapping { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            this.OnClickAsObservable()
                .Subscribe(static _ => AudioHandler.Instance.PlayOneShotSound(SoundType.Button))
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
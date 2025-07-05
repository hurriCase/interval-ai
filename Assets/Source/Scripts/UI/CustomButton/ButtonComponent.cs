using R3;
using Source.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.CustomButton
{
    [RequireComponent(typeof(Button))]
    internal sealed class ButtonComponent : Button
    {
        [field: SerializeField] internal ButtonStateColorMapping ButtonStateColorMapping { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            this.OnClickAsObservable()
                .Subscribe(static _ => AudioHandler.Instance.PlayOneShotSound(SoundType.Button))
                .AddTo(this);

            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (!ButtonStateColorMapping || transition != Transition.ColorTint)
                return;

            var colorBlock = colors;

            colorBlock.normalColor = ButtonStateColorMapping.GetColorForState(ButtonStateType.Normal);
            colorBlock.pressedColor = ButtonStateColorMapping.GetColorForState(ButtonStateType.Pressed);
            colorBlock.disabledColor = ButtonStateColorMapping.GetColorForState(ButtonStateType.Disabled);

            colors = colorBlock;
        }
    }
}
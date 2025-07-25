using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Audio;

namespace Source.Scripts.UI.Components
{
    internal sealed class CheckboxComponent : SwitchableToggle
    {
        protected override void Awake()
        {
            base.Awake();

            this.OnPointerClickAsObservable()
                .Subscribe(static _ => AudioHandler.Instance.PlayOneShotSound(SoundType.Button))
                .RegisterTo(destroyCancellationToken);
        }
    }
}
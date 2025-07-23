using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Audio;

namespace Source.Scripts.UI.Selectables
{
    internal sealed class TabComponent : ThemeToggle
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
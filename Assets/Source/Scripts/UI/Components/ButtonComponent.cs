using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using R3;
using Source.Scripts.Core.Audio;

namespace Source.Scripts.UI.Components
{
    internal sealed class ButtonComponent : ThemeButton
    {
        protected override void Awake()
        {
            base.Awake();

            this.OnClickAsObservable()
                .Subscribe(static _ => AudioHandler.Instance.PlayOneShotSound(SoundType.Button))
                .RegisterTo(destroyCancellationToken);
        }
    }
}
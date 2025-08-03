using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using R3;
using Source.Scripts.Core.Audio;
using UnityEngine.Device;
using VContainer;

namespace Source.Scripts.UI.Components
{
    internal sealed class ButtonComponent : ThemeButton
    {
        [Inject] private IAudioHandlerProvider _audioHandlerProvider;

        protected override void Start()
        {
            base.Start();

            if (Application.isEditor)
                return;

            this.OnClickAsObservable()
                .Subscribe(_audioHandlerProvider.AudioHandler,
                    static (_, handler) => handler.PlayOneShotSound(SoundType.Button))
                .RegisterTo(destroyCancellationToken);
        }
    }
}
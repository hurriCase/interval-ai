using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Audio;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Components
{
    internal sealed class TabComponent : ThemeToggle
    {
        [Inject] private IAudioHandlerProvider _audioHandlerProvider;

        protected override void Start()
        {
            base.Start();

            if (Application.isEditor)
                return;

            this.OnPointerClickAsObservable()
                .Subscribe(_audioHandlerProvider.AudioHandler,
                    static (_, handler) => handler.PlayOneShotSound(SoundType.Button))
                .RegisterTo(destroyCancellationToken);
        }
    }
}
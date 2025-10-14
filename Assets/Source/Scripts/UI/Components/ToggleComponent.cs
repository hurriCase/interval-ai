using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using R3.Triggers;
using Source.Scripts.Core.Audio.Sounds.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Components
{
    internal class ToggleComponent : StateToggle
    {
        private IAudioHandlerProvider _audioHandlerProvider;

        [Inject]
        internal void Inject(IAudioHandlerProvider audioHandlerProvider)
        {
            _audioHandlerProvider = audioHandlerProvider;
        }

        protected override void Start()
        {
            base.Start();

            if (Application.isPlaying is false)
                return;

            this.OnPointerClickAsObservable().SubscribeUntilDestroy(this,
                static self => self._audioHandlerProvider.AudioHandler.PlayOneShotSound(SoundType.Button));
        }
    }
}
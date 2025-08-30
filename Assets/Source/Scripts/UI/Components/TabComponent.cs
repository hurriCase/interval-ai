using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using R3.Triggers;
using Source.Scripts.Core.Audio.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Components
{
    internal sealed class TabComponent : ThemeToggle
    {
        private IAudioHandlerProvider _audioHandlerProvider;

        [Inject]
        public void Inject(IAudioHandlerProvider audioHandlerProvider)
        {
            _audioHandlerProvider = audioHandlerProvider;
        }

        protected override void Start()
        {
            base.Start();

            if (Application.isEditor)
                return;

            this.OnPointerClickAsObservable().SubscribeAndRegister(this,
                static self => self._audioHandlerProvider.AudioHandler.PlayOneShotSound(SoundType.Button));
        }
    }
}
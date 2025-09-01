using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Audio.Base;
using UnityEngine.Device;
using VContainer;

namespace Source.Scripts.UI.Components.Button
{
    internal sealed class ButtonComponent : ThemeButton
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

            this.OnClickAsObservable().SubscribeAndRegister(this,
                static self => self._audioHandlerProvider.AudioHandler.PlayOneShotSound(SoundType.Button));
        }
    }
}
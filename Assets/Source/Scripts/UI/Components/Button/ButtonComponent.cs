using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Audio.Sounds.Base;
using UnityEngine.Device;
using VContainer;

namespace Source.Scripts.UI.Components.Button
{
    internal sealed class ButtonComponent : ThemeButton
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

            if (Application.isEditor)
                return;

            this.OnClickAsObservable().SubscribeUntilDestroy(this,
                static self => self._audioHandlerProvider.AudioHandler.PlayOneShotSound(SoundType.Button));
        }
    }
}
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using R3;
using Source.Scripts.Core.Audio.Base;
using UnityEngine.Device;
using VContainer;

namespace Source.Scripts.UI.Components.Button
{
    internal sealed class ButtonComponent : ThemeButton
    {
        [Inject] private IAudioHandlerProvider _audioHandlerProvider;

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
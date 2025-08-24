using CustomUtils.Runtime.UI.CustomComponents.Selectables;
using R3.Triggers;
using Source.Scripts.Core.Audio.Base;
using Source.Scripts.Core.Others;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Components
{
    internal sealed class CheckboxComponent : SwitchableToggle
    {
        [Inject] private IAudioHandlerProvider _audioHandlerProvider;

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
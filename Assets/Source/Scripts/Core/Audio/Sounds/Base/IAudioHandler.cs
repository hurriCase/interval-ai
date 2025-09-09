using UnityEngine;

namespace Source.Scripts.Core.Audio.Sounds.Base
{
    internal interface IAudioHandler
    {
        void PlayOneShotSound(SoundType soundType, float volumeModifier = 1, float pitchModifier = 1);
        AudioSource PlayClip(AudioClip soundType, float volumeModifier = 1, float pitchModifier = 1);
    }
}
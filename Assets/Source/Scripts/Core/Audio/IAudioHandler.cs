using CustomUtils.Runtime.Audio;
using UnityEngine;

namespace Source.Scripts.Core.Audio
{
    internal interface IAudioHandler
    {
        void Init();
        AudioSource PlaySound(SoundType soundType, float volumeModifier, float pitchModifier);
        void StopSound(SoundType soundType);
        void PlayOneShotSound(SoundType soundType, float volumeModifier, float pitchModifier);
        AudioSource PlayMusic(MusicType musicType);
        AudioSource PlayMusic(AudioData data);
    }
}
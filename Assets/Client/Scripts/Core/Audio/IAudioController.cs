using UnityEngine;

namespace Client.Scripts.Core.Audio
{
    internal interface IAudioController
    {
        void PlayMusic(float volume = 1f);
        void PlayEffect(AudioClip clip, float volume = 1f);
    }
}
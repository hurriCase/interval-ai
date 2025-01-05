using UnityEngine;

namespace Client.Scripts.Patterns.DI.Services
{
    internal interface IAudioController
    {
        void PlayMusic(float volume = 1f);
        void PlayEffect(AudioClip clip, float volume = 1f);
    }
}
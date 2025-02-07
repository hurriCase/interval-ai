using AssetLoader.Runtime;
using CustomAttributes.Runtime.Attributes;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.Core.Audio
{
    [Resource(resourcePath: "DontDestroyOnLoad", name: "P_AudioController")]
    internal sealed class AudioController : PersistentSingletonBehavior<AudioController>, IAudioController
    {
        [SerializeField] [RequiredField] private AudioSource _musicSource;
        [SerializeField] [RequiredField] private AudioSource _effectsSource;

        [SerializeField] [RequiredField] private AudioClip[] _musicClip;

        public void PlayMusic(float volume = 1f)
        {
            PlaySound(_musicSource, GetRandomClip(_musicClip), volume);
        }

        public void PlayEffect(AudioClip clip, float volume = 1f)
        {
            PlaySound(_effectsSource, clip, volume);
        }

        private static void PlaySound(AudioSource source, AudioClip clip, float volume = 1f)
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }

        private static AudioClip GetRandomClip(AudioClip[] clips) => clips[Random.Range(0, clips.Length)];
    }
}
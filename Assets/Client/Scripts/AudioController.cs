using Client.Scripts.Patterns;
using UnityEngine;

namespace Client.Scripts
{
    [Resource("P_AudioManager", "DontDestroyOnLoad/P_AudioManager")]
    internal sealed class AudioController : SingletonDontDestroyOnLoad<AudioController>
    {
        [SerializeField, RequiredField] private AudioSource _musicSource;
        [SerializeField, RequiredField] private AudioSource _effectsSource;

        [SerializeField, RequiredField] private AudioClip[] _musicClip;

        internal void PlayMusic(float volume = 1f) => PlaySound(_musicSource, GetRandomClip(_musicClip), volume);

        internal void PlayEffect(AudioClip clip, float volume = 1f) => PlaySound(_effectsSource, clip, volume);

        private void PlaySound(AudioSource source, AudioClip clip, float volume = 1f)
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }

        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }
    }
}
using Client.Scripts.Patterns;
using UnityEngine;

namespace Client.Scripts
{
    [Resource("P_AudioManager", "DontDestroyOnLoad/P_AudioManager")]
    internal sealed class AudioController : SingletonMonoBehaviour<AudioController>
    {
        [SerializeField, RequiredField] private AudioSource _musicSource;
        [SerializeField, RequiredField] private GameObject _effectsSource;

        [SerializeField, RequiredField] private AudioClip[] _musicClip;

        internal void PlayMusic(float volume = 1f)
        {
            _musicSource.volume = volume;
            _musicSource.clip = GetRandomClip(_musicClip);
            _musicSource.Play();
        }

        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }

        protected override bool IsPersistent => true;
    }
}
namespace Source.Scripts.Core.Audio.Base
{
    internal interface IAudioHandler
    {
        void PlayOneShotSound(SoundType soundType, float volumeModifier = 1, float pitchModifier = 1);
    }
}
using Source.Scripts.Core.Audio.Sounds.Base;

namespace Source.Scripts.Core.Audio.Sounds
{
    internal sealed class AudioHandlerProvider : IAudioHandlerProvider
    {
        public IAudioHandler AudioHandler { get; private set; }

        public void SetAudioHandler(IAudioHandler audioHandler)
        {
            AudioHandler = audioHandler;
        }
    }
}
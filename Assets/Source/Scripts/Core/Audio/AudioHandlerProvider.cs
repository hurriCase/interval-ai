namespace Source.Scripts.Core.Audio
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
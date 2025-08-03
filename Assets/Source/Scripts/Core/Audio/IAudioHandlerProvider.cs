namespace Source.Scripts.Core.Audio
{
    internal interface IAudioHandlerProvider
    {
        IAudioHandler AudioHandler { get; }
        void SetAudioHandler(IAudioHandler audioHandler);
    }
}
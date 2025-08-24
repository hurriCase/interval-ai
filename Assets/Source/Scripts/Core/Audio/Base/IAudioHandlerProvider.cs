namespace Source.Scripts.Core.Audio.Base
{
    internal interface IAudioHandlerProvider
    {
        IAudioHandler AudioHandler { get; }
        void SetAudioHandler(IAudioHandler audioHandler);
    }
}
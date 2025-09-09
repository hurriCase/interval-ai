namespace Source.Scripts.Core.Audio.Sounds.Base
{
    internal interface IAudioHandlerProvider
    {
        IAudioHandler AudioHandler { get; }
        void SetAudioHandler(IAudioHandler audioHandler);
    }
}
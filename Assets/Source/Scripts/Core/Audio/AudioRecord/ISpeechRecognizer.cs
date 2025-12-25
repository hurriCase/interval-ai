using R3;

namespace Source.Scripts.Core.Audio.AudioRecord
{
    internal interface ISpeechRecognizer
    {
        Observable<string> OnTextReceived { get; }
        Observable<float> OnVolumeChanged { get; }
        Observable<string> OnErrorReceived { get; }
        void Init();
        void TryStartListening();
        void CancelListening();
        void StopListening();
        void Dispose();
    }
}
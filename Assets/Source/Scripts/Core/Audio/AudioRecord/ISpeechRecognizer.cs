using R3;

namespace Source.Scripts.Core.Audio.AudioRecord
{
    internal interface ISpeechRecognizer
    {
        Observable<string> OnRecognizedTextReceived { get; }
        Observable<string> OnError { get; }
        void TryStartListening();
        void StopListening();
    }
}
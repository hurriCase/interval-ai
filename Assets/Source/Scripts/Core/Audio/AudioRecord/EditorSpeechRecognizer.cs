using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Audio.AudioRecord
{
    [Preserve]
    internal sealed class EditorSpeechRecognizer : ISpeechRecognizer
    {
        public Observable<string> OnRecognizedTextReceived => _recognizedTextReceived;
        private readonly Subject<string> _recognizedTextReceived = new();

        public Observable<string> OnError => _error;
        private readonly Subject<string> _error = new();

        private readonly string[] _samples =
        {
            "Hello, how are you?",
            "This is a test message",
            "Speech recognition works!",
            "Testing in Unity Editor"
        };

        private bool _isListening;

        public void TryStartListening()
        {
            _isListening = true;

            SimulateRecognition().Forget();
        }

        public void StopListening()
        {
            _isListening = false;
        }

        private async UniTask SimulateRecognition()
        {
            await UniTask.WaitForSeconds(2f);

            if (_isListening is false)
                return;

            var simulatedText = _samples[Random.Range(0, _samples.Length)];

            _recognizedTextReceived.OnNext(simulatedText);
            _isListening = false;
        }
    }
}
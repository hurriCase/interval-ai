using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.Scripting;
using Random = UnityEngine.Random;

namespace Source.Scripts.Core.Audio.AudioRecord
{
    [Preserve]
    internal sealed class EditorSpeechRecognizer : ISpeechRecognizer, IDisposable
    {
        public Observable<string> OnTextReceived => _recognizedTextReceived;
        public Observable<float> OnVolumeChanged => _volumeChanged;
        public Observable<string> OnErrorReceived => _errorReceived;

        private readonly Subject<string> _recognizedTextReceived = new();
        private readonly Subject<float> _volumeChanged = new();
        private readonly Subject<string> _errorReceived = new();

        private readonly string[] _samples =
        {
            "Hello, how are you?",
            "This is a test message",
            "Speech recognition works!",
            "Testing in Unity Editor"
        };

        private bool _isListening;

        private readonly CancellationTokenSource _cancellationSource = new();

        public void Init()
        {
            // No need to do anything
        }

        public void TryStartListening()
        {
            if (_isListening)
                return;

            _isListening = true;
            SimulateRecognition().Forget();
        }

        public void StopListening()
        {
            _isListening = false;

            SendText();
        }

        public void CancelListening()
        {
            _isListening = false;
        }

        private async UniTask SimulateRecognition()
        {
            while (_isListening && _cancellationSource.Token.IsCancellationRequested is false)
            {
                _volumeChanged.OnNext(Random.Range(0f, 1f));

                await UniTask.WaitForSeconds(0.1f, cancellationToken: _cancellationSource.Token);
            }
        }

        private void SendText()
        {
            var simulatedText = _samples[Random.Range(0, _samples.Length)];

            _recognizedTextReceived.OnNext(simulatedText);
            _isListening = false;
        }

        public void Dispose()
        {
            _cancellationSource.Dispose();
            _recognizedTextReceived.Dispose();
            _volumeChanged.Dispose();
            _errorReceived.Dispose();
        }
    }
}
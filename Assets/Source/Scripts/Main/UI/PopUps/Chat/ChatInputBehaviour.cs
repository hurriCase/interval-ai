using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using ImprovedTimers;
using R3;
using Source.Scripts.Core.Audio.AudioRecord;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Chat
{
    internal sealed class ChatInputBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _textContainer;
        [SerializeField] private GameObject _audioContainer;
        [SerializeField] private ThemeButton _audioRecordButton;
        [SerializeField] private ThemeButton _sendAudioButton;
        [SerializeField] private ThemeButton _deleteButton;

        [SerializeField] private TextMeshProUGUI _timeText;

        [SerializeField] private float _waveChangeThreshold;
        [SerializeField] private float _minHeight;
        [SerializeField] private float _maxHeight;
        [SerializeField] private List<RectTransform> _audioWaves;

        private readonly StopwatchTimer _stopwatchTimer = new();

        private ISpeechRecognizer _speechRecognizer;

        [Inject]
        internal void Inject(ISpeechRecognizer speechRecognizer)
        {
            _speechRecognizer = speechRecognizer;
        }

        internal void Init()
        {
            Observable.EveryValueChanged(this, static self => self._stopwatchTimer.CurrentTime)
                .SubscribeUntilDestroy(this, static (time, self) => self.SetTimeText(time));

            _speechRecognizer.OnVolumeChanged
                .ThrottleFirst(TimeSpan.FromSeconds(_waveChangeThreshold))
                .SubscribeUntilDestroy(this, static (normalizedAmplitude, self)
                    => self.UpdateAudioWave(normalizedAmplitude));

            _audioRecordButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.ToggleRecord(true));

            _sendAudioButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.ToggleRecord(false));

            _deleteButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.CancelListening());
        }

        private void ToggleRecord(bool isListening)
        {
            if (isListening)
            {
                _stopwatchTimer.Start();
                _speechRecognizer.TryStartListening();
                ToggleContainers(true);
                return;
            }

            _speechRecognizer.StopListening();
            CleanRecords();
        }

        private void UpdateAudioWave(float normalizedAmplitude)
        {
            for (var i = _audioWaves.Count - 1; i > 0; i--)
            {
                var currentWave = _audioWaves[i];
                var previousWave = _audioWaves[i - 1];
                var previousHeight = previousWave.sizeDelta.y;

                currentWave.sizeDelta = new Vector2(currentWave.sizeDelta.x, previousHeight);
            }

            var firstWave = _audioWaves[0];
            var targetHeight = Mathf.Lerp(_minHeight, _maxHeight, normalizedAmplitude);
            firstWave.sizeDelta = new Vector2(firstWave.sizeDelta.x, targetHeight);
        }

        private void CancelListening()
        {
            _speechRecognizer.CancelListening();

            CleanRecords();
        }

        private void CleanRecords()
        {
            ToggleContainers(false);

            _stopwatchTimer.Stop();
            _stopwatchTimer.Reset();

            foreach (var wave in _audioWaves)
                wave.sizeDelta = new Vector2(wave.sizeDelta.x, _minHeight);
        }

        private void ToggleContainers(bool isListening)
        {
            _textContainer.SetActive(isListening is false);
            _audioContainer.SetActive(isListening);
        }

        private void SetTimeText(float seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            _timeText.SetText("{0:00}.{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }

        private void OnDestroy()
        {
            _stopwatchTimer.Dispose();
        }
    }
}
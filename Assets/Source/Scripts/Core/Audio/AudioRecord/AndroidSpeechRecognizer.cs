using System;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Audio.AndroidNative;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Audio.AudioRecord
{
    [Preserve]
    internal sealed class AndroidSpeechRecognizer : ISpeechRecognizer, IDisposable
    {
        private const string SpeechToTextControllerName = "com.communitycenter.audiorecorder.SpeechToTextController";

        public Observable<string> OnRecognizedTextReceived => _recognizedTextReceived;
        public Observable<string> OnError => _error;

        private readonly Subject<string> _recognizedTextReceived = new();
        private readonly Subject<string> _error = new();

        private readonly AndroidJavaObject _speechToTextController;

        private readonly ILanguageSettingsRepository _languageSettingsRepository;
        private readonly KotlinFunction1Callback _onResultCallback;
        private readonly KotlinFunction1Callback _errorCallback;

        [Preserve]
        internal AndroidSpeechRecognizer(ILanguageSettingsRepository languageSettingsRepository)
        {
            _languageSettingsRepository = languageSettingsRepository;

            using var unityPlayer = new AndroidJavaClass(AndroidConstants.UnityPlayer);

            var activity = unityPlayer.GetStatic<AndroidJavaObject>(AndroidConstants.CurrentActivity);
            _speechToTextController = new AndroidJavaObject(SpeechToTextControllerName, activity);

            _onResultCallback = new KotlinFunction1Callback(text => _recognizedTextReceived?.OnNext(text));
            _errorCallback = new KotlinFunction1Callback(error => _error.OnNext(error));
        }

        public void TryStartListening()
        {
            var systemLanguage = _languageSettingsRepository.LanguageByType.CurrentValue[LanguageType.Native];
            if (RecordPermissionGranter.HasPermission() is false)
            {
                RecordPermissionGranter.RequestPermission((self: this, systemLanguage),
                    static (tuple, isGranted) => tuple.self.HandlePermission(tuple.systemLanguage, isGranted));
                return;
            }

            StartListening(systemLanguage);
        }

        public void StopListening()
        {
            _speechToTextController.Call("stopListening");
        }

        private void StartListening(SystemLanguage systemLanguage)
        {
            var languageCode = systemLanguage.ToLocaleCode();

            _speechToTextController.Call("init", languageCode, _onResultCallback, _errorCallback);
            _speechToTextController.Call("startListening");
        }

        private void HandlePermission(SystemLanguage systemLanguage, bool isGranted)
        {
            if (isGranted)
            {
                StartListening(systemLanguage);
                return;
            }

            _error.OnNext("PermissionDenied");
        }

        public void Dispose()
        {
            _speechToTextController.Call("destroy");
            _speechToTextController.Dispose();
        }
    }
}
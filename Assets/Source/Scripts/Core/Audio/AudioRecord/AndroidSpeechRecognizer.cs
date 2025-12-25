using System;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Audio.AndroidNative;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Audio.AudioRecord
{
    [Preserve]
    internal sealed class AndroidSpeechRecognizer : ISpeechRecognizer, IDisposable
    {
        private const string SpeechToTextControllerName = "com.communitycenter.audiorecorder.SpeechToTextController";
        private const string CallbackInterfaceName = "com.communitycenter.audiorecorder.ResultCallback";

        public Observable<string> OnTextReceived => _textReceived;
        public Observable<float> OnVolumeChanged => _volumeChanged;
        public Observable<string> OnErrorReceived => _errorReceived;

        private readonly Subject<string> _textReceived = new();
        private readonly Subject<float> _volumeChanged = new();
        private readonly Subject<string> _errorReceived = new();

        private readonly AndroidJavaObject _speechToTextController;

        private readonly CallbackProxy<string> _onTextReceived;
        private readonly CallbackProxy<float> _onVolumeChanged;
        private readonly CallbackProxy<string> _onErrorReceived;

        private readonly ILanguageSettingsRepository _languageSettingsRepository;

        [Preserve]
        internal AndroidSpeechRecognizer(ILanguageSettingsRepository languageSettingsRepository)
        {
            _languageSettingsRepository = languageSettingsRepository;

            var activity = AndroidApplication.currentActivity;
            _speechToTextController = new AndroidJavaObject(SpeechToTextControllerName, activity);

            _onTextReceived = new CallbackProxy<string>(text => _textReceived.OnNext(text), CallbackInterfaceName);
            _onVolumeChanged = new CallbackProxy<float>(volume => _volumeChanged.OnNext(volume), CallbackInterfaceName);
            _onErrorReceived = new CallbackProxy<string>(error => _errorReceived.OnNext(error), CallbackInterfaceName);
        }

        public void Init()
        {
            var systemLanguage = _languageSettingsRepository.LanguageByType.CurrentValue[LanguageType.Native];
            var languageCode = systemLanguage.ToLocaleCode();

            _speechToTextController.CallWithLog(
                "init",
                languageCode,
                _onTextReceived,
                _onVolumeChanged,
                _onErrorReceived);

            _languageSettingsRepository.LanguageByType
                .Skip(1) // we pass language through init
                .Select(static languageByType => languageByType[LanguageType.Native])
                .Subscribe(this, static (language, self)
                    => self._speechToTextController.CallWithLog("updateLanguage", language));
        }

        public void TryStartListening()
        {
            if (RecordPermissionGranter.HasPermission() is false)
            {
                RecordPermissionGranter.RequestPermission(this,
                    static (self, isGranted) => self.HandlePermission(isGranted));
                return;
            }

            StartListening();
        }

        private void StartListening()
        {
            _speechToTextController.CallWithLog("startListening");
        }

        public void CancelListening()
        {
            _speechToTextController.CallWithLog("cancelListening");
        }

        public void StopListening()
        {
            _speechToTextController.CallWithLog("stopListening");
        }

        private void HandlePermission(bool isGranted)
        {
            if (isGranted)
            {
                StartListening();
                return;
            }

            _errorReceived.OnNext("PermissionDenied");
        }

        public void Dispose()
        {
            _speechToTextController.CallWithLog("destroy");
            _speechToTextController.Dispose();
        }
    }
}
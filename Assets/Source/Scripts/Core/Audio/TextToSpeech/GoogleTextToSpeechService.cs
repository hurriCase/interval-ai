using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.ApiHelper;
using Source.Scripts.Core.Audio.Sounds.Base;
using Source.Scripts.Core.Audio.TextToSpeech.Data;
using UnityEngine;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    internal sealed class GoogleTextToSpeechService : ApiServiceBase<GoogleTextToSpeechConfig>, ITextToSpeech
    {
        public ReadOnlyReactiveProperty<bool> IsAvailable => _isAvailable;
        private ReactiveProperty<bool> _isAvailable;

        private const string UrlPattern = "https://www.google.com/generate_{0}";
        private const long NoContentCode = 204;

        private readonly IAudioHandlerProvider _audioHandlerProvider;
        private readonly IApiAvailabilityChecker _apiAvailabilityChecker;

        internal GoogleTextToSpeechService(
            GoogleTextToSpeechConfig googleTextToSpeechConfig,
            IApiAvailabilityChecker apiAvailabilityChecker,
            IAudioHandlerProvider audioHandlerProvider,
            IApiClient apiClient)
            : base(apiClient, googleTextToSpeechConfig)
        {
            _apiAvailabilityChecker = apiAvailabilityChecker;
            _audioHandlerProvider = audioHandlerProvider;
        }

        public override async UniTask UpdateAvailable(CancellationToken token)
        {
            var url = ZString.Format(UrlPattern, NoContentCode);
            _isAvailable.Value = await _apiAvailabilityChecker.IsAvailable(url, NoContentCode, token);
        }

        public async UniTask SpeechTextAsync(string text, CancellationToken token)
        {
            var normalizedText = text?.Trim();
            if (string.IsNullOrEmpty(normalizedText))
                return;

            var audioClip = await AudioClipCreator.GetFromCacheAsync(normalizedText, token);
            if (audioClip)
            {
                _audioHandlerProvider.AudioHandler.PlayClip(audioClip);
                return;
            }

            audioClip = await CreateAudioClip(normalizedText, token);

            if (!audioClip)
                return;

            _audioHandlerProvider.AudioHandler.PlayClip(audioClip);
        }

        private async UniTask<AudioClip> CreateAudioClip(string normalizedText, CancellationToken token)
        {
            var request = new TextToSpeechRequest(normalizedText);
            var response = await GetResponse<TextToSpeechRequest, TextToSpeechResponse>(request, token);

            if (response is null || response.IsValid() is false)
                return null;

            var fileExtension = request.Audio.AudioEncoding.ToLowerInvariant();

            return await AudioClipCreator.CreateAudioClipAsync(
                normalizedText,
                response.AudioContent,
                fileExtension,
                token);
        }
    }
}
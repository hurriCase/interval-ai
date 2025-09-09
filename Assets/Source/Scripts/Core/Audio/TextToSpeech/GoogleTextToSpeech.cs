using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.ApiHelper;
using Source.Scripts.Core.Audio.Sounds.Base;
using UnityEngine;
using UnityEngine.Networking;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    internal sealed class GoogleTextToSpeech : ITextToSpeech
    {
        private string ApiUrl =>
            ZString.Format("https://texttospeech.googleapis.com/v1/text:synthesize?key={0}", _googleTTSConfig.ApiKey);

        private string CacheDirectory => Path.Combine(Application.persistentDataPath, "tts_cache");

        private readonly IAudioHandlerProvider _audioHandlerProvider;
        private readonly GoogleTTSConfig _googleTTSConfig;
        private readonly IApiHelper _apiHelper;
        private readonly Dictionary<string, AudioClip> _memoryCache = new();

        internal GoogleTextToSpeech(
            IAudioHandlerProvider audioHandlerProvider,
            GoogleTTSConfig googleTTSConfig,
            IApiHelper apiHelper)
        {
            _audioHandlerProvider = audioHandlerProvider;
            _googleTTSConfig = googleTTSConfig;
            _apiHelper = apiHelper;

            if (Directory.Exists(CacheDirectory) is false)
                Directory.CreateDirectory(CacheDirectory);
        }

        public async UniTask<AudioClip> SpeechTextAsync(string text, CancellationToken token)
        {
            var normalizedText = text?.Trim().ToLowerInvariant() ?? string.Empty;

            var audioClip = await GetFromCacheAsync(normalizedText, token);
            if (audioClip)
            {
                _audioHandlerProvider.AudioHandler.PlayClip(audioClip);
                return audioClip;
            }

            audioClip = await CreateAudioClip(normalizedText, token);

            if (!audioClip)
                return null;

            _audioHandlerProvider.AudioHandler.PlayClip(audioClip);

            return audioClip;
        }

        private async UniTask<AudioClip> CreateAudioClip(string normalizedText, CancellationToken token)
        {
            var request = new TextToSpeechRequest(normalizedText);
            var response = await _apiHelper.PostAsync<TextToSpeechRequest, TextToSpeechResponse>(request, ApiUrl);

            if (response is null || response.AudioContent.IsValid() is false)
                return null;

            var fileName = GenerateFileName(normalizedText);
            var filePath = Path.Combine(CacheDirectory, fileName);
            var audioData = Convert.FromBase64String(response.AudioContent);

            await File.WriteAllBytesAsync(filePath, audioData, token);

            var audioClip = await LoadAudioClipFromFileAsync(filePath, token);

            if (!audioClip)
                return null;

            _memoryCache[normalizedText] = audioClip;

            return audioClip;
        }

        private async UniTask<AudioClip> GetFromCacheAsync(string normalizedText, CancellationToken token)
        {
            if (normalizedText.IsValid() is false)
                return null;

            if (_memoryCache.TryGetValue(normalizedText, out var audioClip))
                return audioClip;

            var fileName = GenerateFileName(normalizedText);
            var filePath = Path.Combine(CacheDirectory, fileName);

            if (File.Exists(filePath) is false)
                return null;

            audioClip = await LoadAudioClipFromFileAsync(filePath, token);

            _memoryCache[normalizedText] = audioClip;
            return audioClip;
        }

        private async UniTask<AudioClip> LoadAudioClipFromFileAsync(string filePath, CancellationToken token)
        {
            try
            {
                var uri = ZString.Format("file://{0}", filePath);
                using var request = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG);
                await request.SendWebRequest().WithCancellation(token);

                if (request.result == UnityWebRequest.Result.Success)
                    return DownloadHandlerAudioClip.GetContent(request);

                Debug.LogError("[GoogleTextToSpeech::LoadAudioClipFromFileAsync] " +
                               $"Failed to load audio clip from {filePath}: {request.error}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.LogError("[GoogleTextToSpeech::LoadAudioClipFromFileAsync] " +
                               $"Exception loading audio clip from {filePath}: {ex.Message}");
                return null;
            }
        }

        private static string GenerateFileName(string text)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            var hashString = BitConverter.ToString(hash).Replace("-", "")[..16]; // First 16 chars
            return $"{hashString}.mp3";
        }
    }
}
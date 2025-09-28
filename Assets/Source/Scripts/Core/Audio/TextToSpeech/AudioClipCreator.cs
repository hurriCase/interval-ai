using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    internal static class AudioClipCreator
    {
        private const string CacheFolderName = "tts_cache";

        private static string CacheDirectory => Path.Combine(Application.persistentDataPath, CacheFolderName);

        private static readonly Dictionary<string, AudioClip> _memoryCache = new();

        static AudioClipCreator()
        {
            CacheDirectory.CreateDirectoryIfNotExist();
        }

        internal static async UniTask<AudioClip> CreateAudioClipAsync(
            string normalizedText,
            string audioContent,
            string extension,
            CancellationToken token)
        {
            var hash = normalizedText.GetHash();
            var fileName = ZString.Format("{0}.{1}", hash, extension);
            var filePath = Path.Combine(CacheDirectory, fileName);
            var audioData = Convert.FromBase64String(audioContent);

            await File.WriteAllBytesAsync(filePath, audioData, token);

            return await LoadAudioClipFromFileAsync(filePath, token);
        }

        internal static async UniTask<AudioClip> GetFromCacheAsync(string normalizedText, CancellationToken token)
        {
            if (_memoryCache.TryGetValue(normalizedText, out var audioClip))
                return audioClip;

            var filePath = Path.Combine(CacheDirectory, normalizedText.GetHash());

            if (File.Exists(filePath) is false)
                return null;

            audioClip = await LoadAudioClipFromFileAsync(filePath, token);

            _memoryCache[normalizedText] = audioClip;
            return audioClip;
        }

        private static async UniTask<AudioClip> LoadAudioClipFromFileAsync(string filePath, CancellationToken token)
        {
            try
            {
                var uri = ZString.Format("file://{0}", filePath);
                using var request = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG);
                await request.SendWebRequest().WithCancellation(token);

                if (request.result == UnityWebRequest.Result.Success)
                    return DownloadHandlerAudioClip.GetContent(request);

                Debug.LogError("[AudioClipCreator::LoadAudioClipFromFileAsync] " +
                               $"Failed to load audio clip from {filePath}: {request.error}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.LogError("[AudioClipCreator::LoadAudioClipFromFileAsync] " +
                               $"Exception loading audio clip from {filePath}: {ex.Message}");
                Debug.LogException(ex);
                return null;
            }
        }
    }
}
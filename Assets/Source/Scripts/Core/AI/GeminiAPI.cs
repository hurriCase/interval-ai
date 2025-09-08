using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Storage;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Source.Scripts.Core.Repositories.Base;
using UnityEngine;
using UnityEngine.Networking;

namespace Source.Scripts.Core.AI
{
    internal sealed class GeminiAPI : IAITextController
    {
        private readonly PersistentReactiveProperty<List<Content>> _chatHistory = new();
        private readonly GenerativeModel _textModel = GenerativeModel.Default;

        public async UniTask InitAsync(CancellationToken token)
        {
            await _chatHistory.InitAsync(PersistentKeys.ChatHistoryKey, token, new List<Content>());
        }

        public async UniTask<string> SendSinglePromptAsync(string prompt)
        {
            var requestData = new ChatRequest(new List<Content> { new(prompt, Role.User) });
            var response = await SendRequestAsync(requestData);

            return ParseResponse(response);
        }

        public async UniTask<string> SendPromptWithChatHistoryAsync(string message)
        {
            var userContent = new Content(message, Role.User);

            _chatHistory.Value.Add(userContent);

            var chatRequest = new ChatRequest(_chatHistory.Value);
            var response = await SendRequestAsync(chatRequest);

            var parsedResponse = ParseResponse(response);

            if (parsedResponse.IsValid() is false)
                return parsedResponse;

            var botContent = new Content(parsedResponse, Role.Model);

            _chatHistory.Value.Add(botContent);

            return parsedResponse;
        }

        public void ClearChatHistoryAsync(List<Content> initialHistory = null)
            => _chatHistory.Value = initialHistory ?? new List<Content>();

        private async UniTask<string> SendRequestAsync(ChatRequest data)
        {
            var fullUrl = string.Format(_textModel.EndpointFormat, _textModel.ModelName, _textModel.ApiKey);
            using var request = new UnityWebRequest(fullUrl, "POST");
            var jsonData = JsonConvert.SerializeObject(data);
            var bodyRaw = Encoding.UTF8.GetBytes(jsonData);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            try
            {
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                    return request.downloadHandler.text;

                var message = ZString.Format("[GeminiAPI::SendRequestAsync] " +
                                             "Request failed: {0} " +
                                             "Response Code: {1}" +
                                             "Response Headers: {2} " +
                                             "Response Body: {3} With Json Data: {4}",
                    request.responseCode, request.GetResponseHeaders(), request.downloadHandler?.text, jsonData);

                Debug.LogError(message);
                return null;
            }
            catch (Exception e)
            {
                var message = ZString.Format("[GeminiAPI::SendRequestAsync] Request failed with exception: {0}" +
                                             "Stack trace: {1}" +
                                             "Request failed: {2}" +
                                             "With Json Data: {3}",
                    e, e.StackTrace, e.Message, jsonData);

                Debug.LogError(message);
                return null;
            }
        }

        private string ParseResponse(string responseJson)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<Response>(responseJson);

                if (response.Candidates is { Length: > 0 } &&
                    response.Candidates[0].Content.Parts is { Length: > 0 })
                    return response.Candidates[0].Content.Parts[0].Text;

                Debug.LogWarning(
                    $"[GeminiAPI::ParseResponse] Failed to parse response with {responseJson} string json");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"[GeminiAPI::ParseResponse] Failed to parse response: {e.Message}");
                return null;
            }
        }
    }

    [Serializable]
    internal struct ChatRequest
    {
        [JsonProperty("contents")] public List<Content> Contents { get; private set; }

        internal ChatRequest(List<Content> contents)
        {
            Contents = contents;
        }
    }

    [Serializable]
    internal struct Response
    {
        [JsonProperty("candidates")] public Candidate[] Candidates { get; private set; }
    }

    [Serializable]
    internal struct Candidate
    {
        [JsonProperty("content")] public Content Content { get; private set; }
    }

    [Serializable]
    internal struct Content
    {
        [JsonProperty("parts")] public Part[] Parts { get; private set; }
        [JsonProperty("role")] public string Role { get; private set; }

        internal Content(string text, Role role)
        {
            Parts = new[] { new Part(text) };
            Role = role.GetJsonPropertyName();
        }
    }

    [Serializable]
    internal struct Part
    {
        [JsonProperty("text")] public string Text { get; private set; }

        internal Part(string text)
        {
            Text = text;
        }
    }

    internal enum Role
    {
        [JsonProperty("user")] User,
        [JsonProperty("model")] Model
    }
}
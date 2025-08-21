using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Storage;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using ZLinq;

namespace Source.Scripts.Core.AI
{
    internal sealed class GeminiAPI : Singleton<GeminiAPI>, IAIController
    {
        private PersistentReactiveProperty<Content[]> _chatHistory;
        private readonly GenerativeModel _textModel = GenerativeModel.Default;

        public async Task<string> SendPromptAsync(string prompt)
        {
            var requestData = new ChatRequest(new[] { new Content(Role.User, prompt) });
            var response = await SendRequestAsync(requestData);

            return ParseResponse(response);
        }

        public async Task<string> SendChatMessageAsync(string message)
        {
            var userContent = new Content(Role.User, message);

            var contentsList = new List<Content>(_chatHistory.Value)
            {
                userContent
            };
            _chatHistory.Value = contentsList.ToArray();

            var chatRequest = new ChatRequest(_chatHistory.Value);
            var response = await SendRequestAsync(chatRequest);

            var parsedResponse = ParseResponse(response);

            if (string.IsNullOrEmpty(parsedResponse))
                return parsedResponse;

            var botContent = new Content(Role.Model, parsedResponse);

            contentsList.Add(botContent);
            _chatHistory.Value = contentsList.ToArray();

            return parsedResponse;
        }

        public void ClearChatHistoryAsync(Content[] initialHistory = null)
            => _chatHistory.Value = initialHistory ?? Array.Empty<Content>();

        private async Task<string> SendRequestAsync(ChatRequest data)
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
                var operation = request.SendWebRequest();
                while (operation.isDone is false)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                    return request.downloadHandler.text;

                Debug.LogError($"[GeminiAPI::SendRequestAsync] Request failed: {request.error}" +
                               $"Response Code: {request.responseCode}" +
                               $"Response Headers: {request.GetResponseHeaders()}" +
                               $"Response Body: {request.downloadHandler?.text}" +
                               $"With Json Data: {jsonData}");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"[GeminiAPI::SendRequestAsync] Request failed with exception: {e}" +
                               $"Stack trace: {e.StackTrace}" +
                               $"Request failed: {e.Message}" +
                               $"With Json Data: {jsonData}");
                return null;
            }
        }

        private string ParseResponse(string responseJson)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<Response>(responseJson);
                if (response.Candidates is { Length: > 0 } &&
                    response.Candidates[0].Content.Parts.Length > 0)
                    return response.Candidates.AsValueEnumerable()
                        .FirstOrDefault().Content.Parts
                        .FirstOrDefault().Text;

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
    internal readonly struct ChatRequest
    {
        [JsonProperty("contents")] public Content[] Contents { get; }

        internal ChatRequest(Content[] contents)
        {
            Contents = contents;
        }
    }

    [Serializable]
    internal struct Response
    {
        [JsonProperty("candidates")] public Candidate[] Candidates { get; set; }
    }

    [Serializable]
    internal struct Candidate
    {
        [JsonProperty("content")] public Content Content { get; set; }
    }

    [Serializable]
    internal readonly struct Content
    {
        [JsonProperty("role")] public string Role { get; }
        [JsonProperty("parts")] public Part[] Parts { get; }

        internal Content(Role role, string text)
        {
            Role = role.GetJsonPropertyName();
            Parts = new[] { new Part(text) };
        }
    }

    [Serializable]
    internal struct Part
    {
        [JsonProperty("text")] public string Text { get; }

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
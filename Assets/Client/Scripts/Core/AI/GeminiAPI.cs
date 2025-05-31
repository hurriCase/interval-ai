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

namespace Client.Scripts.Core.AI
{
    internal sealed class GeminiAPI : Singleton<GeminiAPI>, IAIController
    {
        private PersistentReactiveProperty<Content[]> _chatHistory;
        private readonly GenerativeModel _textModel = GenerativeModel.Default;

        public async Task<string> SendPromptAsync(string prompt)
        {
            var requestData = new ChatRequest
            {
                Contents = new[]
                {
                    GetContent(Role.User, prompt)
                }
            };

            var response = await SendRequestAsync(requestData);
            return ParseResponse(response);
        }

        public async Task<string> SendChatMessageAsync(string message)
        {
            var userContent = GetContent(Role.User, message);

            var contentsList = new List<Content>(_chatHistory.Value)
            {
                userContent
            };
            _chatHistory.Value = contentsList.ToArray();

            var chatRequest = new ChatRequest
            {
                Contents = _chatHistory.Value
            };
            var response = await SendRequestAsync(chatRequest);

            var parsedResponse = ParseResponse(response);

            if (string.IsNullOrEmpty(parsedResponse))
                return parsedResponse;

            var botContent = GetContent(Role.Model, parsedResponse);

            contentsList.Add(botContent);
            _chatHistory.Value = contentsList.ToArray();

            return parsedResponse;
        }

        public void ClearChatHistoryAsync(Content[] initialHistory = null)
            => _chatHistory.Value = initialHistory ?? Array.Empty<Content>();

        private Content GetContent(Role role, string text) =>
            new()
            {
                Role = role.GetJsonPropertyName(),
                Parts = new[] { new Part { Text = text } }
            };

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
    internal struct ChatRequest
    {
        [JsonProperty("contents")]
        public Content[] Contents { get; set; }
    }

    [Serializable]
    internal struct Response
    {
        [JsonProperty("candidates")]
        public Candidate[] Candidates { get; set; }
    }

    [Serializable]
    internal struct Candidate
    {
        [JsonProperty("content")]
        public Content Content { get; set; }
    }

    [Serializable]
    internal struct Content
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("parts")]
        public Part[] Parts { get; set; }
    }

    [Serializable]
    internal struct Part
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    internal enum Role
    {
        [JsonProperty("user")]
        User,

        [JsonProperty("model")]
        Model
    }
}
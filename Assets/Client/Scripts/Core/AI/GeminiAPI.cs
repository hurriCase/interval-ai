using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Text;
using Client.Scripts.Core.AI;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using Client.Scripts.Patterns.Extensions;
using Newtonsoft.Json;

namespace Client.Scripts.Core.AiController
{
    internal sealed class GeminiAPI : Injectable, IAIController
    {
        [Inject] private IDBController _dbController;

        private const string ChatHistoryPath = "ai_chat_history";
        private const string TextModelPath = "ai_settings";

        private GenerativeModel _textModel;
        private Content[] _chatHistory;
        private bool _isInited;

        public async Task InitAsync()
        {
            try
            {
                if (_isInited)
                    return;

                await LoadGenerativeModelAsync();
                await LoadChatHistoryAsync();

                _isInited = true;

                Debug.Log($"[GeminiAPI::InitAsync] GeminiAPI initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[GeminiAPI::InitAsync] Error loading chat history: {e.Message}");
            }
        }

        public async Task<string> SendPromptAsync(string prompt, GenerationConfig config = null)
        {
            var requestData = new ChatRequest
            {
                Contents = new[]
                {
                    GetContent(Role.User, prompt)
                },
                GenerationConfig = _textModel.GenerationConfig
            };

            var response = await SendRequestAsync(requestData);
            return ParseResponse(response);
        }

        public void ClearChatHistory(List<ChatRequest> initialHistory = null)
        {
            _chatHistory = Array.Empty<Content>();
        }

        public async Task<string> SendChatMessageAsync(string message, bool stream = false)
        {
            var userContent = GetContent(Role.User, message);

            var contentsList = new List<Content>(_chatHistory)
            {
                userContent
            };
            _chatHistory = contentsList.ToArray();

            var chatRequest = new ChatRequest
            {
                Contents = _chatHistory,
                GenerationConfig = _textModel.GenerationConfig
            };
            var response = await SendRequestAsync(chatRequest);

            var parsedResponse = ParseResponse(response);

            if (string.IsNullOrEmpty(parsedResponse) is false)
            {
                var botContent = GetContent(Role.Model, parsedResponse);

                contentsList.Add(botContent);
                _chatHistory = contentsList.ToArray();
                await SaveChatHistoryAsync();
            }

            return parsedResponse;
        }

        private Content GetContent(Role role, string text)
        {
            return new Content
            {
                Role = role.GetJsonPropertyName(),
                Parts = new[]
                {
                    new Part { Text = text }
                }
            };
        }

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
                if (response?.Candidates is { Length: > 0 } &&
                    response.Candidates[0].Content.Parts.Length > 0)
                    return response.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

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

        private async Task LoadChatHistoryAsync()
        {
            try
            {
                _chatHistory = await _dbController.ReadDataAsync<Content[]>(ChatHistoryPath) ?? Array.Empty<Content>();
            }
            catch (Exception e)
            {
                Debug.LogError($"[GeminiAPI::LoadChatHistoryAsync] Failed to load chat history: {e.Message}");
                _chatHistory = Array.Empty<Content>();
            }
        }

        private async Task SaveChatHistoryAsync()
        {
            try
            {
                await _dbController.WriteDataAsync(ChatHistoryPath, _chatHistory);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GeminiAPI::SaveChatHistoryAsync] Failed to save chat history: {e.Message}");
            }
        }

        private async Task LoadGenerativeModelAsync()
        {
            try
            {
                _textModel = await _dbController.ReadDataAsync<GenerativeModel>(TextModelPath)
                             ?? await _dbController.WriteDataAsync(TextModelPath, new GenerativeModel());
            }
            catch (Exception e)
            {
                Debug.LogError($"[GeminiAPI::LoadGenerativeModelAsync] Failed to load chat history: {e.Message}");
                _textModel = new GenerativeModel();
            }
        }
    }

    [Serializable]
    internal sealed class ChatRequest
    {
        [JsonProperty("contents")] internal Content[] Contents { get; set; }
        [JsonProperty("generationConfig")] internal GenerationConfig GenerationConfig { get; set; }
    }

    [Serializable]
    internal sealed class Response
    {
        [JsonProperty("candidates")] internal Candidate[] Candidates { get; set; }
    }

    [Serializable]
    internal sealed class Candidate
    {
        [JsonProperty("content")] internal Content Content { get; set; }
    }

    [Serializable]
    internal sealed class Content
    {
        [JsonProperty("role")] internal string Role { get; set; }

        [JsonProperty("parts")] internal Part[] Parts { get; set; }
    }

    [Serializable]
    internal sealed class Part
    {
        [JsonProperty("text")] internal string Text { get; set; }
    }

    internal enum Role
    {
        [JsonProperty("user")] User,
        [JsonProperty("model")] Model
    }
}
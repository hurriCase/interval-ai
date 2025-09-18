using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.ApiHelper;
using Source.Scripts.Core.GenerativeLanguage.Data;
using Source.Scripts.Core.Repositories.Base;

namespace Source.Scripts.Core.GenerativeLanguage
{
    internal sealed class GeminiGenerativeLanguage : IGenerativeLanguage
    {
        private readonly PersistentReactiveProperty<List<Content>> _chatHistory = new();

        private readonly GeminiGenerativeLanguageConfig _geminiGenerativeLanguageConfig;
        private readonly IApiHelper _apiHelper;

        internal GeminiGenerativeLanguage(
            GeminiGenerativeLanguageConfig geminiGenerativeLanguageConfig,
            IApiHelper apiHelper)
        {
            _geminiGenerativeLanguageConfig = geminiGenerativeLanguageConfig;
            _apiHelper = apiHelper;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            await _chatHistory.InitAsync(PersistentKeys.ChatHistoryKey, token, new List<Content>());
        }

        public async UniTask<string> SendPromptWithChatHistoryAsync(string message, CancellationToken token)
        {
            var userContent = new Content(message, Role.User);

            _chatHistory.Value.Add(userContent);

            var chatRequest = new ChatRequest(_chatHistory.Value);

            var parsedResponse = await GetResponseTextFromRequest(chatRequest, token);

            if (parsedResponse.IsValid() is false)
                return parsedResponse;

            var botContent = new Content(parsedResponse, Role.Model);

            _chatHistory.Value.Add(botContent);

            return parsedResponse;
        }

        private async UniTask<string> GetResponseTextFromRequest(ChatRequest chatRequest, CancellationToken token)
        {
            var url = _geminiGenerativeLanguageConfig.GetApiUrl();
            var response = await _apiHelper.PostAsync<ChatRequest, Response>(chatRequest, url, token);

            if (response?.Candidates is { Length: > 0 } &&
                response.Candidates[0].Content.Parts is { Length: > 0 })
                return response.Candidates[0].Content.Parts[0].Text;

            return string.Empty;
        }
    }
}
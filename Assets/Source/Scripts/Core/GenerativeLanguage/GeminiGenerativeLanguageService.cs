using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.ApiHelper;
using Source.Scripts.Core.GenerativeLanguage.Data;
using Source.Scripts.Core.Repositories.Base;

namespace Source.Scripts.Core.GenerativeLanguage
{
    internal sealed class GeminiGenerativeLanguageService : ApiServiceBase<GeminiGenerativeLanguageConfig>,
        IGenerativeLanguage
    {
        private readonly PersistentReactiveProperty<List<Content>> _chatHistory = new();

        internal GeminiGenerativeLanguageService(
            GeminiGenerativeLanguageConfig geminiGenerativeLanguageConfig,
            IApiAvailabilityChecker apiAvailabilityChecker,
            IApiClient apiClient)
            : base(apiAvailabilityChecker, apiClient, geminiGenerativeLanguageConfig) { }

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

            if (string.IsNullOrEmpty(parsedResponse))
                return parsedResponse;

            var botContent = new Content(parsedResponse, Role.Model);

            _chatHistory.Value.Add(botContent);

            return parsedResponse;
        }

        private async UniTask<string> GetResponseTextFromRequest(ChatRequest chatRequest, CancellationToken token)
        {
            var response = await GetResponse<ChatRequest, Response>(chatRequest, token);

            if (response?.Candidates is { Length: > 0 } &&
                response.Candidates[0].Content.Parts is { Length: > 0 })
                return response.Candidates[0].Content.Parts[0].Text;

            return string.Empty;
        }
    }
}
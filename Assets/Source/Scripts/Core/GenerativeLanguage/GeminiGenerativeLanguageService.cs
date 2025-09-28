using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Api.Base;
using Source.Scripts.Core.Api.Interfaces;
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
            AddChatHistory(message, Role.User);

            var chatRequest = new ChatRequest(_chatHistory.Value);

            var response = await GetResponse<ChatRequest, GenerativeLanguageResponse>(chatRequest, token);

            if (response.Success is false)
                return string.Empty;

            var text = response.Data.GetText();

            AddChatHistory(text, Role.Model);

            return text;
        }

        private void AddChatHistory(string message, Role role)
        {
            var userContent = new Content(message, role);

            _chatHistory.Value.Add(userContent);
        }
    }
}
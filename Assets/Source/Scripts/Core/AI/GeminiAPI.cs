using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Storage;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.AI.Data;
using Source.Scripts.Core.ApiHelper;
using Source.Scripts.Core.Repositories.Base;

namespace Source.Scripts.Core.AI
{
    internal sealed class GeminiAPI : IAITextController
    {
        private readonly PersistentReactiveProperty<List<Content>> _chatHistory = new();
        private readonly GenerativeModel _textModel = GenerativeModel.Default;

        private readonly IApiHelper _apiHelper;

        internal GeminiAPI(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            await _chatHistory.InitAsync(PersistentKeys.ChatHistoryKey, token, new List<Content>());
        }

        public async UniTask<string> SendSinglePromptAsync(string prompt)
        {
            var requestData = new ChatRequest(new List<Content> { new(prompt, Role.User) });

            return await GetResponseTextFromRequest(requestData);
        }

        public async UniTask<string> SendPromptWithChatHistoryAsync(string message)
        {
            var userContent = new Content(message, Role.User);

            _chatHistory.Value.Add(userContent);

            var chatRequest = new ChatRequest(_chatHistory.Value);

            var parsedResponse = await GetResponseTextFromRequest(chatRequest);

            if (parsedResponse.IsValid() is false)
                return parsedResponse;

            var botContent = new Content(parsedResponse, Role.Model);

            _chatHistory.Value.Add(botContent);

            return parsedResponse;
        }

        private async UniTask<string> GetResponseTextFromRequest(ChatRequest chatRequest)
        {
            var fullUrl = ZString.Format(_textModel.EndpointFormat, _textModel.ModelName, _textModel.ApiKey);
            var response = await _apiHelper.PostAsync<ChatRequest, Response>(chatRequest, fullUrl);

            if (response?.Candidates is { Length: > 0 } &&
                response.Candidates[0].Content.Parts is { Length: > 0 })
                return response.Candidates[0].Content.Parts[0].Text;

            return string.Empty;
        }

        public void ClearChatHistoryAsync(List<Content> initialHistory = null)
            => _chatHistory.Value = initialHistory ?? new List<Content>();
    }
}
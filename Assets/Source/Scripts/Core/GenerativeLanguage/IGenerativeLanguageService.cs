using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Api.Interfaces;

namespace Source.Scripts.Core.GenerativeLanguage
{
    internal interface IGenerativeLanguageService : IApiService
    {
        UniTask InitAsync(CancellationToken token);
        UniTask<string> SendPromptWithChatHistoryAsync(string message, CancellationToken token);
    }
}
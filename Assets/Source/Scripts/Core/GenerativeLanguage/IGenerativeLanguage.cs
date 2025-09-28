using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.ApiHelper;

namespace Source.Scripts.Core.GenerativeLanguage
{
    internal interface IGenerativeLanguage : IApiService
    {
        UniTask InitAsync(CancellationToken token);
        UniTask<string> SendPromptWithChatHistoryAsync(string message, CancellationToken token);
    }
}
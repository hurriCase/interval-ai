using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.GenerativeLanguage
{
    internal interface IGenerativeLanguage
    {
        UniTask InitAsync(CancellationToken token);
        UniTask<string> SendPromptWithChatHistoryAsync(string message);
    }
}
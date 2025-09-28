using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace Source.Scripts.Core.GenerativeLanguage
{
    internal interface IGenerativeLanguage
    {
        ReadOnlyReactiveProperty<bool> IsAvailable { get; }
        UniTask InitAsync(CancellationToken token);
        UniTask UpdateAvailable(CancellationToken token);
        UniTask<string> SendPromptWithChatHistoryAsync(string message, CancellationToken token);
    }
}
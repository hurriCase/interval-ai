using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.AI.Data;

namespace Source.Scripts.Core.AI
{
    internal interface IAITextController
    {
        UniTask InitAsync(CancellationToken token);
        UniTask<string> SendSinglePromptAsync(string prompt);
        UniTask<string> SendPromptWithChatHistoryAsync(string message);
        void ClearChatHistoryAsync(List<Content> initialHistory = null);
    }
}
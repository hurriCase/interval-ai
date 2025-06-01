using System.Threading.Tasks;

namespace Client.Scripts.Core.AI
{
    internal interface IAIController
    {
        Task<string> SendPromptAsync(string prompt);
        Task<string> SendChatMessageAsync(string message);
        void ClearChatHistoryAsync(Content[] initialHistory = null);
    }
}
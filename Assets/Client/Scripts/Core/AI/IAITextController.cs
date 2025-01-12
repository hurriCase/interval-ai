using System.Threading.Tasks;

namespace Client.Scripts.Core.AI
{
    internal interface IAIController
    {
        Task InitAsync();
        Task<string> SendPromptAsync(string prompt, GenerationConfig config = null);
        Task<string> SendChatMessageAsync(string message);
        Task ClearChatHistoryAsync(Content[] initialHistory = null);
    }
}
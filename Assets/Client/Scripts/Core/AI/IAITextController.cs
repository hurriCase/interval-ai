using System.Threading.Tasks;
using Client.Scripts.Core.AiController;

namespace Client.Scripts.Patterns.DI.Services

{
    internal interface IAIController
    {
        Task InitAsync();
        Task<string> SendPromptAsync(string prompt, GenerationConfig config = null);
        Task<string> SendChatMessageAsync(string message);
        Task ClearChatHistoryAsync(Content[] initialHistory = null);
    }
}
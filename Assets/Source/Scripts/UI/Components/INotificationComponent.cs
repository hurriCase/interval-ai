using Cysharp.Threading.Tasks;

namespace Source.Scripts.UI.Components
{
    internal interface INotificationComponent
    {
        UniTask ShowMessage(string message);
        void HideMessage();
    }
}
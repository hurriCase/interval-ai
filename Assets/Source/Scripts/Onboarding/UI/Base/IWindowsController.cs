using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Onboarding.UI.Base
{
    internal interface IWindowsController
    {
        UniTask InitAsync(CancellationToken cancellationToken);
        void OpenPopUpByType(PopUpType popUpType);
    }
}
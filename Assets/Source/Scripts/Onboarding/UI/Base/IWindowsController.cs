using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Windows.Base;

namespace Source.Scripts.Onboarding.UI.Base
{
    internal interface IWindowsController
    {
        UniTask InitAsync(CancellationToken cancellationToken);
        TPopUpType OpenPopUp<TPopUpType>() where TPopUpType : PopUpBase;
    }
}
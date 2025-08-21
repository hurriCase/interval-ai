using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Windows.Base.PopUp;

namespace Source.Scripts.Onboarding.UI.Base
{
    internal interface IWindowsController
    {
        UniTask InitAsync(CancellationToken cancellationToken);
        TPopUpType OpenPopUp<TPopUpType>() where TPopUpType : PopUpBase;
    }
}
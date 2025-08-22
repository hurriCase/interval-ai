using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Windows.Base;

namespace Source.Scripts.Main.UI.Base
{
    internal interface IWindowsController
    {
        ScreenType InitialScreenType { get; }
        UniTask InitAsync(CancellationToken cancellationToken);
        void OpenScreenByType(ScreenType screenEnum);
        void OpenPopUpByType(PopUpType popUpEnum);
        TPopUpType OpenPopUp<TPopUpType>() where TPopUpType : PopUpBase;
    }
}
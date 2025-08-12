using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Main.UI.Base
{
    internal interface IWindowsController
    {
        ScreenType InitialScreenType { get; }
        UniTask InitAsync(CancellationToken cancellationToken);
        void OpenScreenByType(ScreenType screenType);
        void OpenPopUpByType(PopUpType popUpType);
        void OpenPopUpByType<TParameters>(PopUpType popUpType, TParameters parameters);
    }
}
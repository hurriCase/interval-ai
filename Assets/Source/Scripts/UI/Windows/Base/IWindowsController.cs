using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Windows.Base.PopUp;
using Source.Scripts.UI.Windows.Base.Screen;

namespace Source.Scripts.UI.Windows.Base
{
    internal interface IWindowsController
    {
        UniTask InitAsync(CancellationToken cancellationToken);
        void OpenPopUpByType(PopUpType popUpType);
        void OpenScreenByType(ScreenType screenType);
        ScreenType GetInitialScreenType();
    }
}
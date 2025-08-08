using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Loader;
using VContainer;

namespace Source.Scripts.UI.Windows.Base
{
    internal interface IWindowsController
    {
        UniTask InitAsync(
            IObjectResolver objectResolver,
            IAddressablesLoader addressablesLoader,
            CancellationToken cancellationToken);

        void OpenPopUpByType(PopUpType popUpType);
        void OpenScreenByType(ScreenType screenType);
        ScreenType GetInitialScreenType();
    }
}
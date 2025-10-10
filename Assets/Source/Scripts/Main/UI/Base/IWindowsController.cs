using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.UI.Windows.Base;
using UnityEngine.EventSystems;

namespace Source.Scripts.Main.UI.Base
{
    internal interface IWindowsController
    {
        ReadOnlyReactiveProperty<ScreenType> CurrentScreenType { get; }
        PopUpType CurrentPopUpType { get; }
        UniTask InitAsync(CancellationToken cancellationToken);
        void OpenScreenByType(ScreenType screenEnum);
        TPopUpType OpenPopUp<TPopUpType>() where TPopUpType : PopUpBase;
        void BindPopUpOpen(UIBehaviour component, PopUpType popUpType);
        void BindScreenOpen(UIBehaviour component, ScreenType popUpType);
    }
}
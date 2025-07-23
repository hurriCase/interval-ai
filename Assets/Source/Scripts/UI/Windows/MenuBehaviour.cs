using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Windows
{
    internal sealed class MenuBehaviour : MonoBehaviour, IMenuBehaviour
    {
        [SerializeField] private EnumArray<ScreenType, ThemeToggle> _menuToggles = new(EnumMode.SkipFirst);

        [Inject] private IWindowsController _windowsController;

        public void Init(CancellationToken cancellationToken)
        {
            var linkedSource = cancellationToken.CreateLinkedTokenSourceWithDestroy(this);
            var initialScreenType = _windowsController.GetInitialScreenType();

            foreach (var (screenType, themeToggle) in _menuToggles.AsTuples())
            {
                themeToggle.OnPointerClickAsObservable()
                    .Subscribe((_windowsController, screenType),
                        static (_, tuple) => tuple._windowsController.OpenScreenByType(tuple.screenType))
                    .RegisterTo(linkedSource.Token);

                if (screenType == initialScreenType)
                    themeToggle.isOn = true;
            }
        }
    }
}
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Base.Screen;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Windows.Menu
{
    internal sealed class MenuBehaviour : MonoBehaviour, IMenuBehaviour
    {
        [SerializeField] private EnumArray<ScreenType, TabComponent> _menuToggles = new(EnumMode.SkipFirst);

        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(IWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        public void Init(CancellationToken cancellationToken)
        {
            var linkedSource = cancellationToken.CreateLinkedTokenSourceWithDestroy(this);

            foreach (var (screenType, themeToggle) in _menuToggles.AsTuples())
            {
                if (screenType == ScreenType.OnboardingInput)
                    continue;

                themeToggle.OnPointerClickAsObservable()
                    .Subscribe((_windowsController, screenType),
                        static (_, tuple) => tuple._windowsController.OpenScreenByType(tuple.screenType))
                    .RegisterTo(linkedSource.Token);

                if (screenType == _windowsController.GetInitialScreenType())
                    themeToggle.isOn = true;
            }
        }
    }
}
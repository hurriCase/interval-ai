using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3.Triggers;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Menu;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Base
{
    internal sealed class MenuBehaviour : MonoBehaviour, IMenuBehaviour
    {
        [SerializeField] private EnumArray<ScreenType, ToggleComponent> _menuToggles = new(EnumMode.SkipFirst);
        [SerializeField] private ToggleGroup _toggleGroup;

        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(IWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        public void Init()
        {
            foreach (var (screenType, themeToggle) in _menuToggles.AsTuples())
            {
                themeToggle.OnPointerClickAsObservable().SubscribeAndRegister(this, screenType,
                    static (screenType, self) => self._windowsController.OpenScreenByType(screenType));

                themeToggle.group = _toggleGroup;

                if (screenType == _windowsController.InitialScreenType)
                    themeToggle.isOn = true;
            }
        }
    }
}
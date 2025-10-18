using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Screens.Settings.Behaviours;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Settings
{
    internal sealed class SettingsScreen : ScreenBase
    {
        [SerializeField] private NicknameInputBehaviour _nicknameInputBehaviour;
        [SerializeField] private ThemeButton[] _settingsButtons;

        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(IWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        internal override void Init()
        {
            _nicknameInputBehaviour.Init();

            foreach (var button in _settingsButtons)
                _windowsController.BindPopUpOpen(button, PopUpType.Settings);
        }
    }
}
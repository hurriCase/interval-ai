using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Screens.Settings.Behaviours;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Settings
{
    internal sealed class SettingsScreen : ScreenBase
    {
        [SerializeField] private UserBehaviour _userBehaviour;
        [SerializeField] private ButtonComponent[] _settingsButtons;

        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(IWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        internal override void Init()
        {
            _userBehaviour.Init();

            foreach (var button in _settingsButtons)
            {
                button.OnClickAsObservable()
                    .SubscribeAndRegister(this,
                        static self => self._windowsController.OpenPopUpByType(PopUpType.Settings));
            }
        }
    }
}
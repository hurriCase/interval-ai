using R3;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Screens.Settings.Behaviours;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.Screen;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Settings
{
    internal sealed class SettingsScreen : ScreenBase
    {
        [SerializeField] private UserBehaviour _userBehaviour;
        [SerializeField] private ButtonComponent _settingsButton;

        [Inject] private IWindowsController _windowsController;

        internal override void Init()
        {
            _userBehaviour.Init();

            _settingsButton.OnClickAsObservable()
                .Subscribe(this, (_, self)
                    => self._windowsController.OpenPopUpByType(PopUpType.Settings))
                .RegisterTo(destroyCancellationToken);
        }
    }
}
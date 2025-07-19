using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Screens.Settings.Behaviours;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.Settings
{
    internal sealed class SettingsScreen : ScreenBase
    {
        [SerializeField] private UserBehaviour _userBehaviour;

        internal override void Init()
        {
            _userBehaviour.Init();
        }
    }
}
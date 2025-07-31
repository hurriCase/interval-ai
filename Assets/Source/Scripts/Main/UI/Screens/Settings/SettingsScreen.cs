using Source.Scripts.Main.UI.Screens.Settings.Behaviours;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.Screens.Settings
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
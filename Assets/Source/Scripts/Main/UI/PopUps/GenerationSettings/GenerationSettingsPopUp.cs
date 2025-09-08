using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.PopUps.GenerationSettings.Behaviours;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.GenerationSettings
{
    internal sealed class GenerationSettingsPopUp : PopUpBase
    {
        [SerializeField] private LanguageSettingBehaviour _translateFromSwitch;
        [SerializeField] private WordSettingBehaviour _wordSettingBehaviour;

        internal override void Init()
        {
            _translateFromSwitch.LeftThemeToggle.Init(LanguageType.Learning);
            _translateFromSwitch.RightThemeToggle.Init(LanguageType.Native);

            _translateFromSwitch.Init();
            _wordSettingBehaviour.Init();
        }
    }
}
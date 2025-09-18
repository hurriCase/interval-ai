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
            _translateFromSwitch.Init();
            _wordSettingBehaviour.Init();
        }
    }
}
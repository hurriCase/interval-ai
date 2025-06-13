using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Windows.Screens
{
    internal sealed class LearningWordsScreen : WindowBase<ScreenType>
    {
        [SerializeField] private Button _achievementButton;

        internal override void Init()
        {
            _achievementButton.onClick.AddListener(() =>
                WindowsController.Instance.OpenPopUpByType(PopUpType.Achievements));
        }
    }
}
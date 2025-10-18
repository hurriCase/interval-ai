using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Shared.Progress;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours
{
    internal sealed class AchievementsBehaviour : MonoBehaviour
    {
        [SerializeField] private ThemeButton _achievementPopUpButton;
        [SerializeField] private WeekProgressContainer _weekProgressContainer;

        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(IWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        internal void Init()
        {
            _weekProgressContainer.UpdateCurrentWeeklyProgress();

            _windowsController.BindPopUpOpen(_achievementPopUpButton, PopUpType.Achievements);
        }
    }
}
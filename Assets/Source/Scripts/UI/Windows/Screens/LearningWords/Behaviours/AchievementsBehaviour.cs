using R3;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Shared;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours
{
    internal sealed class AchievementsBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _achievementPopUpButton;
        [SerializeField] private WeekProgressContainer _weekProgressContainer;

        [Inject] private IWindowsController _windowsController;

        internal void Init()
        {
            _achievementPopUpButton.OnClickAsObservable()
                .Subscribe(_windowsController, (_, controller) => controller.OpenPopUpByType(PopUpType.Achievements))
                .AddTo(this);

            _weekProgressContainer.UpdateCurrentWeeklyProgress();
        }
    }
}
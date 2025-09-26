using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Shared.Progress;
using Source.Scripts.UI.Components.Button;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours
{
    internal sealed class AchievementsBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _achievementPopUpButton;
        [SerializeField] private WeekProgressContainer _weekProgressContainer;

        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(IWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        internal void Init()
        {
            _achievementPopUpButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.OpenAchievementPopUp());

            _weekProgressContainer.UpdateCurrentWeeklyProgress();
        }

        private void OpenAchievementPopUp()
        {
            _windowsController.OpenPopUpByType(PopUpType.Achievements);
        }
    }
}
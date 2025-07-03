using System.Collections.Generic;
using R3;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Shared;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements
{
    internal sealed class AchievementsBehaviour : MonoBehaviour
    {
        [SerializeField] private List<ProgressItem> _progressItems;
        [SerializeField] private ButtonComponent _achievementPopUpButton;
        [SerializeField] private WeekProgressContainer _weekProgressContainer;

        internal void Init()
        {
            _achievementPopUpButton.Button.OnClickAsObservable()
                .Subscribe(_ => WindowsController.Instance.OpenPopUpByType(PopUpType.Achievements))
                .AddTo(this);

            _weekProgressContainer.UpdateCurrentWeeklyProgress();
        }
    }
}
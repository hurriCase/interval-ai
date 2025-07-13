using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.PopUps.Achievement.Behaviours;
using Source.Scripts.UI.Windows.PopUps.Achievement.Behaviours.LearningStarts;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement
{
    internal sealed class AchievementsPopUp : PopUpBase
    {
        [SerializeField] private AchievementProgressBehaviour _achievementProgressBehaviour;
        [SerializeField] private CalendarBehaviour _calendarBehaviour;
        [SerializeField] private LearningStatsBehaviour _learningStatsBehaviour;

        internal override void Init()
        {
            _achievementProgressBehaviour.Init();
            _calendarBehaviour.Init();
            _learningStatsBehaviour.Init();
        }
    }
}
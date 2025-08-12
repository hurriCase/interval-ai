using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts;
using Source.Scripts.UI.Windows.Base.PopUp;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement
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
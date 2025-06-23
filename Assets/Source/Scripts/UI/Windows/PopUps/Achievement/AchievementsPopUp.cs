using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement
{
    internal sealed class AchievementsPopUp : PopUpBase
    {
        [SerializeField] private AchievementProgressBehaviour _achievementProgressBehaviour;

        internal override void Init()
        {
            _achievementProgressBehaviour.Init();
        }
    }
}
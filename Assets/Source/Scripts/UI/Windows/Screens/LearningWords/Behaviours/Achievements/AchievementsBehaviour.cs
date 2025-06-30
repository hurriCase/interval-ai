using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Shared;
using UnityEngine;
using ZLinq;

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

            var currentWeek = ProgressRepository.Instance.GetCurrentWeek();
            var localizationKeys =
                LocalizationKeysDatabase.Instance.WeekDaysLocalizationKeys.AsValueEnumerable()
                    .Select(localizationKey => localizationKey.GetLocalization()).ToList();
            _weekProgressContainer.UpdateWeeklyProgress(currentWeek, localizationKeys);
        }
    }
}
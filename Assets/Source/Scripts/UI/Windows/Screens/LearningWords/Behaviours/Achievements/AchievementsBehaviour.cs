using System.Collections.Generic;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements
{
    internal sealed class AchievementsBehaviour : MonoBehaviour
    {
        [SerializeField] private List<ProgressItem> _progressItems;
        [SerializeField] private ButtonComponent _achievementPopUpButton;

        internal void Init()
        {
            _achievementPopUpButton.Button.OnClickAsObservable()
                .Subscribe(_ => WindowsController.Instance.OpenPopUpByType(PopUpType.Achievements))
                .AddTo(this);

            var currentWeek = ProgressRepository.Instance.GetCurrentWeek();
            var localizationKeys = LocalizationKeysDatabase.Instance.WeekDaysLocalizationKeys;

            if (_progressItems.Count != currentWeek.Count || localizationKeys.Count != currentWeek.Count)
            {
                Debug.LogError("[AchievementsBehaviour::Init] There is a mismatch between element counts:\n" +
                               $"_progressItems.Count: {_progressItems.Count}\n" +
                               $"currentWeek.Count: {currentWeek.Count}\n" +
                               $"localizationKeys.Count: {localizationKeys.Count}");
                return;
            }

            for (var i = 0; i < currentWeek.Count; i++)
                _progressItems[i].Init(currentWeek[i], localizationKeys[i]);
        }
    }
}
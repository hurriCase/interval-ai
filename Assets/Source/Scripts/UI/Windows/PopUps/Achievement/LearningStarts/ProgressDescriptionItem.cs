using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.Theme.Components;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts
{
    internal sealed class ProgressDescriptionItem : MonoBehaviour
    {
        [field: SerializeField] internal TextMeshProUGUI DescriptionText { get; private set; }
        [field: SerializeField] internal LearningState LearningState { get; private set; }
        [field: SerializeField] internal ImageThemeComponent StateIndicatorImage { get; private set; }

        internal void Init(int[] totalProgress, ProgressColorMapping progressColorMapping)
        {
            var stateIndex = (int)LearningState;
            var localizationKeysDatabase = LocalizationKeysDatabase.Instance;

            DescriptionText.text = string.Format(
                LocalizationController.Localize(localizationKeysDatabase.GetLearningStateLocalization(stateIndex)),
                totalProgress[stateIndex].ToString());

            progressColorMapping.SetComponentForState(LearningState, StateIndicatorImage);
        }
    }
}
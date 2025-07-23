using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.Theme.Components;
using Source.Scripts.Core.Localization;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.Main.Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class ProgressDescriptionItem : MonoBehaviour
    {
        [field: SerializeField] internal TextMeshProUGUI DescriptionText { get; private set; }
        [field: SerializeField] internal ImageThemeComponent StateIndicatorImage { get; private set; }

        internal void Init(LearningState state, int progress, ProgressColorMapping progressColorMapping)
        {
            var localizationKeysDatabase = LocalizationKeysDatabase.Instance;

            DescriptionText.text = string.Format(
                LocalizationController.Localize(localizationKeysDatabase.GetLearningStateLocalization(state)),
                progress.ToString());

            progressColorMapping.SetComponentForState(state, StateIndicatorImage);
        }
    }
}
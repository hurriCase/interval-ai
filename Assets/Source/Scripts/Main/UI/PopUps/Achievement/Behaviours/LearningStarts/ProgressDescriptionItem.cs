using CustomUtils.Runtime.UI.Theme.Components;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class ProgressDescriptionItem : MonoBehaviour
    {
        [field: SerializeField] internal TextMeshProUGUI DescriptionText { get; private set; }
        [field: SerializeField] internal ImageThemeComponent StateIndicatorImage { get; private set; }

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        internal void Init(LearningState state, int progress, ProgressColorMapping progressColorMapping)
        {
            var localization = _localizationKeysDatabase.GetLearningStateLocalization(state);
            DescriptionText.text = string.Format(localization, progress.ToString());

            progressColorMapping.SetComponentForState(state, StateIndicatorImage);
        }
    }
}
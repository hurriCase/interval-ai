using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.Theme;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Shared.Progress;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class ProgressDescriptionItem : MonoBehaviour
    {
        [field: SerializeField] internal TextMeshProUGUI DescriptionText { get; private set; }
        [field: SerializeField] internal ThemeComponent StateIndicatorImage { get; private set; }

        [SerializeField] private EnumArray<LearningState, LocalizationKey> _progressLearningStates = new(EnumMode.SkipFirst);

        internal void Init(LearningState state, int progress, ProgressColorMapping progressColorMapping)
        {
            _progressLearningStates[state].SubscribeToText(DescriptionText, progress);

            progressColorMapping.SetComponentForState(state, StateIndicatorImage);
        }
    }
}
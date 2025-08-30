using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.Theme.Components;
using Cysharp.Text;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Words.Base;
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

        private LearningState _currentLearningState;
        private int _currentProgress;

        private ILocalizationKeysDatabase _localizationKeysDatabase;

        [Inject]
        internal void Inject(ILocalizationKeysDatabase localizationKeysDatabase)
        {
            _localizationKeysDatabase = localizationKeysDatabase;
        }

        internal void Init(LearningState state, int progress, ProgressColorMapping progressColorMapping)
        {
            _currentLearningState = state;
            _currentProgress = progress;

            LocalizationController.Language.SubscribeAndRegister(this, static self => self.UpdateDescriptionText());

            progressColorMapping.SetComponentForState(state, StateIndicatorImage);
        }

        private void UpdateDescriptionText()
        {
            var localization = _localizationKeysDatabase.GetLearningStateLocalization(_currentLearningState);
            DescriptionText.SetTextFormat(localization, _currentProgress);
        }
    }
}
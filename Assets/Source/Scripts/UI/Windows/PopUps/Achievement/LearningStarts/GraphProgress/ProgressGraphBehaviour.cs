using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes;
using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts.GraphProgress
{
    internal sealed class ProgressGraphBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _maxProgressText;

        [SerializeField] private RectTransform _graphButtonsContainer;
        [SerializeField] private ToggleGroup _graphButtonsGroup;
        [SerializeField] private GraphTypeItem _graphTypeItemPrefab;
        [SerializeField] private List<GraphProgressType> _graphProgressTypes;

        [SerializeField] private AspectRatioFitter _spacingPrefab;
        [SerializeField] private float _spacingRatio;

        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private UILineRenderer _uiLineRenderer;

        [SerializeField] private EnumArray<LearningState, UILineRenderer> _graphLines;

        internal void Init()
        {
            foreach (var progressType in _graphProgressTypes)
            {
                var createdGraphType = Instantiate(_graphTypeItemPrefab, _graphButtonsContainer);
                createdGraphType.Toggle.group = _graphButtonsGroup;
                createdGraphType.Toggle.OnValueChangedAsObservable().Subscribe((Behaviour: this, progressType),
                    (_, tuple) => tuple.Behaviour.UpdateGraph(tuple.progressType));

                var createdSpacing = Instantiate(_spacingPrefab, _graphButtonsContainer);
                createdSpacing.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                createdSpacing.aspectRatio = _spacingRatio;

                LocalizationController.Language.Subscribe((behaviour: this, progressType, createdGraphType.Text),
                    (_, tuple) => tuple.behaviour.UpdateLocalization(tuple.progressType, tuple.Text));
            }
        }

        private void UpdateLocalization(GraphProgressType progressType, TMP_Text graphTypeText)
        {
            var localizationKey = LocalizationKeysDatabase.Instance.GetDateLocalization((int)progressType.DateType);
            graphTypeText.text = string.Format(LocalizationController.Localize(localizationKey), progressType.Amount);
        }

        private void UpdateGraph(GraphProgressType progressType)
        {
            foreach (var (learningState, uiLineRenderer) in _graphLines.AsTuples())
            {
                if (learningState == LearningState.None)
                    continue;

                uiLineRenderer.color = _progressColorMapping.GetColorForState(learningState);
                //uiLineRenderer.SetPoints();
            }
        }
    }
}
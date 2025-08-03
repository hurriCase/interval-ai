using R3;
using R3.Triggers;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Onboarding.Data;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.Behaviours
{
    internal sealed class WordCountSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private RectTransform _rowItem;
        [SerializeField] private ButtonTextComponent _wordCountItem;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _rowSpacingRatio;
        [SerializeField] private float _wordCountSpacingRatio;

        [Inject] private IProgressRepository _progressRepository;
        [Inject] private IWordGoalDatabase _wordGoalDatabase;
        [Inject] private IObjectResolver _objectResolver;

        private const int WordCountPerRow = 3;

        internal override void Init()
        {
            CreateWordGoalSelections();
        }

        private void CreateWordGoalSelections()
        {
            var wordGoals = _wordGoalDatabase.DefaultWordGoals;
            var currentRow = new RectTransform();
            for (var i = 0; i < wordGoals.Count; i++)
            {
                if (i % WordCountPerRow == 0)
                {
                    currentRow = _objectResolver.Instantiate(_rowItem, _contentContainer);

                    _spacing.CreateSpacing(_rowSpacingRatio, _contentContainer,
                        AspectRatioFitter.AspectMode.WidthControlsHeight);
                }

                CreateWordItem(currentRow, wordGoals[i]);
            }
        }

        private void CreateWordItem(Transform currentRow, int wordGoal)
        {
            var createdWordItem = _objectResolver.Instantiate(_wordCountItem, currentRow);

            createdWordItem.Text.text = wordGoal.ToString();
            createdWordItem.Button.OnClickAsObservable()
                .Subscribe((_progressRepository, goal: wordGoal),
                    static (_, tuple) => tuple._progressRepository.NewWordsDailyTarget.Value = tuple.goal)
                .RegisterTo(destroyCancellationToken);
        }
    }
}
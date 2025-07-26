using R3;
using R3.Triggers;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data;
using Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.UI.Behaviours
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

        private const int WordCountPerRow = 5;

        internal override void Init()
        {
            var wordGoals = _wordGoalDatabase.DefaultWordGoals;
            var currentRow = new RectTransform();
            for (var i = 0; i < wordGoals.Count; i++)
            {
                if (i % WordCountPerRow == 0)
                {
                    currentRow = Instantiate(_rowItem, _contentContainer);
                    var createRowSpacing = Instantiate(_spacing, _contentContainer);
                    createRowSpacing.aspectRatio = _rowSpacingRatio;
                }

                var createdWordItem = Instantiate(_wordCountItem, currentRow);
                createdWordItem.Text.text = wordGoals[i].ToString();
                createdWordItem.Button.OnCancelAsObservable()
                    .Subscribe((_progressRepository, goal: wordGoals[i]),
                        static (_, tuple) => tuple._progressRepository.DailyWordsGoal.Value = tuple.goal)
                    .RegisterTo(destroyCancellationToken);

                if (i == wordGoals.Count - 1)
                    continue;

                var createdWordSpacing = Instantiate(_spacing, currentRow);
                createdWordSpacing.aspectRatio = _wordCountSpacingRatio;
            }
        }
    }
}
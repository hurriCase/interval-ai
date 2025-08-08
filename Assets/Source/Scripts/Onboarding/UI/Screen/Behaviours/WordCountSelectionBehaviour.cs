using R3;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Onboarding.Data;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.Screen.Behaviours
{
    internal sealed class WordCountSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private RectTransform _rowItem;
        [SerializeField] private ButtonTextComponent _wordCountItem;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _rowSpacingRatio;
        [SerializeField] private float _wordCountSpacingRatio;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private IOnboardingConfig _onboardingConfig;
        [Inject] private IObjectResolver _objectResolver;

        private const int WordCountPerRow = 3;

        internal override void Init()
        {
            CreateWordGoalSelections();
        }

        private void CreateWordGoalSelections()
        {
            var wordGoals = _onboardingConfig.DefaultWordGoals;
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
                .Subscribe((self: this, wordGoal),
                    static (_, tuple) => tuple.self.SelectWordCount(tuple.wordGoal))
                .RegisterTo(destroyCancellationToken);
        }

        private void SelectWordCount(int wordsGoal)
        {
            _settingsRepository.DailyGoal.Value = wordsGoal;
            continueSubject.OnNext(Unit.Default);
        }
    }
}
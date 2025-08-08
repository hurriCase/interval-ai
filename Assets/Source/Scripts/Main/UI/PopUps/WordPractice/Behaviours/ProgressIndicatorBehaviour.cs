using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class ProgressIndicatorBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _previousCardButton;
        [SerializeField] private TextMeshProUGUI _learnedText;

        [Inject] private IWordAdvanceService _wordAdvanceService;
        [Inject] private IProgressRepository _progressRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        public void Init(PracticeState practiceState)
        {
            _wordAdvanceService.CanUndo
                .Subscribe(this, (canUndo, self) => self._previousCardButton.SetActive(canUndo))
                .RegisterTo(destroyCancellationToken);

            _previousCardButton.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) => behaviour._wordAdvanceService.UndoWordAdvance())
                .RegisterTo(destroyCancellationToken);

            _progressRepository.LearnedWordCountObservables[practiceState]
                .Subscribe(this, static (wordsCount, behaviour) => behaviour.UpdateLearnedText(wordsCount))
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateLearnedText(int wordsCount)
        {
            var localizationKey = _localizationKeysDatabase.GetLearnedCountLocalization(wordsCount);
            _learnedText.SetTextFormat(LocalizationController.Localize(localizationKey), wordsCount);
        }
    }
}
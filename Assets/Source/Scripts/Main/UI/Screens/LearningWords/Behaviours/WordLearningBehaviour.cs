using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts;
using Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours
{
    internal sealed class WordLearningBehaviour : MonoBehaviour
    {
        [SerializeField] private ThemeButton _startPracticeButton;

        [SerializeField] private TextMeshProUGUI _learnGoalText;
        [SerializeField] private LocalizationKey _learnGoalKey;
        [SerializeField] private TextMeshProUGUI _repetitionText;
        [SerializeField] private LocalizationKey _repetitionKey;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        private IProgressRepository _progressRepository;
        private IWindowsController _windowsController;
        private IWordsRepository _wordsRepository;

        [Inject]
        internal void Inject(
            IProgressRepository progressRepository,
            IWindowsController windowsController,
            IWordsRepository wordsRepository)
        {
            _progressRepository = progressRepository;
            _windowsController = windowsController;
            _wordsRepository = wordsRepository;
        }

        internal void Init()
        {
            _plusMinusBehaviour.Init();

            _progressRepository.HasDailyTarget.SubscribeToInteractableUntilDestroy(_startPracticeButton);

            _windowsController.BindPopUpOpen(_startPracticeButton, PopUpType.WordPractice);
            _progressRepository.NewWordsDailyTarget.SubscribeToText(_learnGoalKey, _learnGoalText);
            _wordsRepository.SortedWordsByState
                .Select(static wordSets => wordSets[LearningState.Review].Count)
                .SubscribeToText(_repetitionKey, _repetitionText);
        }
    }
}
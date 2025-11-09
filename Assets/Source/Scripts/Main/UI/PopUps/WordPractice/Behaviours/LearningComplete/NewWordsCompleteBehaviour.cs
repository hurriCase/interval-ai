using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.Main.UI.PopUps.Selection.Category;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class NewWordsCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [SerializeField] private ParticleSystem _confettiParticles;
        [SerializeField] private LocalizationKey _categorySelectionTitleKey;

        private CategorySelectionService _categorySelectionService;
        private ICategoriesRepository _categoriesRepository;
        private IProgressRepository _progressRepository;

        [Inject]
        public void Inject(
            CategorySelectionService categorySelectionService,
            ICategoriesRepository categoriesRepository,
            IProgressRepository progressRepository)
        {
            _categorySelectionService = categorySelectionService;
            _categoriesRepository = categoriesRepository;
            _progressRepository = progressRepository;
        }

        protected override void OnInit()
        {
            negativeButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.TryContinueLearning());
            positiveButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.OpenCategorySelection());

            _progressRepository.OnGoalAchieved
                .SubscribeUntilDestroy(this, static (wordsCount, self) => self.ObGoalAchieved(wordsCount));
        }

        private void ObGoalAchieved(int wordsCount)
        {
            SetState(CompleteType.Complete, wordsCount.ToString());

            _confettiParticles.Play();
        }

        protected override void OnCheckCompleteness(CompleteType completeType)
        {
            if (CompleteType.Complete != completeType)
                return;

            var learnedCount = _progressRepository.GetLearnedWordCount(PracticeState.NewWords).ToString();
            SetState(completeType, learnedCount);
        }

        private void OpenCategorySelection()
        {
            _categorySelectionService.UpdateData();

            var selectionPopUp = windowsController.OpenPopUp<SelectionPopUp>();
            selectionPopUp.SetParameters(_categorySelectionService, _categorySelectionTitleKey.GetLocalization());
            selectionPopUp.OnPopUpHidden.SubscribeUntilDestroy(this, static self => self.UpdateCurrentWord());
        }

        private void TryContinueLearning()
        {
            if (_categoriesRepository.TrySelectRandomCategory())
                return;

            if (currentWordService.HasWord())
            {
                practiceStateService.SetState(PracticeState.Review);
                return;
            }

            windowsController.OpenScreenByType(ScreenType.Main);
        }

        private void UpdateCurrentWord()
        {
            currentWordService.UpdateCurrentWord();
        }
    }
}
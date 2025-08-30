using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.Main.UI.PopUps.Selection.Category;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class NewWordsCompleteBehaviour : LearningCompleteBehaviourBase
    {
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
            positiveButton.Button.OnClickAsObservable()
                .SubscribeAndRegister(this, self => self.OpenCategorySelection());
            negativeButton.Button.OnClickAsObservable().SubscribeAndRegister(this, self => self.TryContinueLearning());

            _progressRepository.GoalAchievedObservable.SubscribeAndRegister(this,
                static (wordsCount, self) => self.SetState(CompleteType.Complete, wordsCount.ToString()));
        }

        protected override void OnCheckCompleteness(CompleteType completeType)
        {
            if (CompleteType.Complete != completeType)
                return;

            var learnedCount = _progressRepository.LearnedWordCounts[PracticeState.NewWords].ToString();
            SetState(completeType, learnedCount);
        }

        private void OpenCategorySelection()
        {
            _categorySelectionService.UpdateData();

            var selectionPopUp = windowsController.OpenPopUp<SelectionPopUp>();
            selectionPopUp.SetParameters(_categorySelectionService);
            selectionPopUp.OnHidePopUp.SubscribeAndRegister(this, self => self.UpdateCurrentWord());
        }

        private void TryContinueLearning()
        {
            if (_categoriesRepository.TrySelectRandomCategory())
                return;

            if (currentWordsService.HasWordByState(PracticeState.Review))
            {
                practiceStateService.SetState(PracticeState.Review);
                return;
            }

            windowsController.OpenScreenByType(ScreenType.LearningWords);
        }

        private void UpdateCurrentWord()
        {
            currentWordsService.UpdateCurrentWords();
        }
    }
}
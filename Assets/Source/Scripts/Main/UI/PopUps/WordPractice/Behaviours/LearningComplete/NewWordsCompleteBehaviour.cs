using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class NewWordsCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private IProgressRepository _progressRepository;

        private CategorySelectionService _categorySelectionService;

        protected override void OnInit()
        {
            var localization = localizationKeysDatabase.GetLocalization(LocalizationType.CategorySelectionName);
            _categorySelectionService = new CategorySelectionService(localization);

            _categorySelectionService.SelectedValues.SubscribeAndRegister(this,
                static (selectedValues, self) => self.SetSelectedCategories(selectedValues));

            positiveButton.Button.OnClickAsObservable().SubscribeAndRegister(this, self => self.OpenCategorySelection());
            negativeButton.Button.OnClickAsObservable().SubscribeAndRegister(this, self => self.TryContinueLearning());

            _progressRepository.GoalAchievedObservable.SubscribeAndRegister(this,
                static (wordsCount, self) => self.SetState(CompleteType.Complete, wordsCount.ToString()));
        }

        private void OpenCategorySelection()
        {
            _categorySelectionService.UpdateData(_categoriesRepository.GetUnselectedCategories());

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

        private void SetSelectedCategories(List<int> selectedValues)
        {
            if (selectedValues is { Count: > 0 })
                _categoriesRepository.SetSelectedCategories(selectedValues);
        }

        private void UpdateCurrentWord()
        {
            currentWordsService.UpdateCurrentWords();
        }
    }
}
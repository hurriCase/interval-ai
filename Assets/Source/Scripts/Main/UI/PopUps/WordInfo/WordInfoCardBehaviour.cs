using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordInfo
{
    internal sealed class WordInfoCardBehaviour : MonoBehaviour
    {
        [SerializeField] private ComponentWithSpacing<DescriptiveImageBehaviour> _descriptiveImage;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        [SerializeField] private SwitchablePair<AccordionComponent, RectTransform> _exampleDisplay;
        [SerializeField] private TextMeshProUGUI _learningExampleText;
        [SerializeField] private TextMeshProUGUI _nativeExampleText;
        [SerializeField] private AspectRatioFitter _accordionSpacing;

        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _singleExampleText;
        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _transcriptionText;
        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _learningWordText;
        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _nativeWordText;
        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _categoryNameText;

        [SerializeField] private ButtonComponent _addToCategoryButton;
        [SerializeField] private string _categoryLocalizationKey;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private IWordStateMutator _wordStateMutator;
        [Inject] private IWindowsController _windowsController;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        private CategorySelectionService _categorySelectionService;

        private WordEntry _currentWordEntry;

        internal void Init()
        {
            _wordProgressBehaviour.Init();
            _exampleDisplay.PositiveComponent.Init();

            _categorySelectionService = new CategorySelectionService(_categoryLocalizationKey);

            _addToCategoryButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self.OpenCategorySelection());
        }

        private void OpenCategorySelection()
        {
            _categorySelectionService.UpdateData(
                _categoriesRepository.CategoryEntries.CurrentValue,
                _currentWordEntry.CategoryIds);

            var selectionPopUp = _windowsController.OpenPopUp<SelectionPopUp>();
            selectionPopUp.SetParameters(_categorySelectionService);

            _categorySelectionService.SelectedValues.SubscribeAndRegister(this,
                static (selectedValues, self) => self.SetCategories(selectedValues));
        }

        private void SetCategories(List<int> selectedValues)
        {
            _wordStateMutator.SetCategories(_currentWordEntry, selectedValues);
        }

        internal void UpdateView(WordEntry wordEntry)
        {
            _currentWordEntry = wordEntry;

            _descriptiveImage.Component.UpdateView(wordEntry.DescriptiveImage);
            _descriptiveImage.Toggle(wordEntry.DescriptiveImage.IsValid);

            _wordProgressBehaviour.UpdateProgress(wordEntry);

            UpdateText(_transcriptionText, wordEntry.Transcription, _settingsRepository.IsShowTranscription.Value);
            UpdateText(_learningWordText, wordEntry.LearningWord);
            UpdateText(_nativeWordText, wordEntry.NativeWord);

            var categoryNames = ZString.Join(", ", wordEntry.CategoryIds);
            UpdateText(_categoryNameText, categoryNames);

            UpdateExampleDisplay(wordEntry.LearningExample, wordEntry.NativeExample);
        }

        private void UpdateExampleDisplay(string learningExample, string nativeExample)
        {
            var isLearningValid = learningExample.IsValid();
            var isNativeValid = nativeExample.IsValid();

            var shouldShowAccordion = isLearningValid && isNativeValid;
            var isHideBoth = isLearningValid is false && isNativeValid is false;

            _exampleDisplay.Toggle(shouldShowAccordion, isHideBoth);
            _accordionSpacing.SetActive(shouldShowAccordion);

            var singleExample = isLearningValid ? learningExample : nativeExample;
            UpdateText(_singleExampleText, singleExample, shouldShowAccordion is false);

            if (shouldShowAccordion is false)
                return;

            _learningExampleText.text = learningExample;
            _nativeExampleText.text = nativeExample;
        }

        private void UpdateText(
            ComponentWithSpacing<TextMeshProUGUI> textComponentWithSpacing,
            string textToShow,
            bool additionalRule = true)
        {
            textComponentWithSpacing.Toggle(textToShow.IsValid() && additionalRule);
            textComponentWithSpacing.Component.text = textToShow;
        }
    }
}
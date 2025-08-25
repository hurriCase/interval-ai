using System.Collections.Generic;
using System.Linq;
using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours
{
    internal sealed class WordInfoCardBehaviour : MonoBehaviour
    {
        [SerializeField] private ComponentWithSpacing<DescriptiveImageBehaviour> _descriptiveImage;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _transcriptionText;
        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _learningWordText;
        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _nativeWordText;
        [SerializeField] private ComponentWithSpacing<TextMeshProUGUI> _categoryNameText;
        [SerializeField] private ComponentWithSpacing<RectTransform> _singleExampleContainer;
        [SerializeField] private TextMeshProUGUI _singleExampleText;

        [SerializeField] private ButtonComponent _addToCategoryButton;
        [SerializeField] private string _categoryLocalizationKey;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IUISettingsRepository _uiSettingsRepository;
        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private IWindowsController _windowsController;
        [Inject] private IWordStateMutator _wordStateMutator;

        private CategorySelectionService _categorySelectionService;

        private WordEntry _currentWordEntry;

        internal void Init()
        {
            _wordProgressBehaviour.Init();

            var localization = _localizationKeysDatabase.GetLocalization(LocalizationType.CategorySelectionName);
            _categorySelectionService = new CategorySelectionService(localization);

            _categorySelectionService.SelectedValues.SubscribeAndRegister(this,
                static (selectedValues, self) => self.SetCategories(selectedValues));

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


        }

        private void SetCategories(List<int> selectedValues)
        {
            if (_currentWordEntry != null)
                _wordStateMutator.SetCategories(_currentWordEntry, selectedValues);
        }

        internal void UpdateView(WordEntry wordEntry)
        {
            _currentWordEntry = wordEntry;

            _descriptiveImage.Component.UpdateView(wordEntry.DescriptiveImage);
            _descriptiveImage.Toggle(wordEntry.DescriptiveImage.IsValid);

            _wordProgressBehaviour.UpdateProgress(wordEntry);

            UpdateText(_transcriptionText, wordEntry.Transcription, _uiSettingsRepository.IsShowTranscription.Value);
            UpdateText(_learningWordText, wordEntry.Word.Learning);
            UpdateText(_nativeWordText, ZString.Join(", ", wordEntry.Word.Natives));

            UpdateCategoryName(wordEntry);

            var firstExample = wordEntry.Examples?.FirstOrDefault().Learning;
            _singleExampleText.text = firstExample;
            _singleExampleContainer.Toggle(firstExample.IsValid());
        }

        private void UpdateCategoryName(WordEntry wordEntry)
        {
            using var builder = ZString.CreateStringBuilder();

            for (var i = 0; i < wordEntry.CategoryIds.Count; i++)
            {
                if (i > 0)
                    builder.Append(", ");

                builder.Append(_categoriesRepository.GetCategoryName(wordEntry.CategoryIds[i]));
            }

            UpdateText(_categoryNameText, builder.ToString());
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
using System.Linq;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.Main.UI.PopUps.Selection.Category;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours
{
    internal sealed class WordInfoCardBehaviour : MonoBehaviour
    {
        [SerializeField] private DescriptiveImageBehaviour _descriptiveImage;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        [SerializeField] private TextMeshProUGUI _transcriptionText;
        [SerializeField] private TextMeshProUGUI _learningWordText;
        [SerializeField] private TextMeshProUGUI _nativeWordText;
        [SerializeField] private TextMeshProUGUI _categoryNameText;
        [SerializeField] private TextMeshProUGUI _singleExampleText;

        [SerializeField] private ButtonComponent _addToCategoryButton;

        private WordCategorySelectionService _wordCategorySelectionService;
        private ICategoriesRepository _categoriesRepository;
        private IUISettingsRepository _uiSettingsRepository;
        private IWindowsController _windowsController;

        private WordEntry _currentWordEntry;

        [Inject]
        internal void Inject(
            WordCategorySelectionService wordCategorySelectionService,
            IUISettingsRepository uiSettingsRepository,
            ICategoriesRepository categoriesRepository,
            IWindowsController windowsController)
        {
            _wordCategorySelectionService = wordCategorySelectionService;
            _categoriesRepository = categoriesRepository;
            _uiSettingsRepository = uiSettingsRepository;
            _windowsController = windowsController;
        }

        internal void Init()
        {
            _wordProgressBehaviour.Init();

            _addToCategoryButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.ShowCategorySelection());
        }

        private void ShowCategorySelection()
        {
            _wordCategorySelectionService.UpdateWord(_currentWordEntry);
            _wordCategorySelectionService.UpdateData();

            var selectionPopUp = _windowsController.OpenPopUp<SelectionPopUp>();
            selectionPopUp.SetParameters(_wordCategorySelectionService);
        }

        internal void UpdateView(WordEntry wordEntry)
        {
            _currentWordEntry = wordEntry;

            _descriptiveImage.UpdateView(wordEntry.DescriptiveImage);
            _descriptiveImage.SetActive(wordEntry.DescriptiveImage.IsValid);

            _wordProgressBehaviour.UpdateProgress(wordEntry);

            UpdateText(_transcriptionText, wordEntry.Transcription, _uiSettingsRepository.IsShowTranscription.Value);
            UpdateText(_learningWordText, wordEntry.Word.Learning);
            UpdateText(_nativeWordText, ZString.Join(", ", wordEntry.Word.Natives));

            UpdateCategoryName(wordEntry);

            var firstExample = wordEntry.Examples?.FirstOrDefault().Learning;
            UpdateText(_singleExampleText, firstExample);
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

        private void UpdateText(TMP_Text textComponentWithSpacing, string textToShow, bool additionalRule = true)
        {
            textComponentWithSpacing.SetActive(string.IsNullOrEmpty(textToShow) is false && additionalRule);
            textComponentWithSpacing.text = textToShow;
        }
    }
}
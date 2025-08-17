using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordInfo
{
    internal sealed class WordInfoCardBehaviour : MonoBehaviour
    {
        [SerializeField] private DescriptiveImageBehaviour _descriptiveImage;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private TextMeshProUGUI _transcriptionText;
        [SerializeField] private TextMeshProUGUI _learningWordText;
        [SerializeField] private TextMeshProUGUI _nativeWordText;
        [SerializeField] private TextMeshProUGUI _categoryNameText;
        [SerializeField] private TextMeshProUGUI _learningExampleText;
        [SerializeField] private TextMeshProUGUI _nativeExampleText;
        [SerializeField] private ButtonComponent _addToCategoryButton;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ICategoriesRepository _categoriesRepository;

        internal void Init()
        {
            _wordProgressBehaviour.Init();
        }

        internal void UpdateView(WordEntry wordEntry)
        {
            _descriptiveImage.UpdateView(wordEntry.DescriptiveImage);
            _wordProgressBehaviour.UpdateProgress(wordEntry);

            UpdateText(_transcriptionText, wordEntry.Transcription, _settingsRepository.IsShowTranscription.Value);
            UpdateText(_learningWordText, wordEntry.LearningWord);
            UpdateText(_nativeWordText, wordEntry.NativeWord);
            UpdateText(_categoryNameText, _categoriesRepository.GetCategoryName(wordEntry.CategoryId));
            UpdateText(_learningExampleText, wordEntry.LearningExample);
            UpdateText(_nativeExampleText, wordEntry.NativeExample);
        }

        private void UpdateText(TMP_Text textComponent, string textToShow, bool additionalRule = true)
        {
            textComponent.SetActive(string.IsNullOrWhiteSpace(textToShow) is false && additionalRule);
            textComponent.text = textToShow;
        }
    }
}
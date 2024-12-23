using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core.MVC.Word
{
    internal class WordItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wordCategoryText;
        [SerializeField] private TextMeshProUGUI _nativeWordText;
        [SerializeField] private TextMeshProUGUI _learningWordText;
        [SerializeField] private TextMeshProUGUI _transcriptionText;
        [SerializeField] private TextMeshProUGUI _examplesText;
        [SerializeField] private Button _deleteButton;

        private WordController _controller;
        private WordModel _model;

        public void Init(WordModel model, WordController controller)
        {
            _model = model;
            _controller = controller;

            UpdateUI();
            SetupListeners();
        }

        private void OnDestroy() => _deleteButton.onClick.RemoveAllListeners();

        private void UpdateUI()
        {
            _wordCategoryText.text = _model.CategoryTitle;
            _nativeWordText.text = _model.NativeWord;
            _learningWordText.text = _model.LearningWord;
            _transcriptionText.text = _model.Transcription;
            _examplesText.text = _model.Examples[0].ToString();
        }

        private void SetupListeners() => _deleteButton.onClick.AddListener(DeleteEntry);

        private async void DeleteEntry()
        {
            try
            {
                await _controller.DeleteEntry(_model.Data.Id);
            }
            catch (Exception e)
            {
                Debug.LogError($"[WordItemView::DeleteEntry] Entity deletion failed: {e.Message}");
            }
        }
    }
}
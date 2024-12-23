using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core.MVC.Progress
{
    internal class ProgressItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _repetitionStageText;
        [SerializeField] private TextMeshProUGUI _totalReviewsText;
        [SerializeField] private TextMeshProUGUI _correctReviewsText;
        [SerializeField] private TextMeshProUGUI _lastReviewDateText;
        [SerializeField] private TextMeshProUGUI _nextReviewDateText;
        [SerializeField] private Button _deleteButton;

        private ProgressController _controller;
        private ProgressModel _model;

        internal void Init(ProgressModel model, ProgressController controller)
        {
            _model = model;
            _controller = controller;
            UpdateUI();

            SetupListeners();
        }

        private void OnDestroy() => _deleteButton.onClick.RemoveAllListeners();

        private void UpdateUI()
        {
            _repetitionStageText.text = _model.RepetitionStage.ToString();
            _totalReviewsText.text = _model.TotalReviews.ToString();
            _correctReviewsText.text = _model.CorrectReviews.ToString();
            _lastReviewDateText.text = _model.LastReviewDate.ToString();
            _nextReviewDateText.text = _model.NextReviewDate.ToString();
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
                Debug.LogError($"[ProgressItemView::DeleteEntry] Entity deletion failed: {e.Message}");
            }
        }
    }
}
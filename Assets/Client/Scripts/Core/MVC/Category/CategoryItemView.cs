using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core.MVC.Category
{
    internal sealed class CategoryItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _wordCountText;
        [SerializeField] private Button _deleteButton;

        private CategoryController _controller;
        private CategoryModel _model;

        internal void Init(CategoryModel model, CategoryController controller)
        {
            _model = model;
            _controller = controller;

            UpdateUI();
            SetupListeners();
        }

        private void OnDestroy() => _deleteButton.onClick.RemoveAllListeners();

        private void UpdateUI()
        {
            _titleText.text = _model.Title;
            _descriptionText.text = _model.Description;
            _wordCountText.text = _model.WordCount.ToString();
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
                Debug.LogError($"[CategoryItemView::DeleteEntry] Entity deletion failed: {e.Message}");
            }
        }
    }
}
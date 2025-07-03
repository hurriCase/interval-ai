using CustomUtils.Runtime.Extensions;
using Source.Scripts.Data.Repositories.Vocabulary;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.CategoryPreview
{
    internal sealed class CategoryPreviewBehaviour : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private CategoryPreviewItem _categoryItemPrefab;
        [SerializeField] private AspectRatioFitter _spacingPrefab;
        [SerializeField] private float _spacingRatio;

        internal void Init()
        {
            foreach (var categoryEntry in VocabularyRepository.Instance.CategoryEntries.Value)
            {
                var createdCategory = Instantiate(_categoryItemPrefab, _contentContainer);

                createdCategory.IconImage.sprite = categoryEntry.Icon;
                createdCategory.NameText.text = categoryEntry.LocalizationKey.GetLocalization();

                var createdSpacing = Instantiate(_spacingPrefab, _contentContainer);
                createdSpacing.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                createdSpacing.aspectRatio = _spacingRatio;
            }
        }
    }
}
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.RatioLayout;
using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.CategoryPreview
{
    internal sealed class CategoryPreviewBehaviour : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private CategoryPreviewItem _categoryItemPrefab;
        [SerializeField] private RatioLayoutElement _spacingPrefab;
        [SerializeField] private float _spacingRatio;

        internal void Init()
        {
            foreach (var categoryEntry in UserData.Instance.CategoryEntries.Value)
            {
                var createdCategory = Instantiate(_categoryItemPrefab, _contentContainer);

                createdCategory.IconImage.sprite = categoryEntry.Icon;
                createdCategory.NameText.text = categoryEntry.LocalizationKey.GetLocalization();

                var createdSpacing = Instantiate(_spacingPrefab, _contentContainer);
                createdSpacing.AspectRatio = _spacingRatio;
            }
        }
    }
}
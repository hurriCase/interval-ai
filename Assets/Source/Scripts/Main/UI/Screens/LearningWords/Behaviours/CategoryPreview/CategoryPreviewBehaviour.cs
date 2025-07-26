using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Loader;
using Source.Scripts.Data.Repositories.Categories.Base;
using Source.Scripts.Data.Repositories.Categories.Entries;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview
{
    internal sealed class CategoryPreviewBehaviour : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private CategoryPreviewItem _categoryItemPrefab;
        [SerializeField] private AspectRatioFitter _spacingPrefab;
        [SerializeField] private float _spacingRatio;

        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private IAddressablesLoader _addressablesLoader;

        internal void Init()
        {
            foreach (var categoryEntry in _categoriesRepository.CategoryEntries.Value)
            {
                var createdCategory = Instantiate(_categoryItemPrefab, _contentContainer);

                SetCategoryIcon(createdCategory, categoryEntry).Forget();
                createdCategory.NameText.text = categoryEntry.LocalizationKey.GetLocalization();

                var createdSpacing = Instantiate(_spacingPrefab, _contentContainer);
                createdSpacing.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                createdSpacing.aspectRatio = _spacingRatio;
            }
        }

        private async UniTask SetCategoryIcon(CategoryPreviewItem categoryItem, CategoryEntry categoryEntry)
        {
            if (categoryEntry.Icon.IsValid is false)
                return;

            categoryItem.IconImage.sprite =
                await _addressablesLoader.LoadAsync<Sprite>(categoryEntry.Icon.AssetGUID, destroyCancellationToken);
        }
    }
}
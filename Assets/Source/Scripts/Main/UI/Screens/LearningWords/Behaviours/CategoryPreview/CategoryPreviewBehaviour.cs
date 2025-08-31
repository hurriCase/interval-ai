using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Category;
using Source.Scripts.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview
{
    internal sealed class CategoryPreviewBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _allCategoriesButton;

        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private CategoryPreviewItem _categoryItemPrefab;
        [SerializeField] private AspectRatioFitter _spacingPrefab;
        [SerializeField] private float _spacingRatio;

        private ICategoriesRepository _categoriesRepository;
        private IAddressablesLoader _addressablesLoader;
        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(
            ICategoriesRepository categoriesRepository,
            IAddressablesLoader addressablesLoader,
            IWindowsController windowsController)
        {
            _categoriesRepository = categoriesRepository;
            _addressablesLoader = addressablesLoader;
            _windowsController = windowsController;
        }

        internal void Init()
        {
            _allCategoriesButton.OnClickAsObservable().SubscribeAndRegister(this,
                static self => self._windowsController.OpenScreenByType(ScreenType.Categories));

            CreateCategoryItems();
        }

        private void CreateCategoryItems()
        {
            foreach (var categoryEntry in _categoriesRepository.CategoryEntries.CurrentValue.Values)
            {
                var createdCategory = Instantiate(_categoryItemPrefab, _contentContainer);

                SetCategoryIcon(createdCategory, categoryEntry).Forget();
                createdCategory.Button.Text.text = categoryEntry.LocalizationKey.GetLocalization();
                createdCategory.Button.OnClickAsObservable()
                    .SubscribeAndRegister(this, categoryEntry, static (categoryEntry, self) =>
                    {
                        var categoryPopUp = self._windowsController.OpenPopUp<CategoryPopUp>();
                        categoryPopUp.SetParameters(categoryEntry);
                    });

                _spacingPrefab.CreateHeightSpacing(_spacingRatio, _contentContainer);
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
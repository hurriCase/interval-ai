using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Category;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview
{
    internal sealed class CategoryPreviewBehaviour : MonoBehaviour
    {
        [SerializeField] private ThemeButton _allCategoriesButton;

        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private ThemeButton _categoryButton;

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
            _windowsController.BindScreenOpen(_allCategoriesButton, ScreenType.Categories);

            CreateCategoryItems();
        }

        private void CreateCategoryItems()
        {
            foreach (var category in _categoriesRepository.CategoryEntries.CurrentValue.Values)
            {
                var categoryButton = Instantiate(_categoryButton, _contentContainer);

                categoryButton.Text.text = category.Name;
                categoryButton.OnClickAsObservable().SubscribeUntilDestroy(this, category,
                    static (categoryEntry, self) => self.OpenCategoryPopUp(categoryEntry));

                _addressablesLoader.AssignImageAsync(categoryButton.Image, category.Icon, destroyCancellationToken);
            }
        }

        private void OpenCategoryPopUp(CategoryEntry categoryEntry)
        {
            var categoryPopUp = _windowsController.OpenPopUp<CategoryPopUp>();
            categoryPopUp.SetParameters(categoryEntry);
        }
    }
}
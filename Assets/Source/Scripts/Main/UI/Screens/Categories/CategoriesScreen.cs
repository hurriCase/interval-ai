using System;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZLinq;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoriesScreen : ScreenBase
    {
        [SerializeField] private ThemeButton _addCategoryButton;
        [SerializeField] private TMP_InputField _searchBar;

        [SerializeField] private CategoryContainerBehaviour _categoryContainer;
        [SerializeField] private RectTransform _container;

        private IWindowsController _windowsController;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IWindowsController windowsController, IObjectResolver objectResolver)
        {
            _windowsController = windowsController;
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            _windowsController.BindPopUpOpen(_addCategoryButton, PopUpType.CategoryCreation);
            _windowsController.BindPopUpOpen(_searchBar, PopUpType.Search);

            foreach (var categoryType in Enum.GetValues(typeof(CategoryType)).OfType<CategoryType>())
            {
                var categoryContainer = _objectResolver.Instantiate(_categoryContainer, _container);
                categoryContainer.Init(categoryType);
            }
        }
    }
}
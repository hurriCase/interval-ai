using System;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZLinq;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoriesScreen : ScreenBase
    {
        [SerializeField] private CategoryContainerBehaviour _categoryContainer;
        [SerializeField] private RectTransform _container;

        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            foreach (var categoryType in Enum.GetValues(typeof(CategoryType)).OfType<CategoryType>())
            {
                var categoryContainer = _objectResolver.Instantiate(_categoryContainer, _container);
                categoryContainer.Init(categoryType);
            }
        }
    }
}
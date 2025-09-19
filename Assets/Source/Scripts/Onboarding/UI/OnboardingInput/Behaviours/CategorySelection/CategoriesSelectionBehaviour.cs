using Source.Scripts.Core.Repositories.Categories.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.CategorySelection
{
    internal sealed class CategoriesSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private RectTransform _categoriesContainer;
        [SerializeField] private OnboardingCategoryEntryItem _categoryEntryItem;

        private ICategoriesRepository _categoriesRepository;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(ICategoriesRepository categoriesRepository, IObjectResolver objectResolver)
        {
            _categoriesRepository = categoriesRepository;
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            foreach (var categoryEntry in _categoriesRepository.CategoryEntries.CurrentValue.Values)
            {
                if (categoryEntry.CategoryType != CategoryType.Default)
                    continue;

                var categoryEntryItem = _objectResolver.Instantiate(_categoryEntryItem, _categoriesContainer);
                categoryEntryItem.Init(categoryEntry);
            }
        }
    }
}
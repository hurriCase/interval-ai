using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Main.UI.Screens.Categories;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours
{
    internal sealed class CategoriesSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private RectTransform _categoriesContainer;
        [SerializeField] private CategoryEntryItem _categoryEntryItem;

        private ICategoriesRepository _categoriesRepository;

        [Inject]
        internal void Inject(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        internal override void Init()
        {
            foreach (var categoryEntry in _categoriesRepository.CategoryEntries.CurrentValue.Values)
            {
                if (categoryEntry.CategoryType != CategoryType.Default)
                    continue;

                var categoryEntryItem = Instantiate(_categoryEntryItem, _categoriesContainer);
                categoryEntryItem.Init(categoryEntry);
            }
        }
    }
}
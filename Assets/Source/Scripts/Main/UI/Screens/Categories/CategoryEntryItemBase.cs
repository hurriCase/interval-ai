using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using R3;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal abstract class CategoryEntryItemBase : View<CategoryEntry>
    {
        [SerializeField] protected TextMeshProUGUI categoryNameText;
        [SerializeField] protected TextMeshProUGUI progressText;
        [SerializeField] protected Image icon;
        [SerializeField] protected Image categoryOpenArea;
        [SerializeField] protected StateToggle selectedCheckbox;

        [Inject] protected ICategoryStateMutator categoryStateMutator;
        [Inject] protected IAddressablesLoader addressablesLoader;

        protected CategoryEntry currentCategoryEntry;

        internal override void Init(CategoryEntry categoryEntry)
        {
            UpdateView(categoryEntry);

            selectedCheckbox.OnValueChangedAsObservable()
                .SubscribeUntilDestroy(this, static (isOn, self) => self.currentCategoryEntry.IsSelected = isOn);

            categoryStateMutator.OnCategoryNameChanged
                .Select(this, static (changedCategory, self) => changedCategory == self.currentCategoryEntry)
                .SubscribeUntilDestroy(this, static self => self.UpdateName());

            OnInit();
        }

        internal override void UpdateView(CategoryEntry categoryEntry)
        {
            currentCategoryEntry = categoryEntry;

            addressablesLoader.AssignImageAsync(icon, categoryEntry.Icon, destroyCancellationToken);

            categoryNameText.text = categoryEntry.Name;
            progressText.text = categoryEntry.WordEntries.Count.ToString();
            selectedCheckbox.isOn = categoryEntry.IsSelected;
        }

        protected abstract void OnInit();

        private void UpdateName()
        {
            categoryNameText.text = currentCategoryEntry.Name;
        }
    }
}
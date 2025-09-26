using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal abstract class CategoryEntryItemBase : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI categoryNameText;
        [SerializeField] protected TextMeshProUGUI progressText;
        [SerializeField] protected Image icon;
        [SerializeField] protected Image categoryOpenArea;
        [SerializeField] protected ToggleComponent selectedCheckbox;

        [Inject] protected ICategoryStateMutator categoryStateMutator;
        [Inject] protected IAddressablesLoader addressablesLoader;

        protected CategoryEntry currentCategoryEntry;

        internal void Init(CategoryEntry categoryEntry)
        {
            addressablesLoader.AssignImageAsync(icon, categoryEntry.Icon, destroyCancellationToken);

            currentCategoryEntry = categoryEntry;
            categoryNameText.text = categoryEntry.LocalizationKey.GetLocalization();
            progressText.text = categoryEntry.WordEntries.Count.ToString();
            selectedCheckbox.isOn = categoryEntry.IsSelected;

            selectedCheckbox.OnValueChangedAsObservable()
                .SubscribeUntilDestroy(this, static (isOn, self) => self.currentCategoryEntry.IsSelected = isOn);

            categoryStateMutator.OnCategoryNameChanged
                .Select(categoryEntry, (currentCategory, changedCategory) => currentCategory == changedCategory)
                .SubscribeUntilDestroy(this, static self => self.UpdateName());

            OnInit();
        }

        protected abstract void OnInit();

        private void UpdateName()
        {
            categoryNameText.text = currentCategoryEntry.LocalizationKey.GetLocalization();
        }
    }
}
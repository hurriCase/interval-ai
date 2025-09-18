using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Category;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoryEntryItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _categoryName;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Image _icon;

        [SerializeField] private Image _categoryOpenArea;
        [SerializeField] private ToggleComponent _selectedCheckbox;

        private IAddressablesLoader _addressablesLoader;
        private IWindowsController _windowsController;

        private CategoryEntry _currentCategoryEntry;

        [Inject]
        internal void Inject(IAddressablesLoader addressablesLoader, IWindowsController windowsController)
        {
            _addressablesLoader = addressablesLoader;
            _windowsController = windowsController;
        }

        internal void Init(CategoryEntry categoryEntry)
        {
            _addressablesLoader.AssignImageAsync(_icon, categoryEntry.Icon, destroyCancellationToken);

            _currentCategoryEntry = categoryEntry;

            _categoryName.text = categoryEntry.LocalizationKey.GetLocalization();
            _progressText.text = categoryEntry.WordEntries.Count.ToString();
            _selectedCheckbox.isOn = categoryEntry.IsSelected;

            _categoryOpenArea.OnPointerClickAsObservable()
                .Subscribe(this, (_, self) => self.OpenCategoryPopUp())
                .RegisterTo(destroyCancellationToken);

            _selectedCheckbox.OnPointerClickAsObservable()
                .Subscribe(this, (_, self) =>
                    self._currentCategoryEntry.IsSelected = self._selectedCheckbox.isOn)
                .RegisterTo(destroyCancellationToken);
        }

        private void OpenCategoryPopUp()
        {
            var selectionPopUp = _windowsController.OpenPopUp<CategoryPopUp>();
            selectionPopUp.SetParameters(_currentCategoryEntry);
        }

        internal void UpdateName()
        {
            _categoryName.text = _currentCategoryEntry.LocalizationKey.GetLocalization();
        }
    }
}
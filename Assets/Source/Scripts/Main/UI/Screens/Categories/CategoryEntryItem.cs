using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Base;
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
        [SerializeField] private CheckboxComponent _selectedCheckbox;

        [Inject] private IWindowsController _windowsController;
        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private IAddressablesLoader _addressablesLoader;

        private CategoryEntry _categoryEntry;

        internal void Init(CategoryEntry categoryEntry)
        {
            //SetCategoryIcon(categoryEntry.Icon.AssetGUID).Forget();

            _categoryEntry = categoryEntry;

            _categoryName.text = categoryEntry.LocalizationKey.GetLocalization();
            _progressText.text = categoryEntry.WordEntries.Count.ToString();
            _selectedCheckbox.isOn = categoryEntry.IsSelected;

            _categoryOpenArea.OnPointerClickAsObservable()
                .Subscribe( this, (_, self) =>
                    self._windowsController.OpenPopUpByType(PopUpType.Category, self._categoryEntry))
                .RegisterTo(destroyCancellationToken);

            _selectedCheckbox.OnPointerClickAsObservable()
                .Subscribe(this, (_, self) =>
                    self._categoryEntry.IsSelected = self._selectedCheckbox.isOn)
                .RegisterTo(destroyCancellationToken);
        }

        internal void UpdateName()
        {
            _categoryName.text = _categoryEntry.LocalizationKey.GetLocalization();
        }

        private async UniTask SetCategoryIcon(string assetGUID)
        {
            _icon.sprite = await _addressablesLoader.LoadAsync<Sprite>(assetGUID, destroyCancellationToken);
        }
    }
}
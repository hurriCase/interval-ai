using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.DI.Repositories.Categories;
using Source.Scripts.Core.Loader;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoryEntryItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Image _icon;
        [SerializeField] private CheckboxComponent _selectedCheckbox;

        [Inject] private IAddressablesLoader _addressablesLoader;

        internal void Init(CategoryEntry categoryEntry)
        {
            SetCategoryIcon(categoryEntry.Icon.AssetGUID).Forget();

            _titleText.text = categoryEntry.LocalizationKey.GetLocalization();
            _progressText.text = categoryEntry.WordEntries.Count.ToString();
        }

        private async UniTask SetCategoryIcon(string assetGUID)
        {
            _icon.sprite = await _addressablesLoader.LoadAsync<Sprite>(assetGUID, destroyCancellationToken);
        }
    }
}
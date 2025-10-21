using System.Collections.Generic;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Category;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.CategoryCreation
{
    internal sealed class CategoryCreationPopUp : PopUpBase
    {
        [SerializeField] private TMP_InputField _categoryNameInputField;
        [SerializeField] private ThemeButton _saveButton;
        [SerializeField] private RectTransform _iconsContainer;
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private StateToggle _iconItem;
        [SerializeField] private float _inactiveAlpha;

        private readonly List<StateToggle> _createdIconItems = new();
        private bool _wasSubscribed;

        private AssetReferenceSprite _selectedIcon;

        private INotificationComponent _notificationComponent;
        private ICategoriesRepository _categoriesRepository;
        private IAddressablesLoader _addressablesLoader;
        private IWindowsController _windowsController;
        private ISpriteReferences _spriteReferences;

        [Inject]
        public void Inject(
            INotificationComponent notificationComponent,
            ICategoriesRepository categoriesRepository,
            IAddressablesLoader addressablesLoader,
            IWindowsController windowsController,
            ISpriteReferences spriteReferences)
        {
            _notificationComponent = notificationComponent;
            _categoriesRepository = categoriesRepository;
            _addressablesLoader = addressablesLoader;
            _windowsController = windowsController;
            _spriteReferences = spriteReferences;
        }

        internal override void Init()
        {
            _saveButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.CreateCategory());

            foreach (var iconReference in _spriteReferences.CategoryIcons)
            {
                var createdItem = Instantiate(_iconItem, _iconsContainer);

                _addressablesLoader.AssignImageAsync(createdItem.Image, iconReference, destroyCancellationToken);
                createdItem.group = _toggleGroup;
                createdItem.OnPointerClickAsObservable()
                    .Do(this, iconReference, static (iconReference, self) => self._selectedIcon = iconReference)
                    .Take(1)
                    .Where(this, static self => self._wasSubscribed is false)
                    .SubscribeUntilDestroy(this, static self => self.SubscribeOnValueChanged());

                _createdIconItems.Add(createdItem);
            }
        }

        private void SubscribeOnValueChanged()
        {
            foreach (var createdIconItem in _createdIconItems)
            {
                createdIconItem.OnValueChangedAsObservable()
                    .SubscribeUntilDestroy(this, createdIconItem.Image,
                        static (isOn, self, image) => self.OnValueChanged(isOn, image));
            }

            _wasSubscribed = true;
        }

        internal override UniTask HideAsync()
        {
            _categoryNameInputField.text = string.Empty;
            _selectedIcon = null;

            return base.HideAsync();
        }

        private void OnValueChanged(bool isOn, Graphic image)
        {
            image.SetAlpha(isOn ? 1f : _inactiveAlpha);
        }

        private void CreateCategory()
        {
            if (string.IsNullOrWhiteSpace(_categoryNameInputField.text))
            {
                _notificationComponent.ShowMessage("Please fill all fields");
                return;
            }

            if (_selectedIcon is null)
            {
                _notificationComponent.ShowMessage("Please select icon");
                return;
            }

            var newCategory = _categoriesRepository.CreateCategory(_categoryNameInputField.text, _selectedIcon);
            var categoryPopUp = _windowsController.OpenPopUp<CategoryPopUp>();
            categoryPopUp.SetParameters(newCategory);

            HideAsync();
        }
    }
}
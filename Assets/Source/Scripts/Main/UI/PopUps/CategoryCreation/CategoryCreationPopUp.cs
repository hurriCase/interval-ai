using System;
using System.Collections.Generic;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
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
        [SerializeField] private ThemeButton _createButton;

        [SerializeField] private TMP_InputField _categoryNameInputField;

        [SerializeField] private RectTransform _iconsContainer;
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private StateToggle _iconItem;

        [SerializeField] private float _inactiveAlpha;
        [SerializeField] private LocalizationKey _textRequiredLocalizationKey;
        [SerializeField] private LocalizationKey _iconRequiredLocalizationKey;

        private readonly List<StateToggle> _iconToggles = new();

        private AssetReferenceSprite _selectedIcon;
        private bool _hasUserInteractedWithIcons;

        private IDisposable _alphaSubscriptions;

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
            _createButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.TryCreateCategory());

            CreateIconToggles();
        }

        internal override UniTask ShowAsync()
        {
            foreach (var toggle in _iconToggles)
                toggle.Image.SetAlpha(1f);

            return base.ShowAsync();
        }

        internal override UniTask HideAsync()
        {
            _categoryNameInputField.text = string.Empty;
            _selectedIcon = null;
            _hasUserInteractedWithIcons = false;
            _alphaSubscriptions?.Dispose();

            return base.HideAsync();
        }

        private void CreateIconToggles()
        {
            foreach (var iconReference in _spriteReferences.CategoryIcons)
            {
                var toggle = Instantiate(_iconItem, _iconsContainer);
                toggle.group = _toggleGroup;

                _addressablesLoader.AssignImageAsync(toggle.Image, iconReference, destroyCancellationToken);
                _iconToggles.Add(toggle);

                toggle.OnPointerClickAsObservable()
                    .SubscribeUntilDestroy(this, iconReference,
                        static (iconReference, self) => self.HandleIconClick(iconReference));
            }
        }

        private void HandleIconClick(AssetReferenceSprite iconReference)
        {
            _selectedIcon = iconReference;

            if (_hasUserInteractedWithIcons)
                return;

            _hasUserInteractedWithIcons = true;
            EnableAlphaChangeOnToggle();
        }

        private void EnableAlphaChangeOnToggle()
        {
            var builder = Disposable.CreateBuilder();

            foreach (var toggle in _iconToggles)
            {
                var currentAlpha = toggle.isOn ? 1f : _inactiveAlpha;
                toggle.Image.SetAlpha(currentAlpha);

                toggle.OnValueChangedAsObservable()
                    .Subscribe((toggle.Image, _inactiveAlpha),
                        static (isOn, tuple) => tuple.Image.SetAlpha(isOn ? 1f : tuple._inactiveAlpha))
                    .AddTo(ref builder)
                    .RegisterTo(destroyCancellationToken);
            }

            _alphaSubscriptions = builder.Build();
        }

        private void TryCreateCategory()
        {
            if (ValidateInput() is false)
                return;

            var newCategory = _categoriesRepository.CreateCategory(_categoryNameInputField.text, _selectedIcon);
            var categoryPopUp = _windowsController.OpenPopUp<CategoryPopUp>();
            categoryPopUp.SetParameters(newCategory);

            HideAsync();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(_categoryNameInputField.text))
            {
                _notificationComponent.ShowMessage(_textRequiredLocalizationKey.GetLocalization());
                return false;
            }

            if (_selectedIcon is not null)
                return true;

            _notificationComponent.ShowMessage(_iconRequiredLocalizationKey.GetLocalization());
            return false;
        }
    }
}
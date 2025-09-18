using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Category;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.CategoryCreation
{
    internal sealed class CategoryCreationPopUp : PopUpBase
    {
        [SerializeField] private TMP_InputField _categoryNameInputField;
        [SerializeField] private ButtonComponent _saveButton;

        private IWindowsController _windowsController;
        private ICategoriesRepository _categoriesRepository;

        [Inject]
        public void Inject(IWindowsController windowsController, ICategoriesRepository categoriesRepository)
        {
            _windowsController = windowsController;
            _categoriesRepository = categoriesRepository;
        }

        internal override void Init()
        {
            _saveButton.OnClickAsObservable().SubscribeAndRegister(this, static self => self.CreateCategory());
        }

        private void CreateCategory()
        {
            if (string.IsNullOrWhiteSpace(_categoryNameInputField.text))
                return;

            var newCategory = _categoriesRepository.CreateCategory(_categoryNameInputField.text);
            var categoryPopUp = _windowsController.OpenPopUp<CategoryPopUp>();
            categoryPopUp.SetParameters(newCategory);
        }
    }
}
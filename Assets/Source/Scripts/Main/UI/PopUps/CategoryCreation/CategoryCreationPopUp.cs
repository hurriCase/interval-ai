using R3;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.CategoryCreation
{
    internal sealed class CategoryCreationPopUp : PopUpBase
    {
        [SerializeField] private TMP_InputField _categoryNameInputField;
        [SerializeField] private ButtonComponent _saveButton;
        [SerializeField] private AccordionComponent _accordionComponent;

        [Inject] private IWindowsController _windowsController;
        [Inject] private ICategoriesRepository _categoriesRepository;

        internal override void Init()
        {
            _saveButton.OnClickAsObservable()
                .Subscribe(this, static (_, self) => self.CreateCategory())
                .RegisterTo(destroyCancellationToken);

            _accordionComponent.Init();
        }

        private void CreateCategory()
        {
            if (string.IsNullOrWhiteSpace(_categoryNameInputField.text))
                return;

            var newCategory = _categoriesRepository.CreateCategory(_categoryNameInputField.text);
           _windowsController.OpenPopUpByType(PopUpType.Category, newCategory);
        }
    }
}
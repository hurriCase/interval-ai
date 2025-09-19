using CustomUtils.Runtime.Extensions;
using R3.Triggers;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Category;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoryEntryItem : CategoryEntryItemBase
    {
        [Inject] private IWindowsController _windowsController;

        protected override void OnInit()
        {
            categoryOpenArea.OnPointerClickAsObservable().SubscribeAndRegister(this, self => self.OpenCategoryPopUp());
        }

        private void OpenCategoryPopUp()
        {
            var selectionPopUp = _windowsController.OpenPopUp<CategoryPopUp>();
            selectionPopUp.SetParameters(currentCategoryEntry);
        }
    }
}
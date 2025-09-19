using CustomUtils.Runtime.Extensions;
using R3.Triggers;
using Source.Scripts.Main.UI.Screens.Categories;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.CategorySelection
{
    internal sealed class OnboardingCategoryEntryItem : CategoryEntryItemBase
    {
        protected override void OnInit()
        {
            categoryOpenArea.OnPointerClickAsObservable().SubscribeAndRegister(this,
                static self => self.selectedCheckbox.isOn = self.selectedCheckbox.isOn is false);
        }
    }
}
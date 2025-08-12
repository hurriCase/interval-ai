using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Base.PopUp;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Screen.Behaviours
{
    internal sealed class IntervalBehaviour : StepBehaviourBase
    {
        [Inject] private IWindowsController _windowsController;

        internal override void OnContinue()
        {
            _windowsController.OpenPopUpByType(PopUpType.OnboardingPractice);
        }
    }
}
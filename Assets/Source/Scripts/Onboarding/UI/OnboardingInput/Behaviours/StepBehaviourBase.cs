using R3;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours
{
    internal abstract class StepBehaviourBase : MonoBehaviour
    {
        internal Observable<Unit> OnContinue => @continue;
        protected readonly Subject<Unit> @continue = new();

        internal virtual void Init() { }
        internal virtual void UpdateView() { }
        internal virtual void HandleContinue() { }
    }
}
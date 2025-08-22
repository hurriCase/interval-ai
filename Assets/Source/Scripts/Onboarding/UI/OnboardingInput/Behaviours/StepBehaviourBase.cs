using R3;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours
{
    internal abstract class StepBehaviourBase : MonoBehaviour
    {
        protected readonly Subject<Unit> continueSubject = new();
        internal Observable<Unit> OnContinueSubject => continueSubject.AsObservable();

        internal virtual void Init() { }
        internal virtual void UpdateView() { }
        internal virtual void OnContinue() { }
    }
}
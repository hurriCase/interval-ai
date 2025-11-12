using R3;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.Setup.Behaviours
{
    internal abstract class StepBehaviourBase : MonoBehaviour
    {
        internal Observable<Unit> OnNextStep => nextStep;
        protected readonly Subject<Unit> nextStep = new();

        internal virtual void Init() { }
        internal virtual void UpdateView() { }
        internal virtual void HandleContinue() { }
    }
}
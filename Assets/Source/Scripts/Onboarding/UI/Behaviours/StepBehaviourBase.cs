using UnityEngine;

namespace Source.Scripts.Onboarding.UI.Behaviours
{
    internal abstract class StepBehaviourBase : MonoBehaviour
    {
        internal abstract void Init();
        internal virtual void OnContinue() { }
    }
}
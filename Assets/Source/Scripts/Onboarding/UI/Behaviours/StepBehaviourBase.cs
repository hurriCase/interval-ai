using UnityEngine;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.UI.Behaviours
{
    internal abstract class StepBehaviourBase : MonoBehaviour
    {
        [field: SerializeField] internal string TitleLocalizationKey { get; private set; }

        internal abstract void Init();
    }
}
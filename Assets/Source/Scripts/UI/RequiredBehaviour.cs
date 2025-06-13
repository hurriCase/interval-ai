using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.UI
{
    internal class RequiredBehaviour<TRequiredComponent> : MonoBehaviour where TRequiredComponent : Object
    {
        protected TRequiredComponent requiredComponent;

        private void OnValidate()
        {
            requiredComponent = GetComponent<TRequiredComponent>();
        }
    }
}
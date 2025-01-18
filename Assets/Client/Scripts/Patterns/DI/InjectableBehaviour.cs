using UnityEngine;

namespace Client.Scripts.Patterns.DI
{
    internal abstract class InjectableBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DependencyInjector.InjectDependencies(this);
        }
    }
}
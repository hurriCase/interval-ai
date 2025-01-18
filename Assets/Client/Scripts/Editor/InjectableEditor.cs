using Client.Scripts.Core;
using Client.Scripts.Patterns.DI;

namespace Client.Scripts.Editor
{
    internal class InjectableEditor : UnityEditor.Editor
    {
        protected virtual void OnEnable()
        {
            ServiceRegister.RegisterRegularServices();
            DependencyInjector.InjectDependencies(this);
        }
    }
}
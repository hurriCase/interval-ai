using Client.Scripts.Core;
using Client.Scripts.Patterns.DI;
using UnityEditor;

namespace Client.Scripts.Editor
{
    internal class InjectableEditorWindow : EditorWindow
    {
        protected virtual void OnEnable()
        {
            ServiceRegister.RegisterRegularServices();
            DependencyInjector.InjectDependencies(this);
        }
    }
}
using Client.Scripts.Core;
using Client.Scripts.Patterns.DI;
using UnityEditor;

namespace Client.Scripts.Editor
{
    internal class InjectableEditorWindow : EditorWindow
    {
        public InjectableEditorWindow()
        {
            ServiceRegister.RegisterRegularServices();
            DependencyInjector.InjectDependencies(this);
        }
    }
}
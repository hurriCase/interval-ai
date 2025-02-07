using Client.Scripts.Core;
using DependencyInjection.Runtime;
using UnityEditor;

namespace Client.Scripts.Editor
{
    [InitializeOnLoad]
    internal static class EditorInitializer
    {
        static EditorInitializer()
        {
            var serviceRegister = new ServiceRegister();
            serviceRegister.RegisterStaticServices();

            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        }

        private static void OnBeforeAssemblyReload()
        {
            DIContainer.Clear();
        }
    }
}
using System;
using System.Reflection;
using Client.Scripts.Patterns;
using Client.Scripts.Patterns.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Client.Scripts.Editor
{
    public static class RequiredFieldsValidator
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void DebugWarnings()
        {
            var monoBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            foreach (var monoBehaviour in monoBehaviours)
            {
                var fields = monoBehaviour
                    .GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    var attribute = field.GetCustomAttribute<RequiredFieldAttribute>();

                    if (attribute != null)
                    {
                        var fieldValue = field.GetValue(monoBehaviour);

                        if (fieldValue.Equals(null) || fieldValue is Array { Length: 0 })
                            Debug.LogError($"Field {field.Name} is required.", monoBehaviour);
                    }
                }
            }
        }
    }
}
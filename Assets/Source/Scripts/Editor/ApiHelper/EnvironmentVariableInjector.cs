using CustomUtils.Editor.Scripts.InputDialog;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Api.Base;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Source.Scripts.Editor.ApiHelper
{
    internal sealed class EnvironmentVariableInjector : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (BuildConstants.PasswordEnvironmentVariable.TryGetValueFromEnvironment(out var password) is false)
            {
                Debug.LogWarning("No password found in environment variables! Using default or empty value.");
                password = EditorUtility.DisplayDialog("Password Required",
                    "No password found in environment variables. Would you like to enter it now?",
                    "Yes", "No")
                    ? EditorInputDialog.Show("Enter Password", "Password:", string.Empty)
                    : string.Empty;
            }

            var configs = Resources.FindObjectsOfTypeAll<ApiConfigBase>();
            foreach (var apiConfigBase in configs)
                apiConfigBase.SetPassword(password);

            Debug.Log("Password injected from environment variables.");
        }
    }
}
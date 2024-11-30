using Firebase;
using Firebase.Extensions;
using UnityEngine;

namespace Client.Scripts
{
    internal static class FireBaseStep
    {
        internal static FirebaseApp FirebaseApp { get; private set; }

        internal static void FireBaseInit()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    FirebaseApp = FirebaseApp.DefaultInstance;

                    Debug.Log($"[FireBaseStep::FireBaseInit] FireBase is initialized");
                }
                else
                    Debug.LogError("[FireBaseStep::FireBaseInit] " +
                                   $"Could not resolve all Firebase dependencies: {dependencyStatus}");
            });
        }
    }
}
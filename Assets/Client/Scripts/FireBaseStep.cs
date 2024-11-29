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
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    FirebaseApp = FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");

                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }
    }
}
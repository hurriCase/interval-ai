using System;
using System.Threading.Tasks;
using Client.Scripts.Steps;
using Firebase;
using UnityEngine;

namespace Client.Scripts
{
    internal sealed class FireBaseStep : IStep
    {
        internal static FirebaseApp FirebaseApp { get; private set; }

        public event Action<int, string> OnCompleted;

        //TODO:<dmitriy.sukharev> I don't understand why is it necessary
        public async Task Execute(int step)
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp = FirebaseApp.DefaultInstance;

                Debug.Log($"[FireBaseStep::FireBaseInit] FireBase is initialized");
            }
            else
                Debug.LogError("[FireBaseStep::FireBaseInit] " +
                               $"Could not resolve all Firebase dependencies: {dependencyStatus}");

            OnCompleted?.Invoke(step, GetType().Name);
        }
    }
}
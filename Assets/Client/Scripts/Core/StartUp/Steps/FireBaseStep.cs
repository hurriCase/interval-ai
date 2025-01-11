using System;
using System.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class FireBaseStep : IStep
    {
        internal static FirebaseApp FirebaseApp { get; private set; }

        public event Action<int, string> OnStepCompleted;

        //TODO:<dmitriy.sukharev> I don't understand why is it necessary
        public async Task Execute(int step)
        {
            try
            {
                var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

                if (dependencyStatus == DependencyStatus.Available)
                {
                    FirebaseApp = FirebaseApp.DefaultInstance;

                    OnStepCompleted?.Invoke(step, GetType().Name);
                    Debug.Log("[FireBaseStep::FireBaseInit] FireBase is initialized");
                }
                else
                    throw new Exception("[FireBaseStep::FireBaseInit] " +
                                        $"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DIStep::Execute] {GetType().Name} step initialization is failed: {e.Message}");
            }
        }
    }
}
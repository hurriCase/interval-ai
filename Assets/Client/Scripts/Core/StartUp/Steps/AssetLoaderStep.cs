using System;
using System.Threading.Tasks;
using AssetLoader.Runtime;
using AssetLoader.Runtime.Config;
using UnityEngine;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class AssetLoaderStep : IStep
    {
        public event Action<int, string> OnStepCompleted;

        public Task Execute(int step)
        {
            try
            {
                var assetLoaderSettings = ResourceLoader<AssetLoaderConfig>.Load();
                AssetLoaderInitializer.Init(assetLoaderSettings);

                OnStepCompleted?.Invoke(step, GetType().Name);
            }
            catch (Exception e)
            {
                Debug.LogError($"[DataStep::Execute] {GetType().Name} step initialization is failed: {e.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
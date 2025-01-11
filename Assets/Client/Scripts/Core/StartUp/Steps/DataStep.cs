using System;
using System.Threading.Tasks;
using Client.Scripts.DB.DBControllers;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class DataStep : Injectable, IStep
    {
        [Inject] private ICloudRepository _cloudRepository;
        [Inject] private IOfflineRepository _offlineRepository;
        [Inject] private IEntityController _entityController;
        [Inject] private IUserDataController _userDataController;

        public event Action<int, string> OnCompleted;

        public async Task Execute(int step)
        {
            try
            {
                await _offlineRepository.InitAsync();
                _userDataController.Init();
                await _cloudRepository.InitAsync();
                await _entityController.InitAsync();

                OnCompleted?.Invoke(step, GetType().Name);
            }
            catch (Exception e)
            {
                Debug.LogError($"[DataStep::Execute] {GetType().Name} step initialization is failed: {e.Message}");
            }
        }
    }
}
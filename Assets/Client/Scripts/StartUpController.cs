using System;
using Client.Scripts.Database;
using UnityEngine;

namespace Client.Scripts
{
    internal sealed class StartUpController : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void OnBeforeSceneLoadRuntimeMethod()
        {
            try
            {
                AudioController.Instance.PlayMusic();
                //TODO: Create DI
                await DBController.Instance.Init();
                await VocabularyController.Init();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
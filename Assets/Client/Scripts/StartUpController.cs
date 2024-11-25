using System;
using Client.Scripts.Database;
using Client.Scripts.Database.Vocabulary;
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
                await DBController.Instance.Init();
                //TODO: Replace with real userID
                await VocabularyController.Init(DBController.Instance, "1");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
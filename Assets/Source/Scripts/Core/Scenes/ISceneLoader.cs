using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Source.Scripts.Core.Scenes
{
    internal interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneAddress, CancellationToken token, LoadSceneMode loadMode = LoadSceneMode.Single);
        void TryUnloadScene(SceneInstance sceneInstance);
    }
}
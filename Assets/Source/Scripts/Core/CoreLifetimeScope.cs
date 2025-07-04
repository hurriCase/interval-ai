using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Scenes;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Core
{
    internal sealed class CoreLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var mainMenuScene = SceneReferences.Instance.MainMenuScene;
            SceneLoader.LoadSceneAsync(mainMenuScene.Address).Forget();
        }
    }
}
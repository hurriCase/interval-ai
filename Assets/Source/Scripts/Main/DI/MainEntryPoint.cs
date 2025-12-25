using System.Threading;
using CustomUtils.Runtime.Scenes.Base;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.GenerativeLanguage;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Windows.Menu;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class MainEntryPoint : IAsyncStartable
    {
        private readonly ISceneTransitionController _sceneTransitionController;
        private readonly IGenerativeLanguageService _generativeLanguageService;
        private readonly IWindowsController _windowsController;
        private readonly IMenuBehaviour _menuManuBehaviour;

        internal MainEntryPoint(
            IGenerativeLanguageService generativeLanguageService,
            ISceneTransitionController transitionController,
            IWindowsController windowsController,
            IMenuBehaviour manuBehaviour)
        {
            _generativeLanguageService = generativeLanguageService;
            _sceneTransitionController = transitionController;
            _windowsController = windowsController;
            _menuManuBehaviour = manuBehaviour;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            await _generativeLanguageService.InitAsync(token);
            await _windowsController.InitAsync(token);

            _menuManuBehaviour.Init();

            _sceneTransitionController.EndTransition();
        }
    }
}
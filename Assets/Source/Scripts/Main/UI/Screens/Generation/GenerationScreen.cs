using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.GenerativeLanguage;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Screens.Generation.Behaviours;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Generation
{
    internal sealed class GenerationScreen : ScreenBase
    {
        [SerializeField] private CategoryPreviewBehaviour _categoryPreviewBehaviour;
        [SerializeField] private CurrentSettingsBehaviour _currentSettingsBehaviour;

        [SerializeField] private ThemeButton _savedGenerationsButton;
        [SerializeField] private ThemeButton _generateButton;
        [SerializeField] private ThemeButton _chatButton;

        private IGenerativeLanguage _generativeLanguage;
        private IWindowsController _windowsController;

        [Inject]
        public void Inject(IGenerativeLanguage generativeLanguage, IWindowsController windowsController)
        {
            _generativeLanguage = generativeLanguage;
            _windowsController = windowsController;
        }

        internal override void Init()
        {
            _categoryPreviewBehaviour.Init();
            _currentSettingsBehaviour.Init();

            _windowsController.BindPopUpOpen(_savedGenerationsButton, PopUpType.Exercise);
            _windowsController.BindPopUpOpen(_chatButton, PopUpType.Chat);

            _generativeLanguage.IsAvailable.SubscribeToInteractableUntilDestroy(_chatButton);
        }

        internal override UniTask ShowAsync()
        {
            _generativeLanguage.UpdateAvailable(destroyCancellationToken);

            return base.ShowAsync();
        }
    }
}
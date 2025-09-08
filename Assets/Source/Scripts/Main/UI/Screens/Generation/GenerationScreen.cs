using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Screens.Generation.Behaviours;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Generation
{
    internal sealed class GenerationScreen : ScreenBase
    {
        [SerializeField] private CategoryPreviewBehaviour _categoryPreviewBehaviour;
        [SerializeField] private CurrentSettingsBehaviour _currentSettingsBehaviour;

        [SerializeField] private ButtonComponent _savedGenerationsButton;
        [SerializeField] private ButtonComponent _generateButton;
        [SerializeField] private ButtonComponent _chatButton;

        private IWindowsController _windowsController;

        [Inject]
        public void Inject(IWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        internal override void Init()
        {
            _categoryPreviewBehaviour.Init();
            _currentSettingsBehaviour.Init();

            _chatButton.OnClickAsObservable().SubscribeAndRegister(this, self => self.OpenChatPopUp());
        }

        private void OpenChatPopUp()
        {
            _windowsController.OpenPopUpByType(PopUpType.Chat);
        }
    }
}
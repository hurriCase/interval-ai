using Cysharp.Text;
using R3;
using Source.Scripts.Core.AI;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Screens.Generation.Behaviours;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.Screen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Generation
{
    internal sealed class GenerationScreen : ScreenBase
    {
        [SerializeField] private CategoryPreviewBehaviour _categoryPreviewBehaviour;

        [SerializeField] private ButtonComponent _savedGenerationsButton;
        [SerializeField] private ButtonComponent _generateButton;
        [SerializeField] private ButtonComponent _chatButton;
        [SerializeField] private SwitchComponent _translateFromSwitch;
        [SerializeField] private CheckboxComponent _unfamiliarWordsCheckbox;

        [SerializeField] private Slider _percentageSlider;
        [SerializeField] private TextMeshProUGUI _percentageText;

        [Inject] private IAIController _aiController;
        [Inject] private IWindowsController _windowsController;

        internal override void Init()
        {
            _categoryPreviewBehaviour.Init();

            _translateFromSwitch.Init();

            _chatButton.OnClickAsObservable()
                .Subscribe(_windowsController,
                    static (_, controller) => controller.OpenPopUpByType(PopUpType.Chat))
                .RegisterTo(destroyCancellationToken);

            _percentageSlider.OnValueChangedAsObservable()
                .Subscribe(this, (percentage, screen)
                    => screen._percentageText.SetTextFormat("{0:0}%", percentage * 100))
                .RegisterTo(destroyCancellationToken);
        }
    }
}
using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Components.Checkbox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.GenerationSettings.Behaviours
{
    internal sealed class WordSettingBehaviour : MonoBehaviour
    {
        [SerializeField] private Slider _percentageSlider;
        [SerializeField] private TextMeshProUGUI _percentText;
        [SerializeField] private CheckboxComponent _isHighlightedCheckbox;

        private IGenerationSettingsRepository _generationSettingsRepository;

        [Inject]
        internal void Inject(IGenerationSettingsRepository generationSettingsRepository)
        {
            _generationSettingsRepository = generationSettingsRepository;
        }

        internal void Init()
        {
            _isHighlightedCheckbox.isOn = _generationSettingsRepository.IsHighlightNewWords.Value;
            _isHighlightedCheckbox.OnValueChangedAsObservable().SubscribeAndRegister(this, static (isOn, self)
                => self._generationSettingsRepository.IsHighlightNewWords.Value = isOn);

            _percentageSlider.value = _generationSettingsRepository.NewWordsPercentage.Value;
            _percentageSlider.OnValueChangedAsObservable()
                .SubscribeAndRegister(this, (percent, self) => self.HandlePercentChange(percent));
        }

        private void HandlePercentChange(float percent)
        {
            _percentText.SetTextFormat("{0:0}%", percent * 100);
            _generationSettingsRepository.NewWordsPercentage.Value = percent;
        }
    }
}
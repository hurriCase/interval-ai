using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Data.Repositories.Settings;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.UI.Behaviours
{
    internal sealed class LevelSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private ButtonTextComponent _levelButtonText;
        [SerializeField] private RectTransform _levelButtonsContainer;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ILocalizationDatabase _localizationDatabase;

        internal override void Init()
        {
            foreach (var (levelType, levelLocalizationKey) in _localizationDatabase.LanguageLevelKeys.AsTuples())
            {
                var createdButton = Instantiate(_levelButtonText, _levelButtonsContainer);
                createdButton.Text.text = levelLocalizationKey.GetLocalization();
                createdButton.Button.OnClickAsObservable()
                    .Subscribe((levelType, _settingsRepository),
                        static (_, tuple) => tuple._settingsRepository.LanguageLevel.Value = tuple.levelType)
                    .RegisterTo(destroyCancellationToken);
            }
        }
    }
}
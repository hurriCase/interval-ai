using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using R3.Triggers;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LevelSelection
{
    internal sealed class LevelSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private ToggleGroup _selectionToggleGroup;
        [SerializeField] private ToggleComponent _selectionCheckbox;
        [SerializeField] private RectTransform _levelButtonsContainer;

        private ILanguageSettingsRepository _languageSettingsRepository;
        private ILocalizationDatabase _localizationDatabase;
        private IAddressablesLoader _addressablesLoader;
        private ISpriteReferences _spriteReferences;

        [Inject]
        internal void Inject(
            ILanguageSettingsRepository languageSettingsRepository,
            ILocalizationDatabase localizationDatabase,
            IAddressablesLoader addressablesLoader,
            ISpriteReferences spriteReferences)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _localizationDatabase = localizationDatabase;
            _addressablesLoader = addressablesLoader;
            _spriteReferences = spriteReferences;
        }

        internal override void Init()
        {
            foreach (var (levelType, levelLocalizationKey) in _localizationDatabase.LanguageLevelKeys.AsTuples())
            {
                var selectionCheckbox = Instantiate(_selectionCheckbox, _levelButtonsContainer);
                selectionCheckbox.Text.text = levelLocalizationKey.GetLocalization();
                selectionCheckbox.group = _selectionToggleGroup;
                selectionCheckbox.OnPointerClickAsObservable().SubscribeAndRegister(this, levelType,
                    static (levelType, self) => self._languageSettingsRepository.LanguageLevel.Value = levelType);

                _addressablesLoader.AssignImageAsync(
                    selectionCheckbox.Image,
                    _spriteReferences.LevelLanguageIcons[levelType],
                    destroyCancellationToken);
            }
        }
    }
}
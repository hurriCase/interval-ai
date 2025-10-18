using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using R3.Triggers;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LevelSelection
{
    internal sealed class LevelSelectionItem : MonoBehaviour
    {
        [SerializeField] private StateToggle _stateToggle;
        [SerializeField] private Image _icon;

        private ILanguageSettingsRepository _languageSettingsRepository;
        private IAddressablesLoader _addressablesLoader;
        private ISpriteReferences _spriteReferences;

        [Inject]
        internal void Inject(
            ILanguageSettingsRepository languageSettingsRepository,
            IAddressablesLoader addressablesLoader,
            ISpriteReferences spriteReferences)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _addressablesLoader = addressablesLoader;
            _spriteReferences = spriteReferences;
        }

        internal void Init(ToggleGroup toggleGroup, string localizationKey, LanguageLevel languageLevel)
        {
            _stateToggle.Text.text = localizationKey.GetLocalization();
            _stateToggle.group = toggleGroup;
            _stateToggle.OnPointerClickAsObservable().SubscribeUntilDestroy(this, languageLevel,
                static (levelType, self) => self._languageSettingsRepository.LanguageLevel.Value = levelType);

            var iconSprite = _spriteReferences.LevelLanguageIcons[languageLevel];
            _addressablesLoader.AssignImageAsync(_icon, iconSprite, destroyCancellationToken);
        }
    }
}
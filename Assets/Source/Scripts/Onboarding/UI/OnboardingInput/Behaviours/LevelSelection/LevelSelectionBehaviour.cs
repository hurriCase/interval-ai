using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LevelSelection
{
    internal sealed class LevelSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private RectTransform _levelButtonsContainer;
        [SerializeField] private LevelSelectionItem _levelSelectionItem;
        [SerializeField] private ToggleGroup _toggleGroup;

        [SerializeField] private EnumArray<LanguageLevel, LocalizationKey> _languageLevelKeys;

        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            foreach (var (levelType, levelLocalizationKey) in _languageLevelKeys.AsTuples())
            {
                var selectionCheckbox = _objectResolver.Instantiate(_levelSelectionItem, _levelButtonsContainer);
                selectionCheckbox.Init(_toggleGroup, levelLocalizationKey, levelType);
            }
        }
    }
}
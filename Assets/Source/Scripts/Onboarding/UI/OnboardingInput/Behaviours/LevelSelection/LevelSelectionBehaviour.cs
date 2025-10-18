using Source.Scripts.Core.Localization.Base;
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

        private ILocalizationDatabase _localizationDatabase;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(ILocalizationDatabase localizationDatabase, IObjectResolver objectResolver)
        {
            _localizationDatabase = localizationDatabase;
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            foreach (var (levelType, levelLocalizationKey) in _localizationDatabase.LanguageLevelKeys.AsTuples())
            {
                var selectionCheckbox = _objectResolver.Instantiate(_levelSelectionItem, _levelButtonsContainer);
                selectionCheckbox.Init(_toggleGroup, levelLocalizationKey, levelType);
            }
        }
    }
}
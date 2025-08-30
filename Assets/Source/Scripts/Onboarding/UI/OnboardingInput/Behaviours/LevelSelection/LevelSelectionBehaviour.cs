using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LevelSelection
{
    internal sealed class LevelSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private ToggleGroup _selectionToggleGroup;
        [SerializeField] private SelectionToggleItem _selectionToggleItem;
        [SerializeField] private RectTransform _levelButtonsContainer;
        [SerializeField] private AspectRatioFitter _aspectRatioFitter;
        [SerializeField] private float _spacingRatio;

        private ILanguageSettingsRepository _languageSettingsRepository;
        private ILocalizationDatabase _localizationDatabase;
        private IAddressablesLoader _addressablesLoader;
        private ISpriteReferences _spriteReferences;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(
            ILanguageSettingsRepository languageSettingsRepository,
            ILocalizationDatabase localizationDatabase,
            IAddressablesLoader addressablesLoader,
            ISpriteReferences spriteReferences,
            IObjectResolver objectResolver)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _localizationDatabase = localizationDatabase;
            _addressablesLoader = addressablesLoader;
            _spriteReferences = spriteReferences;
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            foreach (var (levelType, levelLocalizationKey) in _localizationDatabase.LanguageLevelKeys.AsTuples())
            {
                var createdButton = _objectResolver.Instantiate(_selectionToggleItem, _levelButtonsContainer);
                createdButton.LevelText.text = levelLocalizationKey.GetLocalization();
                createdButton.CheckboxComponent.group = _selectionToggleGroup;
                createdButton.CheckboxComponent.OnPointerClickAsObservable()
                    .Subscribe((levelType, _languageSettingsRepository),
                        static (_, tuple) => tuple._languageSettingsRepository.LanguageLevel.Value = tuple.levelType)
                    .RegisterTo(destroyCancellationToken);

                SetLevelIcon(createdButton.Icon, _spriteReferences.LevelLanguageIcons[levelType].AssetGUID).Forget();

                _aspectRatioFitter.CreateHeightSpacing(_spacingRatio, _levelButtonsContainer);
            }
        }

        private async UniTask SetLevelIcon(Image image, string assetGUID)
        {
            image.sprite = await _addressablesLoader.LoadAsync<Sprite>(assetGUID, destroyCancellationToken);
        }
    }
}
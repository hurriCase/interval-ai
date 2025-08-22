using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Sprites;
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

        [Inject] private ILanguageSettingsRepository _languageSettingsRepository;
        [Inject] private ISpriteReferences _spriteReferences;
        [Inject] private ILocalizationDatabase _localizationDatabase;
        [Inject] private IAddressablesLoader _addressablesLoader;
        [Inject] private IObjectResolver _objectResolver;

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
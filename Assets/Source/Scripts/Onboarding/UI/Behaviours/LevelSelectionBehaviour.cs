using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.UI.Behaviours
{
    internal sealed class LevelSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private ButtonTextComponent _levelButtonText;
        [SerializeField] private RectTransform _levelButtonsContainer;

        [Inject] private IUserRepository _userRepository;

        internal override void Init()
        {
            foreach (var (levelType, levelLocalizationKey) in LocalizationKeysDatabase.Instance
                         .LanguageLevelLocalizationData.AsTuples())
            {
                var createdButton = Instantiate(_levelButtonText, _levelButtonsContainer);
                createdButton.Text.text = levelLocalizationKey.GetLocalization();
                createdButton.Button.OnClickAsObservable()
                    .Subscribe((levelType, _userRepository),
                        static (_, tuple) => tuple._userRepository.UserLevel.Value = tuple.levelType)
                    .RegisterTo(destroyCancellationToken);
            }
        }
    }
}
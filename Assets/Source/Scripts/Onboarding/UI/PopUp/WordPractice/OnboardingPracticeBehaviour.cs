using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.Onboarding.Data;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.PopUp.WordPractice
{
    internal sealed class OnboardingPracticeBehaviour : MonoBehaviour
    {
        [SerializeField] private PracticeState _practiceState;

        [SerializeField] private CardBehaviour _cardBehaviour;
        [SerializeField] private ProgressIndicatorBehaviour _progressIndicatorBehaviour;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private ControlButtonsBehaviour _controlButtonsBehaviour;

        [Inject] private IOnboardingConfig _onboardingConfig;

        internal void Init()
        {
            _cardBehaviour.Init();

            var onboardingWord = _onboardingConfig.OnboardingWord.CreateWord();

            _cardBehaviour.WordEntry.Value = onboardingWord;

            _progressIndicatorBehaviour.Init(_practiceState);

            _wordProgressBehaviour.Init(_practiceState);

            _controlButtonsBehaviour.Init(_practiceState);
        }
    }
}
using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal abstract class LearningCompleteBehaviourBase : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private ButtonComponent _learnNewWordsButton;

        [SerializeField] private TextMeshProUGUI _completeText;
        [SerializeField] private GameObject _noWordsImage;
        [SerializeField] private GameObject _completeImage;

        [SerializeField] private GameObject _addNewWordsContainer;
        [SerializeField] private ButtonComponent _exitButton;
        [SerializeField] private ButtonComponent _learnButton;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        [Inject] protected IWindowsController windowsController;
        [Inject] protected IPracticeStateService practiceStateService;

        [Inject] private IWordsRepository _wordsRepository;
        [Inject] private IAppConfig _appConfig;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        private PracticeState _currentPracticeState;

        internal void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            _plusMinusBehaviour.Init();

            _learnNewWordsButton.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) =>
                {
                    behaviour._buttonsContainer.SetActive(false);
                    behaviour._addNewWordsContainer.SetActive(true);
                })
                .RegisterTo(destroyCancellationToken);

            _learnButton.OnClickAsObservable()
                .Subscribe(practiceStateService, static (_, service) => service.SetState(PracticeState.NewWords))
                .RegisterTo(destroyCancellationToken);

            _exitButton.OnClickAsObservable()
                .Subscribe(windowsController,
                    static (_, controller) => controller.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);

            _wordsRepository.CurrentWordsByState
                .Subscribe(this, static (currentWords, self)
                    => self.CheckCompleteness(currentWords))
                .RegisterTo(destroyCancellationToken);

            OnInit();
        }

        private void CheckCompleteness(EnumArray<PracticeState, WordEntry> currentWords)
        {
            var currentWord = currentWords[_currentPracticeState];
            if (currentWord == null)
                SetState(CompleteType.NoWords);
            else if (currentWord.Cooldown > DateTime.Now)
                SetState(CompleteType.Complete);
        }

        protected void SetState(CompleteType completeType, string newWordCount = null)
        {
            var localization = _localizationKeysDatabase.GetCompletesLocalization(_currentPracticeState, completeType);
            _completeText.SetTextFormat(localization, newWordCount);

            _noWordsImage.SetActive(completeType == CompleteType.NoWords);
            _completeImage.SetActive(completeType == CompleteType.Complete);
        }

        protected abstract void OnInit();
    }
}
using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
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

        [Inject] private IWordsRepository _wordsRepository;
        [Inject] private IDefaultSettingsConfig _defaultSettingsConfig;
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
                .Subscribe(static _ => WordPracticePopUp.CurrentState.OnNext(PracticeState.NewWords))
                .RegisterTo(destroyCancellationToken);

            _exitButton.OnClickAsObservable()
                .Subscribe(windowsController,
                    static (_, controller) => controller.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);

            _wordsRepository.SortedWordsByState
                .Subscribe(this, static (sortedWords, self)
                    => self.CheckCompleteness(sortedWords))
                .RegisterTo(destroyCancellationToken);

            OnInit();
        }

        private void CheckCompleteness(EnumArray<LearningState, SortedSet<WordEntry>> sortedWords)
        {
            foreach (var (learningState, words) in sortedWords.AsTuples())
            {
                var targetLearningStates = _defaultSettingsConfig.PracticeToLearningStates[_currentPracticeState];

                foreach (var targetLearningState in targetLearningStates)
                {
                    if (targetLearningState != learningState)
                        continue;

                    if (words.Count == 0)
                        SetState(CompleteType.NoWords);
                    else if (words.Min.Cooldown > DateTime.Now)
                        SetState(CompleteType.Complete);
                }
            }
        }

        internal void SetState(CompleteType completeType, string newWordCount = null)
        {
            var localization = _localizationKeysDatabase.GetCompletesLocalization(_currentPracticeState, completeType);
            _completeText.SetTextFormat(localization, newWordCount);

            _noWordsImage.SetActive(completeType == CompleteType.NoWords);
            _completeImage.SetActive(completeType == CompleteType.Complete);
        }

        protected abstract void OnInit();
    }
}
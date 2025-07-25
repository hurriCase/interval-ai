using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards
{
    internal sealed class ControlButtonsBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonTextComponent _positiveControlItem;
        [SerializeField] private ButtonTextComponent _cancelControlItem;
        [SerializeField] private ButtonTextComponent _nextControlItem;

        private CardBehaviourBase _cardBehaviourBase;

        [Inject] private IVocabularyRepository _vocabularyRepository;

        internal void Init(CardBehaviourBase cardBehaviourBase)
        {
            _cardBehaviourBase = cardBehaviourBase;

            _positiveControlItem.Button.OnClickAsObservable()
                .Subscribe(this, (_, behaviour) => behaviour.AdvanceWord())
                .RegisterTo(destroyCancellationToken);

            _cancelControlItem.Button.OnClickAsObservable()
                .Subscribe(_cardBehaviourBase, (_, cardBehaviour) =>
                {
                    cardBehaviour.CurrentWord.IsHidden = true;
                    cardBehaviour.UpdateWord();
                })
                .RegisterTo(destroyCancellationToken);

            _nextControlItem.Button.OnClickAsObservable()
                .Subscribe(this, (_, behaviour) => behaviour.SwitchToNext())
                .RegisterTo(destroyCancellationToken);
        }

        internal void UpdateView()
        {
            var isFirstShow = _cardBehaviourBase.CurrentWord.LearningState == LearningState.None;

            var positiveKey = isFirstShow ? "ui.word-practice.already-know" : "ui.word-practice.got-it";
            _positiveControlItem.Text.text = LocalizationController.Localize(positiveKey);

            _cancelControlItem.Button.SetActive(isFirstShow);
            _cancelControlItem.Text.text = LocalizationController.Localize("ui.word-practice.hide");

            var nextKey = isFirstShow ? "ui.word-practice.learn" : "ui.word-practice.missed-it";
            _nextControlItem.Text.text = LocalizationController.Localize(nextKey);
        }

        internal void AdvanceWord()
        {
            var currentWord = _cardBehaviourBase.CurrentWord;
            if (currentWord.LearningState == LearningState.None)
            {
                currentWord.LearningState = LearningState.AlreadyKnown;
                return;
            }

            _vocabularyRepository.AdvanceWord(_cardBehaviourBase.CurrentWord, true);
            _cardBehaviourBase.UpdateWord();
        }

        internal void SwitchToNext()
        {
            var currentWord = _cardBehaviourBase.CurrentWord;
            var success = currentWord.LearningState == LearningState.None;

            _vocabularyRepository.AdvanceWord(currentWord, success);
            _cardBehaviourBase.UpdateWord();
        }
    }
}
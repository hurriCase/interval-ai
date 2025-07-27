using R3;
using Source.Scripts.Data.Repositories.Words.Base;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards
{
    internal sealed class ControlButtonsBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _alreadyKnowButton;
        [SerializeField] private ButtonComponent _hideButton;
        [SerializeField] private ButtonComponent _learnButton;

        [SerializeField] private ButtonComponent _memorizedButton;
        [SerializeField] private ButtonComponent _forgotButton;

        [SerializeField] private GameObject _firstShowContainer;
        [SerializeField] private GameObject _otherShowContainer;

        private CardBehaviourBase _cardBehaviourBase;

        [Inject] private IWordsRepository _wordsRepository;

        private bool _isFirstShow;

        internal void Init(CardBehaviourBase cardBehaviourBase)
        {
            _cardBehaviourBase = cardBehaviourBase;

            _hideButton.OnClickAsObservable()
                .Subscribe(_cardBehaviourBase, (_, cardBehaviour) =>
                {
                    cardBehaviour.CurrentWord.IsHidden = true;
                    cardBehaviour.UpdateWord();
                })
                .RegisterTo(destroyCancellationToken);

            SubscribeAdvanceButton(_alreadyKnowButton, false);
            SubscribeAdvanceButton(_learnButton, true);
            SubscribeAdvanceButton(_memorizedButton, true);
            SubscribeAdvanceButton(_forgotButton, false);
        }

        internal void UpdateView()
        {
            _isFirstShow = _cardBehaviourBase.CurrentWord.LearningState == LearningState.None;

            _firstShowContainer.SetActive(_isFirstShow);
            _otherShowContainer.SetActive(_isFirstShow is false);
        }

        private void SubscribeAdvanceButton(Button button, bool success) =>
            button.OnClickAsObservable()
                .Subscribe((behaviour: this, success), (_, tuple) => tuple.behaviour.AdvanceWord(tuple.success))
                .RegisterTo(destroyCancellationToken);

        private void AdvanceWord(bool success)
        {
            _wordsRepository.AdvanceWord(_cardBehaviourBase.CurrentWord, success);
            _cardBehaviourBase.UpdateWord();
        }
    }
}
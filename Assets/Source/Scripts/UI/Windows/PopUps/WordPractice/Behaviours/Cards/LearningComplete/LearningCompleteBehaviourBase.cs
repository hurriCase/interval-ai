using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Base;
using Source.Scripts.UI.Windows.Shared;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal abstract class LearningCompleteBehaviourBase : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private ButtonComponent _learnNewWordsButton;
        [SerializeField] private ButtonComponent _returnLateButton;

        [SerializeField] private GameObject _addNewWordsContainer;
        [SerializeField] private ButtonComponent _learnButton;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        [SerializeField] protected ButtonComponent exitButton;

        internal void Init()
        {
            _plusMinusBehaviour.Init();

            _learnNewWordsButton.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) =>
                {
                    behaviour._buttonsContainer.SetActive(false);
                    behaviour._addNewWordsContainer.SetActive(true);
                })
                .RegisterTo(destroyCancellationToken);

            _learnButton.OnClickAsObservable()
                .Subscribe(static _ => WordPracticePopUp.StateChangeRequested.OnNext(PracticeState.NewWords))
                .RegisterTo(destroyCancellationToken);

            _returnLateButton.OnClickAsObservable()
                .Subscribe(static _ => WindowsController.Instance.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);

            OnInit();
        }

        protected abstract void OnInit();
    }
}
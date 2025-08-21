using R3.Triggers;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.WordInfo
{
    internal sealed class WordInfoPopUp : PopUpBase
    {
        [SerializeField] private ButtonComponent _editButton;
        [SerializeField] private ButtonComponent _startLearningButton;

        [SerializeField] private RectTransform _cardContainer;
        [SerializeField] private RectTransform _cardContent;

        [SerializeField] private WordInfoCardBehaviour _wordInfoCardBehaviour;

        internal override void Init()
        {
            _wordInfoCardBehaviour.Init();

            _cardContent.OnRectTransformDimensionsChangeAsObservable()
                .SubscribeAndRegister(this, static self => self.UpdateContainerHeight());
        }

        internal void SetParameters(WordEntry wordEntry) => _wordInfoCardBehaviour.UpdateView(wordEntry);

        private void UpdateContainerHeight()
        {
            _cardContainer.sizeDelta = new Vector2(_cardContainer.sizeDelta.x, _cardContent.rect.height);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_cardContainer.parent as RectTransform);
        }
    }
}
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordInfo
{
    internal sealed class WordInfoPopUp : ParameterizedPopUpBase<WordEntry>
    {
        [SerializeField] private ButtonComponent _editButton;
        [SerializeField] private ButtonComponent _startLearningButton;

        [SerializeField] private RectTransform _cardContainer;
        [SerializeField] private RectTransform _cardContent;

        [SerializeField] private WordInfoCardBehaviour _wordInfoCardBehaviour;

        internal override void Init()
        {
            _wordInfoCardBehaviour.Init();
        }

        internal override void Show()
        {
            _wordInfoCardBehaviour.UpdateView(Parameters);

            base.Show();
        }
    }
}
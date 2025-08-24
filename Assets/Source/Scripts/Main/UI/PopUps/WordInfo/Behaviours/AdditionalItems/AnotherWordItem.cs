using R3;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal sealed class AnotherWordItem : AccordionItem, IAdditionalInfoItemBase<TranslationSet>
    {
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private TextMeshProUGUI _learningWordText;
        [SerializeField] private TextMeshProUGUI _translationsText;
        [SerializeField] private ButtonComponent _addButton;

        [Inject] private IWordsRepository _wordsRepository;

        public void Init(TranslationSet translationSet)
        {
            _wordProgressBehaviour.Init();
            _learningWordText.text = translationSet.Learning;
            _translationsText.text = translationSet.GetJoinedNativeWords();

            _addButton.OnClickAsObservable().SubscribeAndRegister(this, translationSet,
                static (translationSet, self) => self._wordsRepository.AddWord(translationSet));
        }
    }
}
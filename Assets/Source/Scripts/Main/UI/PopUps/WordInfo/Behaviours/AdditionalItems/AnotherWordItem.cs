using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal sealed class AnotherWordItem : MonoBehaviour, IAdditionalInfoItemBase<TranslationSet>
    {
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private TextMeshProUGUI _learningWordText;
        [SerializeField] private TextMeshProUGUI _translationsText;
        [SerializeField] private ButtonComponent _addButton;

        private TranslationSet _currentTranslationSet;

        private IWordsRepository _wordsRepository;

        [Inject]
        internal void Inject(IWordsRepository wordsRepository)
        {
            _wordsRepository = wordsRepository;
        }

        public void Init()
        {
            _wordProgressBehaviour.Init();

            _addButton.OnClickAsObservable().SubscribeUntilDestroy(this, _currentTranslationSet,
                static (translationSet, self) => self._wordsRepository.AddWord(translationSet));
        }

        public void UpdateView(TranslationSet translationSet)
        {
            _learningWordText.text = translationSet.Learning;
            _translationsText.text = translationSet.GetJoinedNativeWords();
        }
    }
}
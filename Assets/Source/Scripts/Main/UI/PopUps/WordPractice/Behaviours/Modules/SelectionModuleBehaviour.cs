using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using Source.Scripts.UI.Components.Button;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules
{
    internal sealed class SelectionModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private ButtonComponent[] _wordSelectionItems = new ButtonComponent[SelectionCount];

        private const int SelectionCount = 4;

        private IWordsRepository _wordsRepository;

        [Inject]
        internal void Inject(IWordsRepository wordsRepository)
        {
            _wordsRepository = wordsRepository;
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            var randomWords = _wordsRepository.GetRandomWords(currentWord, SelectionCount - 1);
            var correctWordIndex = Random.Range(0, SelectionCount);

            var index = -1;
            foreach (var wordEntry in randomWords)
            {
                index++;
                if (index == correctWordIndex)
                    index++;

                _wordSelectionItems[index].Text.text = wordEntry.Word.GetHiddenText(practiceSettingsRepository);
            }

            _wordSelectionItems[correctWordIndex].Text.text =
                currentWord.Word.GetHiddenText(practiceSettingsRepository);
        }
    }
}
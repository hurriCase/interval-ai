using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Selection
{
    internal sealed class SelectionModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private ButtonTextComponent[] _wordSelectionItems = new ButtonTextComponent[SelectionCount];

        [Inject] private IVocabularyRepository _vocabularyRepository;

        private const int SelectionCount = 4;

        protected override void UpdateView()
        {
            base.UpdateView();

            var randomWords = _vocabularyRepository.GetRandomWords(currentWord, SelectionCount - 1);
            var correctWordIndex = Random.Range(0, SelectionCount);

            var index = -1;
            foreach (var wordEntry in randomWords)
            {
                index++;
                if (index == correctWordIndex)
                    index++;

                _wordSelectionItems[index].Text.text = wordEntry.GetHiddenWord(userRepository);
            }

            _wordSelectionItems[correctWordIndex].Text.text = currentWord.GetHiddenWord(userRepository);
        }
    }
}
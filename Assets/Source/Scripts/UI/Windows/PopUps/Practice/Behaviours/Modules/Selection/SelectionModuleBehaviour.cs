using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules.Base;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules.Selection
{
    internal sealed class SelectionModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private WordSelectionItem[] _wordSelectionItems = new WordSelectionItem[SelectionCount];

        private const int SelectionCount = 4;

        protected override void UpdateView()
        {
            base.UpdateView();

            var randomWords = VocabularyRepository.Instance.WordEntries.Value.AsValueEnumerable()
                .Where(word => word != currentWord && word.IsHidden is false)
                .OrderBy(_ => Random.value)
                .Take(SelectionCount - 1);

            var correctWordIndex = Random.Range(0, SelectionCount);

            var index = -1;
            foreach (var wordEntry in randomWords)
            {
                index++;
                if (index == correctWordIndex)
                    index++;

                _wordSelectionItems[index].Word.text = wordEntry.HiddenWord;
            }

            _wordSelectionItems[correctWordIndex].Word.text = currentWord.HiddenWord;
        }
    }
}
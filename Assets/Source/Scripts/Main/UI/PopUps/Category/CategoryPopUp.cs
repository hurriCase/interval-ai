using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.UI.Windows.Base.PopUp;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Category
{
    internal sealed class CategoryPopUp : ParameterizedPopUpBase<CategoryEntry>
    {
        [SerializeField] private RectTransform _wordsContainer;
        [SerializeField] private WordItem _wordItem;

        internal override void Show()
        {
            foreach (var wordEntry in Parameters.WordEntries)
            {
                var createdWord = Instantiate(_wordItem, _wordsContainer);
                createdWord.Init(wordEntry);
            }
        }
    }
}